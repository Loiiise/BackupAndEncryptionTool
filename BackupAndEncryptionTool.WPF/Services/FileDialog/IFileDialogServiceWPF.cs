using System;
using System.Diagnostics.CodeAnalysis;

namespace BackupAndEncryptionTool.WPF.Services;

internal interface IFileDialogServiceWPF
{
    string GetFolderPathFromUser();
    bool TryGetFolderPathFromUser([MaybeNullWhen(false)]out string path, [MaybeNullWhen(true)]out Exception exception);
}
