using System;
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

 
        private async void  ExportAllExcel_Click(object sender, EventArgs e)
        {


            string lubanFolder = JsonConfig.ConfigInstance.LubanPath;
            string lubanExe = Path.Combine(JsonConfig.ConfigInstance.LubanPath, "Luban.exe");
            string lubanArguments = $"{lubanExe} ";
            string workDir = JsonConfig.ConfigInstance.ExcelsPath;

            lubanArguments += $"--conf {JsonConfig.ConfigInstance.LubanConfigPath} ";
            lubanArguments += "-t all ";
            lubanArguments += "-s default ";



            if (JsonConfig.ConfigInstance.cs_bin)
            {
                lubanArguments += $"-c cs-bin ";
                lubanArguments += $"-x cs-bin.outputCodeDir={JsonConfig.ConfigInstance.ScriptsPath}/cs_bin ";
            }

            if (JsonConfig.ConfigInstance.bin_cs)
            {
                lubanArguments += $"-d bin ";
                lubanArguments += $"-x bin.outputDataDir={JsonConfig.ConfigInstance.DataPath}/bin \n";
            }

            try
            {
                // 创建 Process 对象
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                //process.StartInfo.Arguments = lubanArguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.CreateNoWindow = false; // 隐藏 CMD 窗口

                // 启动进程
                process.Start();

                // 向 CMD 输入命令
                //process.StandardInput.WriteLine("echo Hello, World!");
                //process.StandardInput.WriteLine("exit"); // 退出 CMD
                process.StandardInput.WriteLine(lubanArguments);
                // 获取输出结果
                //string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine("CMD 输出结果：");
                //Console.WriteLine(output);

                // 等待进程结束
                //process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
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
