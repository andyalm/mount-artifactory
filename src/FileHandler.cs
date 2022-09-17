using MountAnything;

namespace MountArtifactory;

public class FileHandler : PathHandler, IRemoveItemHandler
{
    private readonly ArtifactoryClient _client;
    private readonly RepositoryName _repositoryName;
    private readonly FilePath _filePath;

    public FileHandler(ItemPath path, IPathHandlerContext context, ArtifactoryClient client, RepositoryName repositoryName, FilePath filePath) : base(path, context)
    {
        _client = client;
        _repositoryName = repositoryName;
        _filePath = filePath;
    }

    protected override IItem? GetItemImpl()
    {
        var file = _client.GetFile(_repositoryName, _filePath.Path);

        return file != null ? new FileItem(ParentPath, file) : null;
    }

    protected override IEnumerable<IItem> GetChildItemsImpl()
    {
        if (GetItem() is FileItem { IsFolder: true } file)
        {
            return file.Children?.Select(c => new FileItem(Path, c)) ?? Enumerable.Empty<IItem>();
        }
        
        return Enumerable.Empty<IItem>();
    }

    // can sometimes be too big
    protected override bool CacheChildren => false;
    
    public void RemoveItem()
    {
        _client.Delete(_repositoryName.Value, _filePath.Path);
    }
}