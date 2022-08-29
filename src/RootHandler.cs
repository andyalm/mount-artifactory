using System.Net.Http.Json;
using System.Text.Json;
using MountAnything;

namespace MountArtifactory;

public class RootHandler : PathHandler
{
    private readonly HttpClient _client;

    public RootHandler(ItemPath path, IPathHandlerContext context, HttpClient client) : base(path, context)
    {
        _client = client;
    }

    protected override IItem? GetItemImpl()
    {
        return new RootItem();
    }

    protected override IEnumerable<IItem> GetChildItemsImpl()
    {
        var repositories = _client.GetFromJsonAsync<Repository[]>("api/repositories", new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }).GetAwaiter().GetResult();

        return repositories!.Select(r => new RepositoryItem(Path, r)).ToArray();
    }
}