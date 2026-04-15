using Nuke.Common;
using Nuke.Common.ProjectModel;
using Automation.Nuke.Components;
using Automation.Nuke.Components.Components;
using Automation.Nuke.Components.DefaultBuilds;
using Automation.Nuke.Components.Parameters;

/// <summary>
/// Build configuration for PackageAndVelopackBuild
/// </summary>
/// Support plugins are available for:
///   - JetBrains ReSharper        https://nuke.build/resharper
///   - JetBrains Rider            https://nuke.build/rider
///   - Microsoft VisualStudio     https://nuke.build/visualstudio
///   - Microsoft VSCode           https://nuke.build/vscode

public class Build : GitHubActionsBuild, IShowVersion, IClean, ICompile, IRestore, IScanForSecrets, IRunUnitTests, IRunIntegrationTests, IGenerateCoverageReport, ITest, IUpdateChangelog, IPackage, ITagRelease, IAnnounceRelease
{

    public static int Main() => Execute<Build>(
        x => ((IPackage)x).ReleasePackage);
        
    string IHasGitHubPackages.GitHubOwner => "meddlingidiot";
    int IHasTests.MinCoverageThreshold => 35;
    bool ITestExecution.UseMicrosoftTestingPlatform => true;
    bool IHasTests.UploadToCodecov => true;
 }
