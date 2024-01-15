namespace BackupAndEncryptionTool.Types;

public class FileExtensions
{
    public const string Zip = "zip";

    public const string Configuration = _programPrefix + nameof(Configuration);
    public const string TemporaryArchive = _programPrefix + nameof(TemporaryArchive);
    public const string Archive = _programPrefix + nameof(Archive);

    private const string _programPrefix = "BET";
}
