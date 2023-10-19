#nullable disable
namespace BackupAndEncryptionTool.Types;

public class Configuration
{
    // Required for json parsing
    public Configuration() { }

    public Configuration(string[] sourceDirectoryPath, string[] destinationDirectoryPaths)
    {
        SourceDirectoryPaths = sourceDirectoryPath;
        DestinationDirectoryPaths = destinationDirectoryPaths;
    }

    // todo: fix in #10
    public string Name { get; init; } = DateTime.Now.ToString("yyyyMMddHHmm") + "Dummy";
    public string[] SourceDirectoryPaths { get; init; } 
    public string[] DestinationDirectoryPaths { get; init; }
}