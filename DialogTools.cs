using System.Collections.Generic;
using System.Windows.Forms;


public static class DialogTools
{
    public static string[] OpenFiles(out bool isOk, string filter = "所有文件|*.*")
    {
        List<string> fullNames = new List<string>();
        OpenFileDialog fileDialog = new OpenFileDialog();

        fileDialog.Multiselect = true;
        fileDialog.CheckFileExists = true;
        fileDialog.InitialDirectory = Application.StartupPath;
        fileDialog.Filter = filter;
        if (fileDialog.ShowDialog() == DialogResult.OK)
        {
            foreach (var fileDialogFileName in fileDialog.FileNames)
            {
                fullNames.Add(fileDialogFileName);
            }
            isOk = true;
        }
        else
        {
            isOk = false;
        }
        return fullNames.ToArray();
    }

    public static string SaveFile(out bool isOk, string fileType = "txt")
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = string.Format("{0}（*.{0}）|*.{0}", fileType);
        sfd.FilterIndex = 1;
        sfd.RestoreDirectory = true;
        sfd.FileName = "";
        if (sfd.ShowDialog() == DialogResult.OK)
        {
            isOk = true;
            return sfd.FileName;
        }
        isOk = false;
        return "";
    }

    /// <summary>
    /// 打开文件夹选择器（纯 WinForms，不依赖 WPF）
    /// </summary>
    public static string OpenFolder(out bool isOk)
    {
        using var dialog = new FolderBrowserDialog();
        dialog.InitialDirectory = Application.StartupPath;
        dialog.ShowNewFolderButton = true;

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            isOk = true;
            return dialog.SelectedPath;
        }

        isOk = false;
        return "";
    }

    public static void OpenExplorer(string openPath)
    {
        System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{openPath}\"");
    }
}
