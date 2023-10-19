using BackupAndEncryptionTool.Types;
using System.IO.Compression;

namespace BackupAndEncryptionTool.Services;

public class FileExportService : IFileExportService
{
    public void Export(Configuration configuration)
    {
        if (!configuration.SourceDirectoryPaths.Any() || 
            !configuration.DestinationDirectoryPaths.Any())
        {
            // There's nothing to zip or no location to zip to
            // todo: potentially throw here
            return;
        }

        var pathInitialZipArchive = Path.Combine(configuration.DestinationDirectoryPaths.First(), $"{configuration.Name}.{FileExtensions.TemporaryArchive}");

        using (var initialArchiveStream = new FileStream(pathInitialZipArchive, FileMode.Create))
        {
            using (var initialArchive = new ZipArchive(initialArchiveStream, ZipArchiveMode.Update))
            {
                foreach (var sourceDirectoryPath in configuration.SourceDirectoryPaths.Distinct())
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

        foreach (var remainingDestinationPath in configuration.DestinationDirectoryPaths.Skip(1))
        {
            File.Copy(pathInitialZipArchive, remainingDestinationPath);
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
}