using BackupAndEncryptionTool.Types;
using System.Text.Json;

namespace BackupAndEncryptionTool.Services;

public class ConfigurationFileService : IConfigurationFileService
{
    public void Save(string path, Configuration configuration)
    {
        var serializedConfiguration = JsonSerializer.Serialize(configuration);

        if (!File.Exists(path) &&
            Path.GetDirectoryName(path) is string directory)
        {
            Directory.CreateDirectory(directory);            
        }

        File.WriteAllText(path, serializedConfiguration);
    }

    public Configuration Load(string path)
    {
        if (!File.Exists(path))
        {
            // This is not very sense making. Refactor in #10
            var newConfiguration = new Configuration(Array.Empty<string>(), Array.Empty<string>());
            Save(path, newConfiguration);
            return newConfiguration;
        }

        var serializedConfiguration = File.ReadAllText(path);
        if (JsonSerializer.Deserialize<Configuration>(serializedConfiguration) is Configuration configuration)
        {
            return configuration;
        }

        throw new ArgumentException($"Could not load configuration at: {path}");
    }
}