using MountAnything;

namespace MountArtifactory;

public class FileItem : Item
{
    public FileItem(ItemPath parentPath, File file) : base(parentPath, file)
    {
        IsFolder = file.IsFolder;
        ItemName = new ItemPath(file.Path).Name;
        Children = file.Children;
    }

    public FileItem(ItemPath parentPath, FolderChild folder) : base(parentPath, folder)
    {
        IsFolder = folder.Folder;
        ItemName = folder.Uri.Substring(1);
        IsPartial = true;
    }

    public override string ItemName { get; }
    public override bool IsContainer => IsFolder;
    public bool IsFolder { get; }
    public override bool IsPartial { get; }
    protected override string TypeName => GetType().FullName!;
    public override string? ItemType => IsFolder ? "Directory" : "File";
    public FolderChild[]? Children { get; set; }
}