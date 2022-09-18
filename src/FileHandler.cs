using System.Management.Automation.Provider;
using MountAnything;
using MountAnything.Content;

namespace MountArtifactory;

public class FileHandler : PathHandler, IContentReaderHandler, IRemoveItemHandler
{
    private readonly ArtifactoryClient _client;
    private readonly RepositoryName _repositoryName;
    private readonly FilePath _filePath;
    private readonly Uri _repositoryUri;


    public FileHandler(ItemPath path,
        IPathHandlerContext context,
        ArtifactoryClient client,
        ArtifactoryConfig config,
        RepositoryName repositoryName,
        FilePath filePath) : base(path, context)
    {
        _client = client;
        _repositoryName = repositoryName;
        _filePath = filePath;
        _repositoryUri = config.RepositoryUri(repositoryName.Value);
    }

    protected override IItem? GetItemImpl()
    {
        var file = _client.GetFile(_repositoryName, _filePath.Path);

        return file != null ? new FileItem(ParentPath, _filePath.Path, file, _repositoryUri) : null;
    }

    protected override IEnumerable<IItem> GetChildItemsImpl()
    {
        if (GetItem() is FileItem { IsFolder: true } file)
        {
            return file.Children?.Select(c => new FileItem(Path, _filePath.Path.Combine(c.Uri.Substring(1)), c, _repositoryUri)) ?? Enumerable.Empty<IItem>();
        }
        
        return Enumerable.Empty<IItem>();
    }

    // can sometimes be too big
    protected override bool CacheChildren => false;
    
    public void RemoveItem()
    {
        _client.Delete(_repositoryName.Value, _filePath.Path);
    }

    public IContentReader GetContentReader()
    {
        if (GetItem() is FileItem { IsFolder: false })
        {
            return new ArtifactoryFileContentReader(_client, $"{_repositoryName.Value}/{_filePath.Path}");
        }

        return new StringContentReader(String.Empty);
    }
}