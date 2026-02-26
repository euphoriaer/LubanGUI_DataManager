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

        private void aloneButton5ProtoBuf_Click(object sender, EventArgs e)
        {
            string path = DialogTools.OpenFiles(out var isok).First();
            if (!isok)
            {
                return;
            }
            string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
            string relativePath = Path.GetRelativePath(baseDir, path);
            JsonConfig.ConfigInstance.ProtoBufPathRelative = relativePath;
            JsonConfig.ConfigInstance.SaveConfig();
            aloneTextBox1ProtoBufPath.Text = JsonConfig.ConfigInstance.ProtoBufPath;
            RefreshSetting();
        }

        internal void RefreshSetting()
        {
            airCheckBox2_cs_bin.Checked = JsonConfig.ConfigInstance.cs_bin;
            airCheckBox3bin_cs.Checked = JsonConfig.ConfigInstance.bin_cs;
            airCheckBox6Protobuf_bin.Checked =JsonConfig.ConfigInstance.protobuf_bin;
            airCheckBox4Protobuf_cs.Checked=JsonConfig.ConfigInstance.protobuf_cs;

            aloneTextBox1Luban.Text = JsonConfig.ConfigInstance.LubanPath;
            aloneTextBox2Data.Text = JsonConfig.ConfigInstance.DataPath;
            aloneTextBox3Script.Text = JsonConfig.ConfigInstance.ScriptsPath;
            aloneTextBox1Excel.Text = JsonConfig.ConfigInstance.ExcelsPath;
            aloneTextBox1LubanConfig.Text = JsonConfig.ConfigInstance.LubanConfigPath;
            aloneTextBox1ProtoBufPath.Text = JsonConfig.ConfigInstance.ProtoBufPath;
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


        private async void ExportAllExcel_Click(object sender, EventArgs e)
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
                lubanArguments += $"-x bin.outputDataDir={JsonConfig.ConfigInstance.DataPath}/bin ";
            }

            if (JsonConfig.ConfigInstance.protobuf_bin)
            {
                lubanArguments += $"-d protobuf3-bin ";
                lubanArguments += $"-x protobuf3-bin.outputDataDir={JsonConfig.ConfigInstance.DataPath}/protobuf3_bin ";
            }

            if (JsonConfig.ConfigInstance.protobuf_cs)
            {
                lubanArguments += $"-c protobuf3 ";
                lubanArguments += $"-c cs-protobuf3 ";
                lubanArguments += $"-x protobuf3.outputCodeDir={JsonConfig.ConfigInstance.ScriptsPath}/protobuf3 ";
                lubanArguments += $"-x cs-protobuf3.outputCodeDir={JsonConfig.ConfigInstance.ScriptsPath}/cs_protobuf3 ";
            }

            lubanArguments += "\n";
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            //process.StartInfo.Arguments = lubanArguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.CreateNoWindow = false; // 隐藏 CMD 窗口
            try
            {
                // 创建 Process 对象

                // 启动进程
                process.Start();

                // 向 CMD 输入命令
                //process.StandardInput.WriteLine("echo Hello, World!");
                //process.StandardInput.WriteLine("exit"); // 退出 CMD
                process.StandardInput.WriteLine(lubanArguments);
                // 获取输出结果
                //string output = process.StandardOutput.ReadToEnd();
                //Console.WriteLine(output);

                // 等待进程结束
                //process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
            }
            process.WaitForExit();
            if (JsonConfig.ConfigInstance.protobuf_cs)
            {
                //调用proto 生成代码
                var path = $"{JsonConfig.ConfigInstance.ScriptsPath}/protobuf3";
                var protoFiles = Directory.GetFiles(path);
                foreach (var item in protoFiles)
                {
                    var thisProtoPath = item;


                    Process process2 = new Process();
                    process2.StartInfo.FileName = "cmd.exe";
                    //process.StartInfo.Arguments = lubanArguments;
                    process2.StartInfo.UseShellExecute = false;
                    process2.StartInfo.RedirectStandardInput = true;
                    process2.StartInfo.RedirectStandardOutput = false;
                    process2.StartInfo.CreateNoWindow = false; // 隐藏 CMD 窗口
                    try
                    {
                        // 创建 Process 对象

                        // 启动进程
                        process2.Start();
                        var protoArguments = $"{JsonConfig.ConfigInstance.ProtoBufPath} --csharp_out={JsonConfig.ConfigInstance.ScriptsPath}/protobuf3 --proto_path={path} {Path.GetFileName(item)}\n";
                        process2.StandardInput.WriteLine(protoArguments);
                    }
                    catch
                    {
                    }

                }
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

        private void airCheckBox6_CheckedChanged(object sender)
        {
            JsonConfig.ConfigInstance.protobuf_bin = airCheckBox6Protobuf_bin.Checked;
            JsonConfig.ConfigInstance.SaveConfig();
        }

        private void airCheckBox4_CheckedChanged(object sender)
        {
            JsonConfig.ConfigInstance.protobuf_cs = airCheckBox4Protobuf_cs.Checked;
            JsonConfig.ConfigInstance.SaveConfig();
        }

   

      
    }
}
