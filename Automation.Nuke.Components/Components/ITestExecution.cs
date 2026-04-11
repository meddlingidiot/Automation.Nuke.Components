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
    /// Set to true when using TUnit or any other Microsoft.Testing.Platform (MTP) based test framework.
    /// Required on .NET 10+ SDK where the legacy VSTest protocol is no longer supported by MTP.
    /// </summary>
    /// <example>
    /// In your build class:
    /// <code>bool ITestExecution.UseMicrosoftTestingPlatform => true;</code>
    /// </example>
    bool UseMicrosoftTestingPlatform => false;

    /// <summary>
    /// Shared test execution logic. Override this to customize how tests are discovered and run.
    /// </summary>
    /// <param name="projectNameFilter">Filter for test project names (e.g., "UnitTests", "IntegrationTests")</param>
    void RunTests(string projectNameFilter)
    {
        if (UseMicrosoftTestingPlatform)
            RunTestsMtp(projectNameFilter);
        else
            RunTestsVsTest(projectNameFilter);
    }

    private void RunTestsVsTest(string projectNameFilter)
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

    private void RunTestsMtp(string projectNameFilter)
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
