using BackupAndEncryptionTool.Types;

namespace BackupAndEncryptionTool.Services;

public interface IFileExportService
{
    void Export(Configuration configuration);
}