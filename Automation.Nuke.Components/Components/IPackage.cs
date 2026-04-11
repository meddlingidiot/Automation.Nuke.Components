using Automation.Nuke.Components.DefaultBuilds;
using Automation.Nuke.Components.Parameters;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Nuke.Common.Utilities;
using static Nuke.Common.Tools.DotNet.DotNetTasks;


namespace Automation.Nuke.Components.Components;

public interface IPackage : INukeBuild, IVelopack, IHasSolution, IHasConfiguration,
    IHasGitVersion, IHasArtifacts, IHasGitHubPackages, IDoTag
{
    Target Package => t => t
        .DependsOn<ITest>(x => x.Test)
        .DependsOn<IUpdateChangelog>()
        .Description("Package NuGet packages")
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution)
                .SetConfiguration(Configuration)
                .SetOutputDirectory(PackagePublishDirectory)
                // Safeguard: fall back to a local version if GitVersion isn't available
                .SetVersion(GitVersion.FullSemVer ?? "0.0.0-local"));
                //.EnableNoBuild()); //see if this fixes it

        });
    
    Target ReleasePackage => t => t
        .DependsOn(Package)
        .When(IsServerBuild || ForceTagRelease, _ => _
            .Triggers<ITagRelease>(x => x.TagRelease))
        .Description("Deploy NuGet packages to GitHub Packages")
        .Executes(() =>
        {
            if (!IsServerBuild && !ForceTagRelease)
            {
                Serilog.Log.Information("Skipping NuGet push — not a server build. Use --force-tag-release to push locally.");
                return;
            }

            Serilog.Log.Information("Deploying NuGet packages to GitHub Packages...");

            var currentBranch = GitTasks.GitCurrentBranch();
            var isMainBranch = currentBranch.Equals("main", StringComparison.OrdinalIgnoreCase);
            var feedName = isMainBranch ? "Production" : "Prerelease";

            var packages = ArtifactsDirectoryParam.GlobFiles("**/*.nupkg");

            foreach (var package in packages)
            {
                Serilog.Log.Information("Pushing {Package} to GitHub Packages ({Feed})", package.Name, feedName);
                DotNetTasks.DotNetNuGetPush(s => s
                    .SetTargetPath(package)
                    .SetSource($"https://nuget.pkg.github.com/{GitHubOwner}/index.json")
                    .SetApiKey(GitHubToken)
                    .SetSkipDuplicate(true));
            }
        });
}

