using System.Net.Http;
using System.Text.Json;
using Automation.Nuke.Components.Parameters;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.ReportGenerator;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

namespace Automation.Nuke.Components.Components;

/// <summary>
/// Generates code coverage reports from test results.
/// Override the CoverageReport target to customize report generation.
/// </summary>
public interface IGenerateCoverageReport : INukeBuild, IHasTests, IHasArtifacts
{
    Target CoverageReport => t => t
        .DependsOn<IRunUnitTests>(x => x.UnitTests)
        .DependsOn<IRunIntegrationTests>(x => x.IntegrationTests)
        .Description("Generate coverage report")
        .Executes(() =>
        {
            // Ensure we have coverage files before running the tool
            if (TestResultDirectory.GlobFiles("**/coverage.cobertura.xml").Count == 0)
            {
                Serilog.Log.Warning("No coverage files found in {TestResultDirectory}. Skipping ReportGenerator.", TestResultDirectory);
                return;
            }

            // Combine all coverage.cobertura.xml files into one report
            ReportGenerator(s => s
                .SetReports(TestResultDirectory / "**" / "coverage.cobertura.xml")
                .SetTargetDirectory(CoverageReportDirectory)
                .SetReportTypes(ReportTypes.HtmlInline, ReportTypes.Cobertura, ReportTypes.JsonSummary));

            // Read the JSON summary to get coverage percentage
            var summaryFile = CoverageReportDirectory / "Summary.json";
            var json = JsonDocument.Parse(File.ReadAllText(summaryFile));
            var lineCoverage = json.RootElement
                .GetProperty("summary")
                .GetProperty("linecoverage")
                .GetDouble();

            Serilog.Log.Information("Line Coverage: {Coverage:F2}%", lineCoverage);

            if (lineCoverage < MinCoverageThreshold)
            {
                throw new Exception($"Coverage {lineCoverage:F2}% is below threshold {MinCoverageThreshold}%");
            }

            if (UploadToCodecov)
            {
                UploadCoverageToCodecov(CoverageReportDirectory / "Cobertura.xml");
            }
        });

    private void UploadCoverageToCodecov(AbsolutePath coberturaFile)
    {
        var commit = Environment.GetEnvironmentVariable("GITHUB_SHA")
                     ?? ProcessTasks.StartProcess("git", "rev-parse HEAD").AssertWaitForExit()
                         .Output.Select(x => x.Text).LastOrDefault()?.Trim()
                     ?? throw new Exception("Could not determine git commit SHA for Codecov upload");

        var branch = Environment.GetEnvironmentVariable("GITHUB_HEAD_REF")
                     ?? Environment.GetEnvironmentVariable("GITHUB_REF_NAME")
                     ?? ProcessTasks.StartProcess("git", "branch --show-current").AssertWaitForExit()
                         .Output.Select(x => x.Text).LastOrDefault()?.Trim()
                     ?? "main";

        var slug = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY")
                   ?? ParseSlugFromRemote();

        var query = $"commit={Uri.EscapeDataString(commit)}&branch={Uri.EscapeDataString(branch)}";
        if (slug is not null)
            query += $"&slug={Uri.EscapeDataString(slug)}";
        if (!string.IsNullOrEmpty(CodecovToken))
            query += $"&token={Uri.EscapeDataString(CodecovToken)}";

        using var http = new HttpClient();
        http.DefaultRequestHeaders.Add("Accept", "text/plain");
        http.DefaultRequestHeaders.Add("X-Reduced-Redundancy", "false");

        Serilog.Log.Information("Requesting Codecov upload URL...");
        var response = http.GetAsync($"https://codecov.io/upload/v4?{query}").GetAwaiter().GetResult();

        if (!response.IsSuccessStatusCode)
        {
            var error = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            throw new Exception($"Codecov upload URL request failed ({response.StatusCode}): {error}");
        }

        var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        var lines = body.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var uploadUrl = lines[0].Trim();

        Serilog.Log.Information("Uploading coverage report to Codecov...");
        var fileContent = new StringContent(File.ReadAllText(coberturaFile), System.Text.Encoding.UTF8, "text/plain");
        var putResponse = http.PutAsync(uploadUrl, fileContent).GetAwaiter().GetResult();

        if (!putResponse.IsSuccessStatusCode)
        {
            var error = putResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            throw new Exception($"Codecov file upload failed ({putResponse.StatusCode}): {error}");
        }

        if (lines.Length > 1)
            Serilog.Log.Information("Coverage uploaded: {Url}", lines[1].Trim());
        else
            Serilog.Log.Information("Coverage uploaded successfully");
    }

    private static string? ParseSlugFromRemote()
    {
        var remote = ProcessTasks.StartProcess("git", "remote get-url origin").AssertWaitForExit()
            .Output.Select(x => x.Text).LastOrDefault()?.Trim();

        if (string.IsNullOrEmpty(remote))
            return null;

        // https://github.com/owner/repo.git  or  git@github.com:owner/repo.git
        if (remote.StartsWith("https://"))
        {
            var path = new Uri(remote).AbsolutePath.TrimStart('/').TrimEnd('/');
            if (path.EndsWith(".git")) path = path[..^4];
            return path;
        }

        var colonIdx = remote.LastIndexOf(':');
        if (colonIdx >= 0)
        {
            var path = remote[(colonIdx + 1)..].TrimEnd('/');
            if (path.EndsWith(".git")) path = path[..^4];
            return path;
        }

        return null;
    }
}
