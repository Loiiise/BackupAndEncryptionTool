using System.Collections.Generic;
using System.Windows.Controls;

namespace BackupAndEncryptionTool.WPF.Extensions;

internal static class ListBoxExtensions
{
    /// <summary>
    /// This is a hacked version doing a _very_ interesting string split.
    /// I did not want to deal with WPF stuff, should be fixed in the future. See #9
    /// </summary>
    public static string[] ToStringArray(this ListBox listBox)
    {
        var stringList = new List<string>();
        foreach (var item in listBox.Items.SourceCollection)
        {
            var rawData = item.ToString();

            if (rawData is null) continue;

            var rawDataSplit = rawData.Split("System.Windows.Controls.ListBoxItem: ");

            if (rawDataSplit is null ||
                rawDataSplit.Length == 0)
            {
                continue;
            }

            if (rawDataSplit.Length == 1)
            {
                stringList.Add(rawDataSplit[0]);
            }
            else if (rawDataSplit.Length == 2)
            {
                stringList.Add(rawDataSplit[1]);
            }
        }

        return stringList.ToArray();
    }
}
