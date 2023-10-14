using BackupAndEncryptionTool.Services;
using BackupAndEncryptionTool.Types;
using BackupAndEncryptionTool.WPF.Extensions;
using BackupAndEncryptionTool.WPF.Services;
using System;
using System.IO;
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
                "LoiiiseBackupAndEncryptionTool",
                $"configuration.{FileExtensions.Configuration}");

            // todo: make proper services at some point, see #4
            _configurationFileService = new ConfigurationFileService();
            _fileSystemService = new FileDialogServiceWPF();

            InitializeComponent();
            SetConfiguration();
        }

        public void AddSourcePath(object sender, RoutedEventArgs e) => TryAddFolderToCollectionAndSave(sourcePaths.Items);
        public void AddDestinationPath(object sender, RoutedEventArgs e) => TryAddFolderToCollectionAndSave(destinationPaths.Items);

        public void PerformBackup(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Back me up babyy");            
        }

        private void TryAddFolderToCollectionAndSave(ItemCollection collection)
        {
            if (_fileSystemService.TryGetFolderPathFromUser(out var newPath, out var exception))
            {               
                collection.Add(newPath);

                var configuration = new Configuration(sourcePaths.ToStringArray(), destinationPaths.ToStringArray());
                _configurationFileService.Save(_configurationPath, configuration);
            }
            else
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void SetConfiguration()
        {
            var configuration = _configurationFileService.Load(_configurationPath);

            foreach (var sourceItem in configuration.SourcePaths)
            {
                sourcePaths.Items.Add(sourceItem);
            }

            foreach (var destinationItem in configuration.DestinationPaths)
            {
                sourcePaths.Items.Add(destinationItem);
            }
        }

        private string _configurationPath;
        private IConfigurationFileService _configurationFileService;
        private IFileDialogServiceWPF _fileSystemService;
    }
}
