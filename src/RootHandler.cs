using System.Net.Http.Json;
using System.Text.Json;
using MountAnything;

namespace MountArtifactory;

public class RootHandler : PathHandler
{
    private readonly ArtifactoryClient _client;

    public RootHandler(ItemPath path, IPathHandlerContext context, ArtifactoryClient client) : base(path, context)
    {
        _client = client;
    }

    protected override IItem? GetItemImpl()
    {
        return new RootItem();
    }

    protected override IEnumerable<IItem> GetChildItemsImpl()
    {
        return _client.GetRepositories()
            .Select(r => new RepositoryItem(Path, r))
            .ToArray();
    }
}