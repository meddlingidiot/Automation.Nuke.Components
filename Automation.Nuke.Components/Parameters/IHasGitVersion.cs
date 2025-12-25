using Nuke.Common;
using Nuke.Common.Tools.GitVersion;

namespace Automation.Nuke.Components.Parameters;

public interface IHasGitVersion : INukeBuild
{
    [GitVersion(Framework = "net8.0")] 
    GitVersion GitVersion => TryGetValue(() => GitVersion);
}
