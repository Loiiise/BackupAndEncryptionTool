using BackupAndEncryptionTool.Types;

namespace BackupAndEncryptionTool.Services;

public interface IConfigurationFileService
{
    void Save(string path, Configuration configuration);
    Configuration Load(string path);
}
