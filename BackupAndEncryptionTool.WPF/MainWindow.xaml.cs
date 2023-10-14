using BackupAndEncryptionTool.WPF.Services;
using System.Windows;
using System.Windows.Controls;

namespace BackupAndEncryptionTool
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // todo: make proper services at some point, see #4
            _fileSystemService = new FileDialogServiceWPF();
            InitializeComponent();
        }

        public void AddSourcePath(object sender, RoutedEventArgs e) => TryAddFolderToCollection(sourcePaths.Items);
        public void AddDestinationPath(object sender, RoutedEventArgs e) => TryAddFolderToCollection(destinationPaths.Items);

        public void PerformBackup(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Back me up babyy");            
        }

        private void TryAddFolderToCollection(ItemCollection collection)
        {
            if (_fileSystemService.TryGetFolderPathFromUser(out var newPath, out var exception))
            {
                collection.Add(newPath);
            }
            else
            {
                MessageBox.Show(exception.Message);
            }
        }

        private IFileDialogServiceWPF _fileSystemService;
    }
}
