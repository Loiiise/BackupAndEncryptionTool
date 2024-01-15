namespace BackupAndEncryptionTool.Services;

public interface IFileEncryptionService
{
    void EncryptFile(string sourcePath, string destinationPath);
    void DecryptFile(string sourcePath, string destinationPath);
}