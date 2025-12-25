using Automation.Nuke.Components.Components;
using Automation.Nuke.Components.Parameters;
using Nuke.Common;

namespace Automation.Nuke.Components.DefaultBuilds;

public class PackageBuild : AzurePipelinesBuild, IShowVersion,
    IClean, ICompile, IRestore, IScanForSecrets, IRunUnitTests, IRunIntegrationTests, 
    IGenerateCoverageReport, ITest, IUpdateChangelog,
    IPackage, ITagRelease, IAnnounceRelease
{
    public static int Main() => Execute<PackageBuild>(
        x => ((IPackage)x).ReleasePackage);

    int IHasTests.MinCoverageThreshold => 20;
    //bool IHasTests.BreakBuildOnWarnings => false;

}
