using System.Security.Cryptography;

namespace BackupAndEncryptionTool.Services;

public class FileEncryptionService : IFileEncryptionService
{
    public void EncryptFile(string sourcePath, string destinationPath)
    {
        var aes = Aes.Create();

        // todo: actuually configure        
        // todo: configure block size
        aes.Key = _dummyKey;
        aes.IV = _dummyIV;

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        int amountOfBytesRead;
        int offset = 0; // todo: currently unused

        int blockSizeBytes = aes.BlockSize / 8;
        byte[] data = new byte[blockSizeBytes];
        int bytesRead = 0;

        using var sourceFileStream = new FileStream(sourcePath, FileMode.Open);
        using var destinationFileStream = new FileStream(destinationPath, FileMode.Create);
        using var destinationCryptoStream = new CryptoStream(destinationFileStream, encryptor, CryptoStreamMode.Write);

        do
        {
            amountOfBytesRead = sourceFileStream.Read(data, 0, blockSizeBytes);
            offset += amountOfBytesRead;
            destinationCryptoStream.Write(data, 0, amountOfBytesRead);
            bytesRead += blockSizeBytes;
        } while (amountOfBytesRead > 0);

        destinationCryptoStream.FlushFinalBlock();
    }

    public void DecryptFile(string sourcePath, string destinationPath)
    {
        if (!File.Exists(destinationPath) &&
            Path.GetDirectoryName(destinationPath) is string directory)
        {
            Directory.CreateDirectory(directory);
        }

        var aes = Aes.Create();

        // todo: actuually configure        
        // todo: configure block size
        aes.Key = _dummyKey;
        aes.IV = _dummyIV;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        int amountOfBytesRead;
        int offset = 0; // todo: currently unused

        int blockSizeBytes = aes.BlockSize / 8;
        byte[] data = new byte[blockSizeBytes];

        using var sourceFileStream = new FileStream(sourcePath, FileMode.Open);
        using var destinationFileStream = new FileStream(destinationPath, FileMode.Create);
        using var destinationCryptoStream = new CryptoStream(destinationFileStream, decryptor, CryptoStreamMode.Write);

        do
        {
            amountOfBytesRead = sourceFileStream.Read(data, 0, blockSizeBytes);
            offset += amountOfBytesRead;
            destinationCryptoStream.Write(data, 0, amountOfBytesRead);
        } while (amountOfBytesRead > 0);

        destinationCryptoStream.FlushFinalBlock();
    }

    private readonly byte[] _dummyKey = new byte[32];
    private readonly byte[] _dummyIV = new byte[16];
}