using System.Diagnostics;

namespace ExcelDataExport
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            //读取配置



        }

        private void LubanPathSet_Click(object sender, EventArgs e)
        {
            string lubanPath = DialogTools.OpenFolder(out var isok);
            if (!isok)
            {
                return;
            }
            string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
            string relativePath = Path.GetRelativePath(baseDir, lubanPath);
            JsonConfig.ConfigInstance.LubanPathRelative = relativePath;
            JsonConfig.ConfigInstance.SaveConfig();
            aloneTextBox1Luban.Text = JsonConfig.ConfigInstance.LubanPath;
        }

        private void aloneButton2_Click(object sender, EventArgs e)
        {
            string path = DialogTools.OpenFolder(out var isok);
            if (!isok)
            {
                return;
            }
            string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
            string relativePath = Path.GetRelativePath(baseDir, path);
            JsonConfig.ConfigInstance.DataPathRelative = relativePath;
            JsonConfig.ConfigInstance.SaveConfig();
            aloneTextBox2Data.Text = JsonConfig.ConfigInstance.DataPath;
        }

        private void aloneButton3_Click(object sender, EventArgs e)
        {
            string path = DialogTools.OpenFolder(out var isok);
            if (!isok)
            {
                return;
            }
            string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
            string relativePath = Path.GetRelativePath(baseDir, path);
            JsonConfig.ConfigInstance.ScriptsPathRelative = relativePath;
            JsonConfig.ConfigInstance.SaveConfig();
            aloneTextBox3Script.Text = JsonConfig.ConfigInstance.ScriptsPath;
        }

        private void aloneButton4_Click(object sender, EventArgs e)
        {
            string path = DialogTools.OpenFolder(out var isok);
            if (!isok)
            {
                return;
            }
            string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
            string relativePath = Path.GetRelativePath(baseDir, path);
            JsonConfig.ConfigInstance.ExcelsPathRelative = relativePath;
            JsonConfig.ConfigInstance.SaveConfig();
            aloneTextBox1Excel.Text = JsonConfig.ConfigInstance.ExcelsPath;
            RefreshSetting();
        }
        private void aloneButton5Luban_Config_Click(object sender, EventArgs e)
        {
            string path = DialogTools.OpenFiles(out var isok).First();
            if (!isok)
            {
                return;
            }
            string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
            string relativePath = Path.GetRelativePath(baseDir, path);
            JsonConfig.ConfigInstance.LubanConfigPathRelative = relativePath;
            JsonConfig.ConfigInstance.SaveConfig();
            aloneTextBox1LubanConfig.Text = JsonConfig.ConfigInstance.LubanConfigPath;
            RefreshSetting();
        }

        internal void RefreshSetting()
        {
            airCheckBox2_cs_bin.Checked = JsonConfig.ConfigInstance.cs_bin;
            airCheckBox3bin_cs.Checked = JsonConfig.ConfigInstance.bin_cs;
            aloneTextBox1Luban.Text = JsonConfig.ConfigInstance.LubanPath;
            aloneTextBox2Data.Text = JsonConfig.ConfigInstance.DataPath;
            aloneTextBox3Script.Text = JsonConfig.ConfigInstance.ScriptsPath;
            aloneTextBox1Excel.Text = JsonConfig.ConfigInstance.ExcelsPath;
            aloneTextBox1LubanConfig.Text = JsonConfig.ConfigInstance.LubanConfigPath;
            //excel 文件夹存在
            if (!string.IsNullOrEmpty(JsonConfig.ConfigInstance.ExcelsPath))
            {
                var filePaths = Directory.GetFiles(JsonConfig.ConfigInstance.ExcelsPath);
                excelListBox.Items.Clear();
                foreach (var item in filePaths)
                {
                    excelListBox.Items.Add(Path.GetFileName(item));
                }
            }
        }


        private void ExportAllExcel_Click(object sender, EventArgs e)
        {
            string lubanFolder = JsonConfig.ConfigInstance.LubanPath;
            string lubanExe = Path.Combine(JsonConfig.ConfigInstance.LubanPath, "Luban.exe");
            string lubanArguments = "";
            string workDir = JsonConfig.ConfigInstance.ExcelsPath;

            lubanArguments += $"--conf {JsonConfig.ConfigInstance.LubanConfigPath} \n";
            lubanArguments += "-t all \n";
            lubanArguments += "-s default \n";



            if (JsonConfig.ConfigInstance.cs_bin)
            {
                lubanArguments += $"-c cs-bin \n";
                lubanArguments += $"-x cs-bin.outputCodeDir={JsonConfig.ConfigInstance.ScriptsPath}/cs_bin \n";
            }

            if (JsonConfig.ConfigInstance.bin_cs)
            {
                lubanArguments += $"-d bin \n";
                lubanArguments += $"-x bin.outputDataDir={JsonConfig.ConfigInstance.DataPath}/bin \n";
            }


            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = lubanExe,
                    // 根据实际需求修改参数，例如导出所有
                    Arguments = lubanArguments,

                    // 【关键1】设置执行目录，Luban 会在这里寻找 luban.json
                    WorkingDirectory = lubanFolder,

                    // 【关键2】必须设为 true 才能弹出独立的 CMD 黑窗口
                    UseShellExecute = true,

                    // 【关键3】设为 false 以显示窗口 (默认为 false，但显式写出更清晰)
                    CreateNoWindow = false,

                    // 窗口样式 (可选): Normal, Hidden, Maximized, Minimized
                    WindowStyle = ProcessWindowStyle.Normal
                };

                // 启动进程
                // 此时会弹出一个黑色的 CMD 窗口，Luban 的日志会实时打印在里面
                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();

                    // 等待 Luban 执行完毕，窗口会自动关闭（如果 Luban 执行完退出的话）
                    // 如果不想阻塞 UI 线程，可以注释掉下面这行，但通常导表需要等待完成
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        MessageBox.Show("Luban 导表成功！请查看日志。");
                    }
                    else
                    {
                        MessageBox.Show($"Luban 导表失败，退出码: {process.ExitCode}\n请查看错误信息。");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动 Luban 失败:\n{ex.Message}");
            }

        }

        private void airCheckBox2_CheckedChanged(object sender)
        {
            JsonConfig.ConfigInstance.cs_bin = airCheckBox2_cs_bin.Checked;
            JsonConfig.ConfigInstance.SaveConfig();
        }

        private void airCheckBox3bin_cs_CheckedChanged(object sender)
        {
            JsonConfig.ConfigInstance.bin_cs = airCheckBox3bin_cs.Checked;
            JsonConfig.ConfigInstance.SaveConfig();
        }

     
    }
}
