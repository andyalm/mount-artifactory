using MountAnything;

namespace MountArtifactory;

public class FileHandler : PathHandler
{
    public FileHandler(ItemPath path, IPathHandlerContext context) : base(path, context)
    {
    }

    protected override IItem? GetItemImpl()
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<IItem> GetChildItemsImpl()
    {
        throw new NotImplementedException();
    }
}