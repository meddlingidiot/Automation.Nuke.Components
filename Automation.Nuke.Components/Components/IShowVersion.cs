using Automation.Nuke.Components.Parameters;
using Nuke.Common;

namespace Automation.Nuke.Components.Components;

public interface IShowVersion : INukeBuild, IHasGitVersion
{
    Target ShowVersion => t => t
        .DependentFor<IClean>()
        .Executes(() => { Serilog.Log.Information("GitVersion: {GitVersion}", GitVersion.FullSemVer); });
}