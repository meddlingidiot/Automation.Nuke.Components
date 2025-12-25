using Automation.Nuke.Components.Parameters;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;

namespace Automation.Nuke.Components.Components;

public interface IClean : INukeBuild, IHasSolution, IHasConfiguration, IHasArtifacts
{
    Target Clean => t => t
        .Description("Clean build artifacts")
        .Executes(() =>
        {
            // Clean solution output directories (bin/obj)
            DotNetTasks.DotNetClean(s => DotNetCleanSettingsExtensions
                .SetProject<DotNetCleanSettings>(s, (string)Solution)
                .SetConfiguration(Configuration));

            // Delete artifacts directory if it exists
            ArtifactsDirectory.CreateOrCleanDirectory();

            // Optionally clean specific directories
            TestResultDirectory.CreateOrCleanDirectory();
            PackagePublishDirectory.CreateOrCleanDirectory();
            CoverageReportDirectory.CreateOrCleanDirectory();
            VelopackPublishDirectory.CreateOrCleanDirectory();
        });
}