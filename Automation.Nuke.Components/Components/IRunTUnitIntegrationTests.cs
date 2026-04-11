using Nuke.Common;

namespace Automation.Nuke.Components.Components;

/// <summary>
/// Runs integration tests using TUnit / Microsoft.Testing.Platform.
/// Inherits from <see cref="ITUnitTestExecution"/> for shared TUnit test logic.
/// Override <see cref="ITUnitTestExecution.RunTests"/> to customize test execution behavior.
/// </summary>
public interface IRunTUnitIntegrationTests : ITUnitTestExecution
{
    Target TUnitIntegrationTests => t => t
        .DependsOn<ICompile>(x => x.Compile)
        .Description("Run TUnit integration tests")
        .Executes(() => RunTests("IntegrationTests"));
}
