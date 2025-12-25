using Automation.Nuke.Components.Components;
using Nuke.Common;

namespace Automation.Nuke.Components.DefaultBuilds;

public class CompileBuild : AzurePipelinesBuild, IShowVersion, IClean, ICompile, IRestore, IScanForSecrets
{
    public static int Main() => Execute<CompileBuild>(x => ((ICompile)x).Compile); 

    //bool IHasTests.BreakBuildOnWarnings => false;

}