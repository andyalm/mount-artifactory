using System.Management.Automation;
using Microsoft.Extensions.DependencyInjection;
using MountAnything;
using MountAnything.Routing;

namespace MountArtifactory;

public class Provider : MountAnythingProvider<ArtifactoryDriveParameters>
{
    public override Router CreateRouter()
    {
        var root = Router.Create<RootHandler>();
        root.Map<RepositoryHandler, RepositoryName>(repository =>
        {
            repository.MapRecursive<FileHandler, FilePath>();
        });
        root.ConfigureServices(services =>
        {
            services.AddDriveInfo<ArtifactoryDriveInfo>();
            services.AddTransient(s => s.GetRequiredService<ArtifactoryDriveInfo>().ArtifactoryConfig);
            services.AddTransient<ArtifactoryClient>();
        });

        return root;
    }

    public override IEnumerable<PSDriveInfo> GetDefaultDrives(ProviderInfo providerInfo)
    {
        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(ArtifactoryConfig.Env.ARTIFACTORY_ENDPOINT)))
        {
            yield return new ArtifactoryDriveInfo(ArtifactoryConfig.FromEnv(), new PSDriveInfo("artifactory", providerInfo, "", "Navigate artifactory as a virtual filesystem", null));
        }
    }
    
    protected override PSDriveInfo NewDrive(PSDriveInfo driveInfo, ArtifactoryDriveParameters parameters)
    {
        if (driveInfo is ArtifactoryDriveInfo)
        {
            return driveInfo;
        }
        
        var artifactoryConfig = new ArtifactoryConfig(ArtifactoryConfig.ConstructUri(parameters.ArtifactoryEndpoint))
        {
            ApiKey = parameters.ApiKey
        };
        return new ArtifactoryDriveInfo(artifactoryConfig, driveInfo);
    }
}