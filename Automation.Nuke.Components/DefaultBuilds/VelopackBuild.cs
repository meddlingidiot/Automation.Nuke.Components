using Automation.Nuke.Components.Components;
using Automation.Nuke.Components.Parameters;
using Nuke.Common;

namespace Automation.Nuke.Components.DefaultBuilds;

public class VelopackBuild : AzurePipelinesBuild, IShowVersion,
    IClean, ICompile, IRestore, IScanForSecrets, IRunUnitTests, IRunIntegrationTests, 
    IGenerateCoverageReport, ITest, IUpdateChangelog,
    IVelopack, ITagRelease, IAnnounceRelease
{
    public static int Main() => Execute<VelopackBuild>(
        y => ((IVelopack)y).ReleaseVelopack);

    //string IHasVelopack.VelopackProjectName => "Automation.NukeWpfAndPackageExample.WpfExample";
    //string IHasVelopack.VelopackIconPath => "Automation.NukeWpfANdPackageExample.WpfExample/assets/reset-password.ico";
    
    int IHasTests.MinCoverageThreshold => 20;
    
    //bool IHasTests.BreakBuildOnWarnings => false;

}