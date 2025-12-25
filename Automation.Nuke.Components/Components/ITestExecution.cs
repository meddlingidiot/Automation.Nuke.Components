using Automation.Nuke.Components.Parameters;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Automation.Nuke.Components.Components;

/// <summary>
/// Base interface for test execution with shared logic.
/// Inherit from this to create custom test execution strategies.
/// </summary>
public interface ITestExecution : INukeBuild, IHasSolution, IHasConfiguration, IHasTests, IHasArtifacts
{
    /// <summary>
    /// Shared test execution logic. Override this to customize how tests are discovered and run.
    /// </summary>
    /// <param name="projectNameFilter">Filter for test project names (e.g., "UnitTests", "IntegrationTests")</param>
    void RunTests(string projectNameFilter)
    {
        DotNetTest(s => s
            .SetConfiguration(Configuration)
            .EnableNoBuild()
            .SetResultsDirectory(TestResultDirectory)
            .SetDataCollector("XPlat Code Coverage")
            .CombineWith(
                Solution.AllProjects.Where(p => p.Name.Contains(projectNameFilter)),
                (settings, project) => settings
                    .SetProjectFile(project)
                    .SetResultsDirectory(TestResultDirectory)
                    .SetLoggers($"trx;LogFileName={project.Name}.trx")
            )
        );
    }
}
