using System.Diagnostics;
using System.Text;

namespace ExcelDataExport
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 使用文件夹选择器设置路径，并保存到配置
        /// </summary>
        private void SetPathFromFolderPicker(string propertyName, Action<string> setRelativePath, Func<string> getFullPath, Control textBox)
        {
            string selectedPath = DialogTools.OpenFolder(out var isok);
            if (!isok) return;

            string baseDir = GetBaseDir();
            string relativePath;
            try
            {
                relativePath = Path.GetRelativePath(baseDir, selectedPath);
            }
            catch (ArgumentException)
            {
                // 跨盘符时无法计算相对路径，直接存绝对路径
                relativePath = selectedPath;
            }

            setRelativePath(relativePath);
            JsonConfig.ConfigInstance.SaveConfig();
            textBox.Text = getFullPath();
        }

        /// <summary>
        /// 使用文件选择器设置路径，并保存到配置
        /// </summary>
        private void SetPathFromFilePicker(string propertyName, Action<string> setRelativePath, Func<string> getFullPath, Control textBox)
        {
            string[] files = DialogTools.OpenFiles(out var isok);
            if (!isok || files.Length == 0) return;

            string selectedPath = files[0];
            string baseDir = GetBaseDir();
            string relativePath;
            try
            {
                relativePath = Path.GetRelativePath(baseDir, selectedPath);
            }
            catch (ArgumentException)
            {
                relativePath = selectedPath;
            }

            setRelativePath(relativePath);
            JsonConfig.ConfigInstance.SaveConfig();
            textBox.Text = getFullPath();
            RefreshSetting();
        }

        private static string GetBaseDir()
        {
            return Path.GetDirectoryName(Application.ExecutablePath);
        }

        private void LubanPathSet_Click(object sender, EventArgs e)
        {
            SetPathFromFolderPicker(
                nameof(JsonConfig.ConfigInstance.LubanPathRelative),
                v => JsonConfig.ConfigInstance.LubanPathRelative = v,
                () => JsonConfig.ConfigInstance.LubanPath,
                aloneTextBox1Luban);
        }

        private void aloneButton2_Click(object sender, EventArgs e)
        {
            SetPathFromFolderPicker(
                nameof(JsonConfig.ConfigInstance.DataPathRelative),
                v => JsonConfig.ConfigInstance.DataPathRelative = v,
                () => JsonConfig.ConfigInstance.DataPath,
                aloneTextBox2Data);
        }

        private void aloneButton3_Click(object sender, EventArgs e)
        {
            SetPathFromFolderPicker(
                nameof(JsonConfig.ConfigInstance.ScriptsPathRelative),
                v => JsonConfig.ConfigInstance.ScriptsPathRelative = v,
                () => JsonConfig.ConfigInstance.ScriptsPath,
                aloneTextBox3Script);
        }

        private void aloneButton4_Click(object sender, EventArgs e)
        {
            SetPathFromFolderPicker(
                nameof(JsonConfig.ConfigInstance.ExcelsPathRelative),
                v => JsonConfig.ConfigInstance.ExcelsPathRelative = v,
                () => JsonConfig.ConfigInstance.ExcelsPath,
                aloneTextBox1Excel);
            RefreshSetting();
        }

        private void aloneButton5Luban_Config_Click(object sender, EventArgs e)
        {
            SetPathFromFilePicker(
                nameof(JsonConfig.ConfigInstance.LubanConfigPathRelative),
                v => JsonConfig.ConfigInstance.LubanConfigPathRelative = v,
                () => JsonConfig.ConfigInstance.LubanConfigPath,
                aloneTextBox1LubanConfig);
        }

        private void aloneButton5ProtoBuf_Click(object sender, EventArgs e)
        {
            SetPathFromFilePicker(
                nameof(JsonConfig.ConfigInstance.ProtoBufPathRelative),
                v => JsonConfig.ConfigInstance.ProtoBufPathRelative = v,
                () => JsonConfig.ConfigInstance.ProtoBufPath,
                aloneTextBox1ProtoBufPath);
        }

        /// <summary>
        /// 当文本框失去焦点时，如果内容有变化则保存（支持用户手动编辑路径）
        /// </summary>
        private void textBox_Leave(object sender, EventArgs e)
        {
            if (sender is not Control tb) return;

            string newFullPath = tb.Text.Trim();
            if (string.IsNullOrEmpty(newFullPath)) return;

            string baseDir = GetBaseDir();
            string relativePath;
            try
            {
                relativePath = Path.GetRelativePath(baseDir, newFullPath);
            }
            catch (ArgumentException)
            {
                relativePath = newFullPath;
            }

            // 判断是哪个文本框，更新对应的配置
            bool changed = false;
            if (tb == aloneTextBox1Luban && JsonConfig.ConfigInstance.LubanPathRelative != relativePath)
            {
                JsonConfig.ConfigInstance.LubanPathRelative = relativePath;
                changed = true;
            }
            else if (tb == aloneTextBox2Data && JsonConfig.ConfigInstance.DataPathRelative != relativePath)
            {
                JsonConfig.ConfigInstance.DataPathRelative = relativePath;
                changed = true;
            }
            else if (tb == aloneTextBox3Script && JsonConfig.ConfigInstance.ScriptsPathRelative != relativePath)
            {
                JsonConfig.ConfigInstance.ScriptsPathRelative = relativePath;
                changed = true;
            }
            else if (tb == aloneTextBox1Excel && JsonConfig.ConfigInstance.ExcelsPathRelative != relativePath)
            {
                JsonConfig.ConfigInstance.ExcelsPathRelative = relativePath;
                changed = true;
            }
            else if (tb == aloneTextBox1LubanConfig && JsonConfig.ConfigInstance.LubanConfigPathRelative != relativePath)
            {
                JsonConfig.ConfigInstance.LubanConfigPathRelative = relativePath;
                changed = true;
            }
            else if (tb == aloneTextBox1ProtoBufPath && JsonConfig.ConfigInstance.ProtoBufPathRelative != relativePath)
            {
                JsonConfig.ConfigInstance.ProtoBufPathRelative = relativePath;
                changed = true;
            }

            if (changed)
            {
                JsonConfig.ConfigInstance.SaveConfig();
                RefreshSetting();
            }
        }

        internal void RefreshSetting()
        {
            airCheckBox2_cs_bin.Checked = JsonConfig.ConfigInstance.cs_bin;
            airCheckBox3bin_cs.Checked = JsonConfig.ConfigInstance.bin_cs;
            airCheckBox6Protobuf_bin.Checked = JsonConfig.ConfigInstance.protobuf_bin;
            airCheckBox4Protobuf_cs.Checked = JsonConfig.ConfigInstance.protobuf_cs;

            aloneTextBox1Luban.Text = JsonConfig.ConfigInstance.LubanPath;
            aloneTextBox2Data.Text = JsonConfig.ConfigInstance.DataPath;
            aloneTextBox3Script.Text = JsonConfig.ConfigInstance.ScriptsPath;
            aloneTextBox1Excel.Text = JsonConfig.ConfigInstance.ExcelsPath;
            aloneTextBox1LubanConfig.Text = JsonConfig.ConfigInstance.LubanConfigPath;
            aloneTextBox1ProtoBufPath.Text = JsonConfig.ConfigInstance.ProtoBufPath;

            // 刷新 Excel 文件列表
            if (!string.IsNullOrEmpty(JsonConfig.ConfigInstance.ExcelsPath) &&
                Directory.Exists(JsonConfig.ConfigInstance.ExcelsPath))
            {
                try
                {
                    var filePaths = Directory.GetFiles(JsonConfig.ConfigInstance.ExcelsPath);
                    excelListBox.Items.Clear();
                    foreach (var item in filePaths)
                    {
                        excelListBox.Items.Add(Path.GetFileName(item));
                    }
                }
                catch
                {
                    // 忽略目录不可读等异常
                }
            }
            else
            {
                excelListBox.Items.Clear();
            }

            dungeonRichTextBox1ToolTips.Text = "1.protobuf 不支持中文路径，不支持注释";
        }


        private async void ExportAllExcel_Click(object sender, EventArgs e)
        {
            // 检查必要路径是否已配置
            if (string.IsNullOrEmpty(JsonConfig.ConfigInstance.LubanPathRelative) ||
                string.IsNullOrEmpty(JsonConfig.ConfigInstance.LubanConfigPathRelative))
            {
                MessageBox.Show("请先在「设置」页面配置 Luban 路径和 Luban 配置文件路径", "配置不完整", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string lubanExe = Path.Combine(JsonConfig.ConfigInstance.LubanPath, "Luban.exe");
            if (!File.Exists(lubanExe))
            {
                MessageBox.Show($"找不到 Luban.exe：{lubanExe}", "文件缺失", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 构建 Luban 命令
            var argsBuilder = new StringBuilder();
            argsBuilder.Append($"\"{lubanExe}\" ");
            argsBuilder.Append($"--conf \"{JsonConfig.ConfigInstance.LubanConfigPath}\" ");
            argsBuilder.Append("-t all ");
            argsBuilder.Append("-s default ");

            if (JsonConfig.ConfigInstance.cs_bin)
            {
                argsBuilder.Append("-c cs-bin ");
                argsBuilder.Append($"-x cs-bin.outputCodeDir=\"{JsonConfig.ConfigInstance.ScriptsPath}/cs_bin\" ");
            }

            if (JsonConfig.ConfigInstance.bin_cs)
            {
                argsBuilder.Append("-d bin ");
                argsBuilder.Append($"-x bin.outputDataDir=\"{JsonConfig.ConfigInstance.DataPath}/bin\" ");
            }

            if (JsonConfig.ConfigInstance.protobuf_bin)
            {
                argsBuilder.Append("-d protobuf3-bin ");
                argsBuilder.Append($"-x protobuf3-bin.outputDataDir=\"{JsonConfig.ConfigInstance.DataPath}/protobuf3_bin\" ");
            }

            if (JsonConfig.ConfigInstance.protobuf_cs)
            {
                argsBuilder.Append("-c protobuf3 ");
                argsBuilder.Append("-c cs-protobuf3 ");
                argsBuilder.Append($"-x protobuf3.outputCodeDir=\"{JsonConfig.ConfigInstance.ScriptsPath}/protobuf3\" ");
                argsBuilder.Append($"-x cs-protobuf3.outputCodeDir=\"{JsonConfig.ConfigInstance.ScriptsPath}/cs_protobuf3\" ");
            }

            argsBuilder.AppendLine();

            try
            {
                using Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.CreateNoWindow = false;

                process.Start();
                process.StandardInput.WriteLine(argsBuilder.ToString());
                process.StandardInput.WriteLine("exit"); // 正常退出 cmd
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Luban 导出失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 如果勾选了 protobuf_cs，额外调用 protobuf.exe 生成 C# 代码
            if (JsonConfig.ConfigInstance.protobuf_cs)
            {
                var protoDir = $"{JsonConfig.ConfigInstance.ScriptsPath}/protobuf3";
                if (!Directory.Exists(protoDir))
                {
                    MessageBox.Show($"找不到 proto 输出目录：{protoDir}", "目录缺失", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var protoFiles = Directory.GetFiles(protoDir, "*.proto");
                foreach (var protoFile in protoFiles)
                {
                    try
                    {
                        using Process process2 = new Process();
                        process2.StartInfo.FileName = "cmd.exe";
                        process2.StartInfo.UseShellExecute = false;
                        process2.StartInfo.RedirectStandardInput = true;
                        process2.StartInfo.RedirectStandardOutput = false;
                        process2.StartInfo.CreateNoWindow = false;

                        process2.Start();
                        var protoArgs = $"\"{JsonConfig.ConfigInstance.ProtoBufPath}\" --csharp_out=\"{JsonConfig.ConfigInstance.ScriptsPath}/protobuf3\" --proto_path=\"{protoDir}\" \"{Path.GetFileName(protoFile)}\"";
                        process2.StandardInput.WriteLine(protoArgs);
                        process2.StandardInput.WriteLine("exit");
                        process2.WaitForExit();
                    }
                    catch
                    {
                        // 个别 proto 文件失败不影响其他
                    }
                }
            }

            MessageBox.Show("导出完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void excelListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
