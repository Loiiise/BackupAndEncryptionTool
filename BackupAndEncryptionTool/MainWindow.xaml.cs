using BackupAndEncryptionTool.Services;
using System.Windows;

namespace BackupAndEncryptionTool
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // todo: make proper services at some point, see #4
            _fileSystemService = new();
            InitializeComponent();
        }


        public void AddSourcePath(object sender, RoutedEventArgs e)
        {
            var newPath = _fileSystemService.PromptUserForFolderPath();
            this.sourcePaths.Items.Add(newPath);
        }

        public void AddDestinationPath(object sender, RoutedEventArgs e)
        {
            var newPath = _fileSystemService.PromptUserForFolderPath();
            this.destinationPaths.Items.Add(newPath);
        }
        public void PerformBackup(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Back me up babyy");            
        }

        private FileSystemService _fileSystemService;
    }
}
