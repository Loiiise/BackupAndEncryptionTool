#nullable disable
namespace BackupAndEncryptionTool.Types;

public class Configuration
{
    // Required for json parsing
    public Configuration() { }

    public Configuration(string[] sourcePath, string[] destinationPaths)
    {
        SourcePaths = sourcePath;
        DestinationPaths = destinationPaths;
    }

    // todo: fix in #10
    public string Name { get; init; } = DateTime.Now.ToString("yyyy-MM-dd-HH:mm") + "Dummy";
    public string[] SourcePaths { get; init; } 
    public string[] DestinationPaths { get; init; }
}