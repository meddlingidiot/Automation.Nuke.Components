using System.Text.Json;
using Automation.Nuke.Components.Parameters;
using Nuke.Common;
using Nuke.Common.IO;
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
        });
}
