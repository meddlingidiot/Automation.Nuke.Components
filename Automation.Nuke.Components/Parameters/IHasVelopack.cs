using Nuke.Common;

namespace Automation.Nuke.Components.Parameters;

public interface IHasVelopack: INukeBuild
{
    [Parameter] string VelopackProjectName => TryGetValue(() => VelopackProjectName);
    [Parameter] string VelopackIconPath => TryGetValue(() => VelopackIconPath) ?? 
                                           "";
    [Parameter] string VelopackChannel => TryGetValue(() => VelopackChannel) ?? 
                                          "win";
    [Parameter] string VelopackBlobContainer => TryGetValue(() => VelopackBlobContainer) ?? 
                                                "installers";
    [Parameter] string AzureBlobAccount => TryGetValue(() => AzureBlobAccount) ?? 
                                           "meddlingidiotinstallers";
    
    [Parameter] string AzureBlobEndpoint => TryGetValue(() => AzureBlobEndpoint) ?? 
                                            "https://meddlingidiotinstallers.blob.core.windows.net";
    [Secret] string AzureBlobSasToken => TryGetValue(() => AzureBlobSasToken);
    [Parameter] string AzureBlobSasTokenLocal => TryGetValue(() => AzureBlobSasTokenLocal) ?? 
                                                 "sp=racwdl&st=2025-12-28T02:30:01Z&se=2029-12-31T10:45:01Z&spr=https&sv=2024-11-04&sr=c&sig=2RTMTJ8fLUFu80HoDRhzEmUargNMveQzK%2BX%2BcdjRF7A%3D";
    [Parameter] string AzureBlobTimeout => TryGetValue(() => AzureBlobTimeout) ?? 
                                           "30";
    [Parameter] int KeepMaxReleases => TryGetValue<int?>(() => KeepMaxReleases) ?? 3;
}