namespace BackupAndEncryptionTool.Types;

public class FileExtensions
{
    public const string Configuration = _programPrefix + nameof(Configuration);
    public const string TemporaryArchive = "zip"; //_programPrefix + nameof(TemporaryArchive); todo: this extension will be used as a temp file from #15 on

    private const string _programPrefix = "BET";
}
