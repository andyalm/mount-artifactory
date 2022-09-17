using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using MountAnything;

namespace MountArtifactory;

public class ArtifactoryClient : IDisposable
{
    private readonly HttpClient _client;
    
    public ArtifactoryClient(IPathHandlerContext context)
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

        _client = new HttpClient(new DebugLoggingHandler(context, new HttpClientHandler()))
        {
            BaseAddress = new Uri(endpoint)
        };
        var apiKey = Environment.GetEnvironmentVariable(Env.ARTIFACTORY_API_KEY);
        if (!string.IsNullOrEmpty(apiKey))
        {
            _client.DefaultRequestHeaders.Add("X-JFrog-Art-Api", apiKey);
        }   
    }

    public Repository[] GetRepositories()
    {
        return _client.GetJson<Repository[]>("api/repositories");
    }
    
    public Repository? GetRepository(string repositoryName)
    {
        try
        {
            return _client.GetJson<Repository>($"api/repositories/{repositoryName}");
        }
        catch (HttpRequestException ex) when(ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public File? GetFile(string repositoryName, ItemPath? filePath = null)
    {
        var requestUri = $"api/storage/{repositoryName}";
        if (filePath != null)
        {
            requestUri += $"/{filePath}";
        }
        try
        {
            return _client.GetJson<File>(requestUri);
        }
        catch (HttpRequestException ex) when(ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public void Delete(string repositoryName, ItemPath filePath)
    {
        _client.Delete($"{repositoryName}/{filePath}");
    }

    public class DebugLoggingHandler : DelegatingHandler
    {
        private readonly IPathHandlerContext _context;

        public DebugLoggingHandler(IPathHandlerContext context, HttpMessageHandler innerHandler) : base(innerHandler)
        {
            _context = context;
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _context.WriteDebug(ToMessage(request));
            Stopwatch timer = new Stopwatch();
            timer.Start();
            var response = base.Send(request, cancellationToken);
            _context.WriteDebug($"{response.StatusCode:D} ({response.StatusCode}) in {timer.ElapsedMilliseconds}ms");

            return response;
        }

        private string ToMessage(HttpRequestMessage request)
        {
            return $"{request.Method.Method} {request.RequestUri}";
        }
    }

    public void Dispose()
    {
        _client.Dispose();
    }
    
    public static class Env
    {
        public const string ARTIFACTORY_ENDPOINT = nameof(ARTIFACTORY_ENDPOINT);
        public const string ARTIFACTORY_API_KEY = nameof(ARTIFACTORY_API_KEY);
    }
}