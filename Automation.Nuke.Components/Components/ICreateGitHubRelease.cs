using Automation.Nuke.Components.Parameters;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitHub;

namespace Automation.Nuke.Components.Components;

/// <summary>
/// Creates a GitHub release and comments on milestone issues after a tag is pushed.
/// Requires <see cref="ITagRelease"/> to be implemented on the build.
/// </summary>
public interface ICreateGitHubRelease : INukeBuild, IHasGitVersion, IHasGitHubPackages
{
    [GitRepository]
    GitRepository GitRepository => TryGetValue(() => GitRepository);

    /// <summary>
    /// The milestone title to look up issues for. Defaults to the current version tag (e.g. "v1.2.3").
    /// Override this to use a different milestone naming convention.
    /// </summary>
    string MilestoneTitle => $"v{GitVersion.MajorMinorPatch}";

    Target CreateGitHubRelease => _ => _
        .TriggeredBy<ITagRelease>(x => x.TagRelease)
        .ProceedAfterFailure()
        .OnlyWhenStatic(() => GitRepository.IsOnMasterBranch())
        .OnlyWhenStatic(() => GitHubActions.Instance != null)
        .Executes(async () =>
        {
            var issues = await GitRepository.GetGitHubMilestoneIssues(MilestoneTitle);
            foreach (var issue in issues)
                await GitHubActions.Instance.CreateComment(issue.Number, $"Released in {MilestoneTitle}! 🎉");
        });
}
