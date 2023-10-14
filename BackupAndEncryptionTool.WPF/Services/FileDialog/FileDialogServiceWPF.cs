using Microsoft.Win32;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace BackupAndEncryptionTool.WPF.Services;

internal class FileDialogServiceWPF : IFileDialogServiceWPF
{
    public string GetFolderPathFromUser()
    {
        var openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            var directory = new FileInfo(openFileDialog.FileName).Directory;

            if (directory is not null)
            {
                return directory.FullName;
            }
            throw new IOException("Failed to get directory from path");
        }

        throw new IOException("Failed to open file dialog");
    }

    public bool TryGetFolderPathFromUser([MaybeNullWhen(false)] out string path, [MaybeNullWhen(true)] out Exception exception)
    {
        try
        {
            path = GetFolderPathFromUser();
            exception = null;
            return true;
        }
        catch (Exception caughtException)
        {
            path = null;
            exception = caughtException;
            return false;
        }
    }
}
