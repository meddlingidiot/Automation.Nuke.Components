using System.Text.Json;
using Automation.Nuke.Components.Parameters;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.ReportGenerator;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

namespace Automation.Nuke.Components.Components;

/// <summary>
/// Generates code coverage reports from TUnit test results.
/// Depends on TUnit-specific test targets rather than the VSTest-based ones.
/// Override the CoverageReport target to customize report generation.
/// </summary>
public interface IGenerateTUnitCoverageReport : INukeBuild, IHasTests, IHasArtifacts
{
    Target CoverageReport => t => t
        .DependsOn<IRunTUnitUnitTests>(x => x.TUnitUnitTests)
        .DependsOn<IRunTUnitIntegrationTests>(x => x.TUnitIntegrationTests)
        .Description("Generate coverage report")
        .Executes(() =>
        {
            if (TestResultDirectory.GlobFiles("**/coverage.cobertura.xml").Count == 0)
            {
                Serilog.Log.Warning("No coverage files found in {TestResultDirectory}. Skipping ReportGenerator.", TestResultDirectory);
                return;
            }

            ReportGenerator(s => s
                .SetReports(TestResultDirectory / "**" / "coverage.cobertura.xml")
                .SetTargetDirectory(CoverageReportDirectory)
                .SetReportTypes(ReportTypes.HtmlInline, ReportTypes.Cobertura, ReportTypes.JsonSummary));

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
