using MountAnything;

namespace MountArtifactory;

public record Repository
{
    public string Key { get; set; } = null!;
    public string? Type { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? PackageType { get; set; }
}

public class RepositoryItem : Item<Repository>
{
    public RepositoryItem(ItemPath parentPath, Repository repository) : base(parentPath, repository)
    {
        ItemName = repository.Key;
    }

    public override string ItemName { get; }
    public override bool IsContainer => true;
}