using System.Management.Automation;

namespace MountArtifactory;

public class ArtifactoryDriveParameters
{
    [Parameter(Mandatory = true)] 
    public string ArtifactoryEndpoint { get; set; } = null!;

    [Parameter(Mandatory = false)]
    public string? ApiKey { get; set; }
}