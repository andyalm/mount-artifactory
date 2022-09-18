using System.Management.Automation;
using MountAnything;

namespace MountArtifactory;

public class FileItem : Item
{
    public FileItem(ItemPath parentPath, ItemPath filePath, File file, Uri repositoryUri) : base(parentPath, file)
    {
        IsFolder = file.IsFolder;
        Path = filePath.FullName;
        ItemName = new ItemPath(file.Path).Name;
        Children = file.Children;
        RepositoryUri = repositoryUri;
    }

    public FileItem(ItemPath parentPath, ItemPath filePath, FolderChild folder, Uri repositoryUri) : base(parentPath, new PSObject())
    {
        IsFolder = folder.Folder;
        Path = filePath.FullName;
        ItemName = folder.Uri.Substring(1);
        IsPartial = true;
        RepositoryUri = repositoryUri;
    }

    public override string ItemName { get; }
    public override bool IsContainer => IsFolder;
    
    [ItemProperty]
    public string Path { get; set; }
    public bool IsFolder { get; }
    public override bool IsPartial { get; }
    protected override string TypeName => GetType().FullName!;
    public override string? ItemType => IsFolder ? "Directory" : "File";
    public FolderChild[]? Children { get; set; }
    
    [ItemProperty]
    public string Url => $"{RepositoryUri}/{Path}";
    private Uri RepositoryUri { get; }
}