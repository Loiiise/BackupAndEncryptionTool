using System;
using System.Diagnostics.CodeAnalysis;

namespace BackupAndEncryptionTool.WPF.Services;

internal interface IFileDialogServiceWPF
{
    string GetFilePathFromUser();
    bool TryGetFilePathFromUser([MaybeNullWhen(false)]out string path, [MaybeNullWhen(true)]out Exception exception);
    string GetFolderPathFromUser();
    bool TryGetFolderPathFromUser([MaybeNullWhen(false)]out string path, [MaybeNullWhen(true)]out Exception exception);
}
