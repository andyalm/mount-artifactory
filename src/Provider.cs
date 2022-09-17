using Autofac;
using MountAnything;
using MountAnything.Routing;

namespace MountArtifactory;

public class Provider : IMountAnythingProvider
{
    public Router CreateRouter()
    {
        var root = Router.Create<RootHandler>();
        root.Map<RepositoryHandler, RepositoryName>(repository =>
        {
            repository.MapRecursive<FileHandler, FilePath>();
        });
        root.RegisterServices(builder =>
        {
            builder.RegisterType<ArtifactoryClient>();
        });

        return root;
    }

    public IEnumerable<DefaultDrive> GetDefaultDrives()
    {
        if (!string.IsNullOrEmpty(ArtifactoryClient.Env.ARTIFACTORY_ENDPOINT))
        {
            yield return new DefaultDrive("artifactory");
        }
    }
}