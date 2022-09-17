using MountAnything;

namespace MountArtifactory;

public record File
{
    #region Shared Properties

    public string Uri { get; set; }
    public string Repo { get; set; }
    public string Path { get; set; }
    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public string ModifiedBy { get; set; }
    public DateTimeOffset LastUpdated { get; set; }

    #endregion

    #region Folder Properties

    public FolderChild[]? Children { get; set; }

    #endregion

    #region File Properties

    public string? DownloadUri { get; set; }
    public string? RemoteUrl { get; set; }
    public int? Size { get; set; }
    public string? MimeType { get; set; }
    public Checksum? Checksums { get; set; }
    public Checksum? OriginalChecksums { get; set; }

    #endregion

    public bool IsFile => Size != null;
    public bool IsFolder => !IsFile;
}

public record FolderChild
{
    public string Uri { get; set; }
    public bool Folder { get; set; }
}

public record Checksum
{
    public string Md5 { get; set; }
    public string Sha1 { get; set; }
    public string Sha256 { get; set; }
}