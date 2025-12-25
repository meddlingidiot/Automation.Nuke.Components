using Automation.Nuke.Components.Parameters;
using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Automation.Nuke.Components.Components;

public interface ICompile : INukeBuild, IHasSolution, IHasConfiguration, IHasGitVersion, IHasTests
{
    Target Compile => t => t
        .DependsOn<IRestore>()
        .Description("Compile Solution")
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .SetTreatWarningsAsErrors(BreakBuildOnWarnings)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion));
        });
}
    