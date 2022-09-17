using MountAnything;

namespace MountArtifactory;

public class RepositoryHandler : PathHandler
{
    private readonly ArtifactoryClient _client;

    public RepositoryHandler(ItemPath path, IPathHandlerContext context, ArtifactoryClient client) : base(path, context)
    {
        _client = client;
    }

    protected override IItem? GetItemImpl()
    {
        var repository = _client.GetRepository(ItemName);

        return repository != null ? new RepositoryItem(ParentPath, repository) : null;
    }

    protected override IEnumerable<IItem> GetChildItemsImpl()
    {
        var file = _client.GetFile(ItemName);
        if (file != null)
        {
            return file.Children?.Select(c => new FileItem(Path, c)) ?? Enumerable.Empty<IItem>();
        }
        
        return Enumerable.Empty<IItem>();
    }
}