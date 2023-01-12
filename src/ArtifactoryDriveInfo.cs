using System.Management.Automation;

namespace MountArtifactory;

public class ArtifactoryDriveInfo : PSDriveInfo
{
    public ArtifactoryConfig ArtifactoryConfig { get; }

    public ArtifactoryDriveInfo(ArtifactoryConfig artifactoryConfig, PSDriveInfo driveInfo) : base(driveInfo)
    {
        ArtifactoryConfig = artifactoryConfig;
    }
}