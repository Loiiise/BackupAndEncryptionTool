using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BackupAndEncryptionTool
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        public void AddSourcePath(object sender, RoutedEventArgs e)
        {
            var currentAmountOfPaths = sourcePaths.Items.Count;
            this.sourcePaths.Items.Add($"SomeSourcePath{currentAmountOfPaths + 1}");
        }

        public void AddDestinationPath(object sender, RoutedEventArgs e)
        {
            var currentAmountOfPaths = sourcePaths.Items.Count;
            this.destinationPaths.Items.Add($"SomeDestinationPath{currentAmountOfPaths + 1}");
        }
        public void PerformBackup(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Back me up babyy");            
        }
    }
}
