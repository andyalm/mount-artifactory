using System.Text;
using MountAnything;
using MountAnything.Content;

namespace MountArtifactory;

public class FileHandler : PathHandler,
    IContentReaderHandler,
    IContentWriterHandler,
    INewItemHandler,
    IRemoveItemHandler
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

    public IStreamContentReader GetContentReader()
    {
        if (GetItem() is FileItem { IsFolder: false })
        {
            var response = _client.Get($"{_repositoryName.Value}/{_filePath.Path}");

            return new HttpResponseContentReader(response);
        }

        return new EmptyContentReader();
    }

    public IStreamContentWriter GetContentWriter()
    {
        if (GetItem() is FileItem { IsFolder: false })
        {
            return new StreamContentWriter(stream =>
                _client.Put($"{_repositoryName.Value}/{_filePath.Path}", stream));
        }

        throw new InvalidOperationException("Folder content can not be updated");
    }

    public void NewItem(string? itemTypeName, object? newItemValue)
    {
        if (string.IsNullOrEmpty(itemTypeName))
        {
            itemTypeName = "File";
        }
        if (itemTypeName != "File")
        {
            throw new InvalidOperationException($"Creating item type '{itemTypeName}' is not currently supported. Only File creation is currently supported");
        }

        var contentStream = newItemValue switch
        {
            null => new MemoryStream(),
            string stringValue => new MemoryStream(Encoding.UTF8.GetBytes(stringValue)),
            _ => throw new InvalidOperationException(
                "Currently only string content is supported when creating new items")
        };
        
        _client.Put($"{_repositoryName.Value}/{_filePath.Path}", contentStream);
    }
}