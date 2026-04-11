using Automation.Nuke.Components.Parameters;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Automation.Nuke.Components.Components;

/// <summary>
/// Base interface for TUnit test execution using Microsoft.Testing.Platform (MTP).
/// Required for TUnit on .NET 10+ where the legacy VSTest protocol is no longer supported.
/// Inherit from this to create custom TUnit test execution strategies.
/// </summary>
public interface ITUnitTestExecution : INukeBuild, IHasSolution, IHasConfiguration, IHasTests, IHasArtifacts
{
    /// <summary>
    /// Shared TUnit test execution logic using MTP-compatible dotnet test invocation.
    /// Override this to customize how TUnit tests are discovered and run.
    /// </summary>
    /// <param name="projectNameFilter">Filter for test project names (e.g., "UnitTests", "IntegrationTests")</param>
    void RunTests(string projectNameFilter)
    {
        DotNetTest(s => s
            .SetConfiguration(Configuration)
            .EnableNoBuild()
            .CombineWith(
                Solution.AllProjects.Where(p => p.Name.Contains(projectNameFilter)),
                (settings, project) => settings
                    .SetProjectFile(project)
                    .SetProperty("TestingPlatformDotnetTestSupport", "true")
                    .SetProperty("TestingPlatformCommandLineArguments",
                        $"--coverage --coverage-output-format cobertura " +
                        $"--coverage-output \"{TestResultDirectory / project.Name / "coverage.cobertura.xml"}\" " +
                        $"--report-trx --report-trx-filename \"{TestResultDirectory / (project.Name + ".trx")}\"")
            )
        );
    }
}
