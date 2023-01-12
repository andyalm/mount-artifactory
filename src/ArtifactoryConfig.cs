namespace MountArtifactory;

public class ArtifactoryConfig
{
    public static ArtifactoryConfig FromEnv()
    {
        var endpoint = Environment.GetEnvironmentVariable(Env.ARTIFACTORY_ENDPOINT);
        if (string.IsNullOrEmpty(endpoint))
        {
            throw new Exception($"{Env.ARTIFACTORY_ENDPOINT} environment variable is required");
        }

        return new ArtifactoryConfig(ConstructUri(endpoint))
        {
            ApiKey = Environment.GetEnvironmentVariable(Env.ARTIFACTORY_API_KEY)
        };
    }

    public static Uri ConstructUri(string endpoint)
    {
        if (!endpoint.EndsWith("/"))
        {
            endpoint += "/";
        }

        return new Uri(endpoint);
    }
    
    public ArtifactoryConfig(Uri endpointUri)
    {
        EndpointUri = endpointUri;
    }
    
    public Uri EndpointUri { get; }
    public string? ApiKey { get; set; }

    public Uri RepositoryUri(string repositoryName)
    {
        return new Uri($"{EndpointUri}{repositoryName}");
    }
    
    public static class Env
    {
        public const string ARTIFACTORY_ENDPOINT = nameof(ARTIFACTORY_ENDPOINT);
        public const string ARTIFACTORY_API_KEY = nameof(ARTIFACTORY_API_KEY);
    }
}