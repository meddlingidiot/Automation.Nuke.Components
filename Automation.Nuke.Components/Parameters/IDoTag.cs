using Nuke.Common;
using Nuke.Common.IO;

namespace Automation.Nuke.Components.Parameters;

public interface IDoTag : INukeBuild
{
    [Parameter("Force tag for local builds - Default is 'false'")]
    bool ForceTagRelease => TryGetValue<bool?>(() => ForceTagRelease) ?? false;

}