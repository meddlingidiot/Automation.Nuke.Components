using System.Runtime.InteropServices;
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
                EnsureCodecovUploaderInstalled().Wait();
                UploadCoverageToCodecov(CoverageReportDirectory / "Cobertura.xml");
            }
        });

    AbsolutePath CodecovUploaderPath
    {
        get
        {
            var exe = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "codecov.exe" : "codecov";
            return TemporaryDirectory / "codecov" / exe;
        }
    }

    private async Task EnsureCodecovUploaderInstalled()
    {
        if (CodecovUploaderPath.FileExists()) return;

        var os = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "windows"
               : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux"
               : "macos";
        var exe = os == "windows" ? "codecov.exe" : "codecov";
        var downloadUrl = $"https://uploader.codecov.io/latest/{os}/{exe}";

        Serilog.Log.Information("Downloading Codecov uploader from {Url}...", downloadUrl);

        var installDir = TemporaryDirectory / "codecov";
        installDir.CreateDirectory();

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "nuke-build");
        var bytes = await client.GetByteArrayAsync(downloadUrl);
        await File.WriteAllBytesAsync(CodecovUploaderPath, bytes);

        if (os != "windows")
            ProcessTasks.StartProcess("chmod", $"+x {CodecovUploaderPath}").AssertZeroExitCode();

        Serilog.Log.Information("Codecov uploader installed to {Path}", CodecovUploaderPath);
    }

    private void UploadCoverageToCodecov(AbsolutePath coberturaFile)
    {
        var args = $"--file \"{coberturaFile}\" --rootDir \"{RootDirectory}\"";

        if (!string.IsNullOrEmpty(CodecovToken))
            args += $" --token \"{CodecovToken}\"";

        var githubRepo = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
        if (!string.IsNullOrEmpty(githubRepo))
            args += $" --slug \"{githubRepo}\"";

        var commit = Environment.GetEnvironmentVariable("GITHUB_SHA");
        if (!string.IsNullOrEmpty(commit))
            args += $" --sha \"{commit}\"";

        var branch = Environment.GetEnvironmentVariable("GITHUB_HEAD_REF")
                     ?? Environment.GetEnvironmentVariable("GITHUB_REF_NAME");
        if (!string.IsNullOrEmpty(branch))
            args += $" --branch \"{branch}\"";

        Serilog.Log.Information("Uploading coverage to Codecov...");
        ProcessTasks.StartProcess(CodecovUploaderPath, args, logOutput: true).AssertZeroExitCode();
    }
}
