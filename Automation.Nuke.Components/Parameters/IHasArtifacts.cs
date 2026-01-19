using Nuke.Common;
using Nuke.Common.IO;

namespace Automation.Nuke.Components.Parameters;

public interface IHasArtifacts : INukeBuild
{
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    [Parameter] AbsolutePath ArtifactsDirectoryParam => TryGetValue(() => ArtifactsDirectoryParam) ?? ArtifactsDirectory;
    AbsolutePath CoverageReportDirectory => ArtifactsDirectoryParam / "coverage-report";
    AbsolutePath TestResultDirectory => ArtifactsDirectoryParam / "test-results";
    AbsolutePath PackagePublishDirectory => ArtifactsDirectoryParam / "packages";
    AbsolutePath VelopackPublishDirectory => RootDirectory / ".tmp" / "velopack";
}
