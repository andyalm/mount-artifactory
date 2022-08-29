using Autofac;
using MountAnything;
using MountAnything.Routing;

namespace MountArtifactory;

public class Provider : IMountAnythingProvider
{
    public Router CreateRouter()
    {
        var root = Router.Create<RootHandler>();
        root.Map<RepositoryHandler>(repository =>
        {
            repository.MapRegex<FileHandler>(@"[a-zA-Z0-9\s-_]+");
        });
        root.RegisterServices(builder =>
        {
            builder.Register(c =>
            {
                var endpoint = Environment.GetEnvironmentVariable("ARTIFACTORY_ENDPOINT");
                if (string.IsNullOrEmpty(endpoint))
                {
                    throw new Exception("ARTIFACTORY_ENDPOINT environment variable is required");
                }

                return new HttpClient
                {
                    BaseAddress = new Uri(endpoint)
                };
            });
        });

        return root;
    }

    public IEnumerable<DefaultDrive> GetDefaultDrives()
    {
        yield return new DefaultDrive("artifactory");
    }
}