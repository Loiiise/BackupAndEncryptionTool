using BackupAndEncryptionTool.Types;
using System.IO.Compression;

namespace BackupAndEncryptionTool.Services;

public class FileExportService : IFileExportService
{
    public FileExportService(IFileEncryptionService fileEncryptionService)
    {
        _fileEncryptionService = fileEncryptionService;
    }

    public void Export(Configuration configuration)
    {
        if (!configuration.SourceDirectoryPaths.Any() || 
            !configuration.DestinationDirectoryPaths.Any())
        {
            // There's nothing to zip or no location to zip to
            // todo: potentially throw here
            return;
        }

        // Zip source folders to temporary file
        var pathUnencryptedTemporaryZipArchive = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "LoiiiseBackupAndEncryptionTool",
                $"{configuration.Name}.{FileExtensions.TemporaryArchive}");
        ZipTo(pathUnencryptedTemporaryZipArchive, configuration.SourceDirectoryPaths);

        // Encrypt temporary file to first location
        var destinationFileName = $"{configuration.Name}.{FileExtensions.Archive}";
        var firstDestinationPath = Path.Combine(configuration.DestinationDirectoryPaths.First(), destinationFileName);
        _fileEncryptionService.EncryptFile(pathUnencryptedTemporaryZipArchive, firstDestinationPath);

        // Distribute encrypted file
        foreach (var otherDestinationDirectory in configuration.DestinationDirectoryPaths.Skip(1))
        {
            var otherDestinationPath = Path.Combine(otherDestinationDirectory, destinationFileName);
            File.Copy(firstDestinationPath, otherDestinationPath);
        }        
    }


    private void ZipTo(string destinationPath, string[] sourceDirectoryPaths)
    {
        using (var initialArchiveStream = new FileStream(destinationPath, FileMode.Create))
        {
            using (var initialArchive = new ZipArchive(initialArchiveStream, ZipArchiveMode.Update))
            {
                foreach (var sourceDirectoryPath in sourceDirectoryPaths.Distinct())
                {
                    var parentOrSourceDirectory = Directory.GetParent(sourceDirectoryPath) is DirectoryInfo directoryInfo ?
                        directoryInfo.FullName :
                        "";

                    foreach (var sourceFilePath in GetAllFilePathsInDirectory(sourceDirectoryPath))
                    {
                        var pathInZippedArchive = Path.GetRelativePath(parentOrSourceDirectory, sourceFilePath);
                        initialArchive.CreateEntryFromFile(sourceFilePath, pathInZippedArchive);
                    }
                }
            }
        }
    }

    private IEnumerable<string> GetAllFilePathsInDirectory(string directoryPath)
    {
        foreach (var fileInDirectory in Directory.GetFiles(directoryPath))
        {
            yield return fileInDirectory;
        }
        
        foreach (var fileInSubDirectory in Directory.GetDirectories(directoryPath)
            .SelectMany(GetAllFilePathsInDirectory)
            .Where(filePath => filePath is not null))
        {
            yield return fileInSubDirectory;
        }
    }

    private IFileEncryptionService _fileEncryptionService;
}