using Nuke.Common;

namespace Automation.Nuke.Components.Parameters;

public interface IHasConfiguration : INukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    Configuration Configuration => TryGetValue(() => Configuration) ??
                                   (IsLocalBuild ? Configuration.Debug : Configuration.Release);
}
