using System.Diagnostics;
using System.Net;
using MountAnything;

namespace MountArtifactory;

public class ArtifactoryClient : IDisposable
{
    private readonly HttpClient _client;
    
    public ArtifactoryClient(ArtifactoryConfig config, IPathHandlerContext context)
    {
        _client = new HttpClient(new DebugLoggingHandler(context, new HttpClientHandler()))
        {
            BaseAddress = config.EndpointUri
        };
        if (!string.IsNullOrEmpty(config.ApiKey))
        {
            _client.DefaultRequestHeaders.Add("X-JFrog-Art-Api", config.ApiKey);
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

    public HttpResponseMessage Get(string uri)
    {
        return _client.Get(uri);
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
    
    
}