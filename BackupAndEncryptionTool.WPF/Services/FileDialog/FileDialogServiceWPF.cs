using Microsoft.Win32;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace BackupAndEncryptionTool.WPF.Services;

internal class FileDialogServiceWPF : IFileDialogServiceWPF
{
    public string GetFilePathFromUser()
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.Title = "ff";
        if (openFileDialog.ShowDialog() == true)
        {
            return openFileDialog.FileName;
        }

        throw new IOException("Failed to open file dialog");
    }

    public bool TryGetFilePathFromUser([MaybeNullWhen(false)] out string path, [MaybeNullWhen(true)] out Exception exception)
    {
        try
        {
            path = GetFilePathFromUser();
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

    public string GetFolderPathFromUser()
    {
        if (TryGetFolderPathFromUser(out var folderPath, out var exception))
        {
            return folderPath;
        }

        throw exception;
    }

    public bool TryGetFolderPathFromUser([MaybeNullWhen(false)] out string path, [MaybeNullWhen(true)] out Exception exception)
    {
        path = null;

        return 
            TryGetFilePathFromUser(out var filePath, out exception) &&
            TryGetFolderPathFromFilePath(filePath, out path, out exception);
    }

    private bool TryGetFolderPathFromFilePath(string filePath, [MaybeNullWhen(false)] out string path, [MaybeNullWhen(true)] out Exception exception)
    {
        var directory = new FileInfo(filePath).Directory;

        if (directory is not null)
        {
            path = directory.FullName;
            exception = null;
            return true;
        }

        throw new IOException("Failed to get directory from path");
    }
}
