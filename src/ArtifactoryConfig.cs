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
        if (!endpoint.EndsWith("/"))
        {
            endpoint += "/";
        }
        
        var config = new ArtifactoryConfig
        {
            EndpointUri = new Uri(endpoint)
        };
        
        config.ApiKey = Environment.GetEnvironmentVariable(Env.ARTIFACTORY_API_KEY);

        return config;
    }
    
    public Uri EndpointUri { get; set; }
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