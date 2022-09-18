using MountAnything.Content;

namespace MountArtifactory;

public class ArtifactoryFileContentReader : StreamContentReader
{
    private readonly ArtifactoryClient _client;
    private readonly string _fileUri;

    public ArtifactoryFileContentReader(ArtifactoryClient client, string fileUri)
    {
        _client = client;
        _fileUri = fileUri;
    }

    protected override Stream CreateContentStream()
    {
        var response = _client.Get(_fileUri);

        //TODO: We really should dispose the response here, but need the StreamContentReader to expose a method for doing so
        return response.Content.ReadAsStream();
    }
}