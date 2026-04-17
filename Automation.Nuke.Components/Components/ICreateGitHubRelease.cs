using Automation.Nuke.Components.Parameters;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitHub;
using Octokit;

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
        .OnlyWhenStatic(() => GitRepository.IsOnMainOrMasterBranch())
        .OnlyWhenStatic(() => GitHubActions.Instance != null)
        .Executes(async () =>
        {
            var client = new GitHubClient(new ProductHeaderValue("nuke-build"))
            {
                Credentials = new Credentials(GitHubToken)
            };

            var owner = GitRepository.GetGitHubOwner();
            var repoName = GitRepository.GetGitHubName();

            var milestones = await client.Issue.Milestone.GetAllForRepository(owner, repoName);
            var milestone = milestones.FirstOrDefault(m => m.Title == MilestoneTitle);
            var issues = milestone != null
                ? await client.Issue.GetAllForRepository(owner, repoName,
                    new RepositoryIssueRequest { Milestone = milestone.Number.ToString(), State = ItemStateFilter.All })
                : [];

            var releaseNotes = issues.Count > 0
                ? "## Issues\n\n" + string.Join("\n", issues.Select(i => $"- #{i.Number} {i.Title}"))
                : string.Empty;

            var release = await client.Repository.Release.Create(
                owner,
                repoName,
                new NewRelease(MilestoneTitle)
                {
                    Name = MilestoneTitle,
                    Body = releaseNotes,
                    Draft = false,
                    Prerelease = false,
                });

            Serilog.Log.Information("GitHub release created: {Url}", release.HtmlUrl);

            foreach (var issue in issues)
                await client.Issue.Comment.Create(owner, repoName, issue.Number, $"Released in [{MilestoneTitle}]({release.HtmlUrl})! 🎉");
        });
}
