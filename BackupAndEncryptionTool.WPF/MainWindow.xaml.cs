using BackupAndEncryptionTool.Services;
using BackupAndEncryptionTool.Types;
using BackupAndEncryptionTool.WPF.Extensions;
using BackupAndEncryptionTool.WPF.Services;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BackupAndEncryptionTool
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // todo: make less static. See #10
            _configurationPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                _applicationName,
                $"configuration.{FileExtensions.Configuration}");

            // todo: make proper services at some point, see #4
            _configurationFileService = new ConfigurationFileService();
            _fileEncryptionService = new FileEncryptionService();
            _fileExportService = new FileExportService(_fileEncryptionService);
            _fileSystemService = new FileDialogServiceWPF();

            InitializeComponent();
            SetConfiguration();
        }

        public void AddSourcePath(object sender, RoutedEventArgs e) => TryAddFolderToCollectionAndSave(sourcePaths.Items);
        public void RemoveSourcePath(object sender, RoutedEventArgs e) => TryRemoveFolderFromCollectionAndSave(sourcePaths.Items, sourcePaths.SelectedIndex);
        public void AddDestinationPath(object sender, RoutedEventArgs e) => TryAddFolderToCollectionAndSave(destinationPaths.Items);
        public void RemoveDestinationPath(object sender, RoutedEventArgs e) => TryRemoveFolderFromCollectionAndSave(destinationPaths.Items, destinationPaths.SelectedIndex);

        public void PerformBackup(object sender, RoutedEventArgs e) 
        {
            var configuration = GenerateCurrentConfiguration();
            _fileExportService.Export(configuration);

            MessageBox.Show("Back me up babyy");            
        }

        private void DecryptBackedupFile(object sender, RoutedEventArgs e)
        {
            // Get file to decrypt
            if (!_fileSystemService.TryGetFilePathFromUser(out var encryptedFilePath, out var exception))
            {
                // todo: Occurs when dialog is closed without selecting a file, which is fine. Maybe fix at some point.
                MessageBox.Show(exception.Message);
                return;
            }

            // todo: works for now, proper selecting of files in #19
            var decryptFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                _applicationName,
                $"dummyDecryptedFile.{FileExtensions.Zip}");

            _fileEncryptionService.DecryptFile(encryptedFilePath, decryptFilePath);

            MessageBox.Show("Undo that backup babyy");
        }

        private void TryAddFolderToCollectionAndSave(ItemCollection collection)
        {
            if (_fileSystemService.TryGetFolderPathFromUser(out var newPath, out var exception))
            {               
                collection.Add(newPath);

                SaveCurrentConfiguration();
            }
            else
            {
                // todo: Occurs when dialog is closed without selecting a file, which is fine. Maybe fix at some point.
                MessageBox.Show(exception.Message);
            }
        }

        private void TryRemoveFolderFromCollectionAndSave(ItemCollection collection, int indexToRemove)
        {
            collection.RemoveAt(indexToRemove);
            SaveCurrentConfiguration();
        }

        private Configuration GenerateCurrentConfiguration() => new Configuration(sourcePaths.ToStringArray(), destinationPaths.ToStringArray());

        private void SaveCurrentConfiguration()
        {
            var configuration = GenerateCurrentConfiguration();
            _configurationFileService.Save(_configurationPath, configuration);
        }

        private void SetConfiguration()
        {
            var configuration = _configurationFileService.Load(_configurationPath);

            foreach (var sourceItem in configuration.SourceDirectoryPaths)
            {
                sourcePaths.Items.Add(sourceItem);
            }

            foreach (var destinationItem in configuration.DestinationDirectoryPaths)
            {
                destinationPaths.Items.Add(destinationItem);
            }
        }

        private string _configurationPath;
        private IConfigurationFileService _configurationFileService;
        private IFileEncryptionService _fileEncryptionService;
        private IFileExportService _fileExportService;
        private IFileDialogServiceWPF _fileSystemService;

        // todo: move
        private const string _applicationName = "LoiiiseBackupAndEncryptionTool";
    }
}
