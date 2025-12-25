using Automation.Nuke.Components.Components;
using Automation.Nuke.Components.Parameters;
using Nuke.Common;

namespace Automation.Nuke.Components.DefaultBuilds;

public class TestBuild : AzurePipelinesBuild, IShowVersion, IClean, ICompile, IRestore, IScanForSecrets, 
    IRunUnitTests, IRunIntegrationTests, IGenerateCoverageReport, ITest, IUpdateChangelog,
    IPackage, IVelopack, ITagRelease, IAnnounceRelease
{
    public static int Main() => Execute<TestBuild>(
        x => ((ITest)x).Test);

    int IHasTests.MinCoverageThreshold => 20;
    //bool IHasTests.BreakBuildOnWarnings => false;

}