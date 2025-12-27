using Nuke.Common;

namespace Automation.Nuke.Components.Parameters;

/// <summary>
/// Provides GitHub Actions-specific configuration parameters.
/// </summary>
public interface IGitHubActionsConfig : INukeBuild
{
    [Parameter(".NET SDK version to install")]
    string DotNetSdkVersion => TryGetValue(() => DotNetSdkVersion) ?? "10.0.x";
}
