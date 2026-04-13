using Nuke.Common;

namespace Automation.Nuke.Components.Components;

/// <summary>
/// Orchestrates all TUnit testing steps using Microsoft.Testing.Platform (MTP).
/// Use this in place of <see cref="ITest"/> when your test projects use TUnit on .NET 10+.
///
/// To customize individual steps, implement the specific interfaces:
/// - <see cref="ITUnitTestExecution"/> - Override RunTests() to change how all tests execute
/// - <see cref="IRunTUnitUnitTests"/> - Replace just unit test execution
/// - <see cref="IRunTUnitIntegrationTests"/> - Replace just integration test execution
/// - <see cref="IGenerateTUnitCoverageReport"/> - Replace just coverage reporting
/// </summary>
public interface ITUnitTest : IRunTUnitUnitTests, IRunTUnitIntegrationTests, IGenerateTUnitCoverageReport
{
    /// <summary>
    /// Main test target that runs all TUnit tests and generates coverage report.
    /// Depends on secret scanning to ensure no secrets are committed.
    /// </summary>
    Target Test => t => t
        .DependsOn<IScanForSecrets>(x => x.ScanForSecrets)
        .DependsOn<IGenerateTUnitCoverageReport>(x => x.CoverageReport)
        .Before<IUpdateChangelog>(x => x.UpdateChangelog)
        .Description("Run all TUnit tests with coverage")
        .Executes(() => { });
}
