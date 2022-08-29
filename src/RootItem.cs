using System.Management.Automation;
using MountAnything;

namespace MountArtifactory;

public class RootItem : IItem
{
    public PSObject ToPipelineObject(Func<ItemPath, string> pathResolver)
    {
        return new PSObject();
    }

    public ItemPath FullPath => ItemPath.Root;
    public bool IsContainer => true;
}