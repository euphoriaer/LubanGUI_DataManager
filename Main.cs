using System.Diagnostics;
using System.Text;

namespace ExcelDataExport
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            ApplyLayoutImprovements();
        }

        /// <summary>
        /// 运行时布局美化：用 GroupBox 分组，让界面更整洁
        /// </summary>
        private void ApplyLayoutImprovements()
        {
            var tab = 设置;
            if (tab == null) return;

            // ---- GroupBox 1：路径设置 ----
            var groupPaths = new GroupBox
            {
                Text = "  路径设置  ",
                Location = new Point(3, 3),
                Size = new Size(892, 445),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(78, 87, 100),
                Parent = tab
            };

            // 将路径相关的 6 个 TextBox + 6 个 Button 移入 GroupBox
            var pathRows = new[]
            {
                (TB: aloneTextBox1Excel,      BTN: (Control)aloneButton4),
                (TB: aloneTextBox1Luban,      BTN: (Control)aloneButton1),
                (TB: aloneTextBox2Data,       BTN: (Control)aloneButton2),
                (TB: aloneTextBox3Script,     BTN: (Control)aloneButton3),
                (TB: aloneTextBox1LubanConfig, BTN: (Control)aloneButton5Luban_Config),
                (TB: aloneTextBox1ProtoBufPath, BTN: (Control)aloneButton5ProtoBuf),
            };

            foreach (var (TB, BTN) in pathRows)
            {
                // 先计算相对于 GroupBox 的坐标，再更换父容器
                var tbLoc = new Point(TB.Left - groupPaths.Left, TB.Top - groupPaths.Top);
                var btnLoc = new Point(BTN.Left - groupPaths.Left, BTN.Top - groupPaths.Top);

                TB.Parent = groupPaths;
                TB.Location = tbLoc;

                BTN.Parent = groupPaths;
                BTN.Location = btnLoc;
            }

            // ---- GroupBox 2：导出格式 ----
            var groupFormats = new GroupBox
            {
                Text = "  导出格式  ",
                Location = new Point(3, 452),
                Size = new Size(892, 170),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(78, 87, 100),
                Parent = tab
            };

            // 将 2 个 Label + 6 个 CheckBox 移入 GroupBox
            var formatControls = new Control[]
            {
                dungeonLabel1, dungeonLabel2,
                airCheckBox1, airCheckBox2_cs_bin, airCheckBox6Protobuf_bin,
                airCheckBox5, airCheckBox3bin_cs, airCheckBox4Protobuf_cs,
            };

            foreach (var ctrl in formatControls)
            {
                // 先计算相对于 GroupBox 的坐标，再更换父容器
                var loc = new Point(ctrl.Left - groupFormats.Left, ctrl.Top - groupFormats.Top);
                ctrl.Parent = groupFormats;
                ctrl.Location = loc;
            }

            // 调整窗体标题等美化
            Text = "Luban 数据导出管理器";
            airForm1.Text = "Luban 数据导出管理器";

            // ---- 美化 Excel 文件列表 ----
            // 失焦后仍然保留选中高亮
            excelListBox.HideSelection = false;

            // ---- 右键菜单 ----
            var ctxMenu = new ContextMenuStrip();

            var miOpen = ctxMenu.Items.Add("📂 打开");
            miOpen.Click += (_, _) => OpenSelectedExcel();

            ctxMenu.Items.Add(new ToolStripSeparator());

            var miRename = ctxMenu.Items.Add("✏️ 重命名");
            miRename.Click += (_, _) => RenameSelectedExcel();

            var miDelete = ctxMenu.Items.Add("🗑️ 删除");
            miDelete.Click += (_, _) => DeleteSelectedExcel();

            ctxMenu.Items.Add(new ToolStripSeparator());

            var miFolder = ctxMenu.Items.Add("📁 打开所在文件夹");
            miFolder.Click += (_, _) => OpenSelectedFolder();

            excelListBox.ContextMenuStrip = ctxMenu;
            excelListBox.MouseDown += (_, args) =>
            {
                if (args.Button == MouseButtons.Right)
                {
                    var hit = excelListBox.HitTest(args.Location);
                    if (hit.Item != null)
                        hit.Item.Selected = true;
                }
            };
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

        // ---------------------------------------------------------------
        // 获取当前选中文件的信息，未选中则返回 null
        // ---------------------------------------------------------------
        private (string FullPath, string FileName)? GetSelectedExcel()
        {
            if (excelListBox.SelectedItems.Count == 0) return null;

            var name = excelListBox.SelectedItems[0].Text;
            if (string.IsNullOrEmpty(name)) return null;

            var fullPath = Path.Combine(JsonConfig.ConfigInstance.ExcelsPath, name);
            if (!File.Exists(fullPath)) return null;

            return (fullPath, name);
        }

        // ---------------------------------------------------------------
        // 右键 / 双击 → 打开
        // ---------------------------------------------------------------
        private void OpenSelectedExcel()
        {
            var sel = GetSelectedExcel();
            if (sel == null) return;
            new ExcelViewer(sel.Value.FullPath, sel.Value.FileName).Show();
        }

        private void excelListBox_DoubleClick(object sender, EventArgs e)
        {
            OpenSelectedExcel();
        }

        // ---------------------------------------------------------------
        // 右键 → 重命名
        // ---------------------------------------------------------------
        private void RenameSelectedExcel()
        {
            var sel = GetSelectedExcel();
            if (sel == null) return;

            var oldName = sel.Value.FileName;
            var oldPath = sel.Value.FullPath;

            // 弹出输入框
            var inputForm = new Form
            {
                Text = "重命名文件",
                Size = new Size(420, 160),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false,
            };

            var lbl = new Label
            {
                Text = "请输入新文件名（保留 .xlsx 后缀）：",
                Location = new Point(12, 16),
                AutoSize = true,
                Parent = inputForm,
            };

            var txt = new TextBox
            {
                Text = oldName,
                Location = new Point(12, 42),
                Size = new Size(380, 26),
                Parent = inputForm,
            };
            txt.Select(0, Path.GetFileNameWithoutExtension(oldName).Length);

            var btnOk = new Button
            {
                Text = "确定",
                Size = new Size(80, 30),
                Location = new Point(220, 80),
                DialogResult = DialogResult.OK,
                Parent = inputForm,
            };

            var btnCancel = new Button
            {
                Text = "取消",
                Size = new Size(80, 30),
                Location = new Point(310, 80),
                DialogResult = DialogResult.Cancel,
                Parent = inputForm,
            };

            inputForm.AcceptButton = btnOk;
            inputForm.CancelButton = btnCancel;

            if (inputForm.ShowDialog(this) != DialogResult.OK) return;

            var newName = txt.Text.Trim();
            if (string.IsNullOrEmpty(newName) || newName == oldName) return;
            if (!newName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                newName += ".xlsx";

            var newPath = Path.Combine(JsonConfig.ConfigInstance.ExcelsPath, newName);

            if (File.Exists(newPath))
            {
                MessageBox.Show($"文件 \"{newName}\" 已存在，不能覆盖。", "重命名失败",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                File.Move(oldPath, newPath);
                RefreshSetting();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"重命名失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------------------------------------------------------------
        // 右键 → 删除
        // ---------------------------------------------------------------
        private void DeleteSelectedExcel()
        {
            var sel = GetSelectedExcel();
            if (sel == null) return;

            var result = MessageBox.Show(
                $"确定要删除文件 \"{sel.Value.FileName}\" 吗？\n\n此操作不可撤销。",
                "确认删除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                File.Delete(sel.Value.FullPath);
                RefreshSetting();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------------------------------------------------------------
        // 右键 → 打开所在文件夹
        // ---------------------------------------------------------------
        private void OpenSelectedFolder()
        {
            var sel = GetSelectedExcel();
            if (sel == null) return;

            Process.Start("explorer.exe", $"/select,\"{sel.Value.FullPath}\"");
        }

        private void excelListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
