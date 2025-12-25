using Automation.Nuke.Components.Parameters;
using Nuke.Common;
using Nuke.Common.Tools.DotNet;

namespace Automation.Nuke.Components.Components;

public interface IRestore : INukeBuild, IHasSolution
{
    Target Restore => t => t
        .DependsOn<IClean>()
        .Description("Restore NuGet packages")
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(s => DotNetRestoreSettingsExtensions
                .SetProjectFile<DotNetRestoreSettings>(s, (string)Solution));
        });
}