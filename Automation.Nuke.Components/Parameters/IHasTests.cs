using Nuke.Common;

namespace Automation.Nuke.Components.Parameters;

public interface IHasTests : INukeBuild
{
    [Parameter("Break build on secret leaks - Default is 'true'")]
    bool BreakBuildOnSecretLeaks => TryGetValue<bool?>(() => BreakBuildOnSecretLeaks) ?? true;
    
    [Parameter("Break build on warnings - Default is 'true'")]
    bool BreakBuildOnWarnings => TryGetValue<bool?>(() => BreakBuildOnWarnings) ?? true;

    [Parameter] int MinCoverageThreshold => TryGetValue<int?>(() => MinCoverageThreshold) ?? 0;
}
