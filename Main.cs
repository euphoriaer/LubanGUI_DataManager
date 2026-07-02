using System.Diagnostics;
using System.Text;

namespace ExcelDataExport
{
    public partial class Main : Form
    {
        private RichTextBox _logBox = null!;
        private Panel _logPanel = null!;
        private Form? _floatForm;
        private bool _resizingLog;

        public Main()
        {
            InitializeComponent();
            ApplyLayoutImprovements();
        }

        /// <summary>
        /// 运行时美化：设置页 GroupBox 分组 + 导出页 SplitContainer 日志面板
        /// </summary>
        private void ApplyLayoutImprovements()
        {
            // ====== 设置页美化 ======
            SetupSettingsTab();
            // ====== 导出页：恢复简洁布局 ======
            SetupExportTab();
            // ====== 右侧日志面板：可停靠 / 弹出 ======
            SetupLogPanel();
        }

        private void SetupSettingsTab()
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

            // ---- JSON 导出复选框事件 ----
            airCheckBox1.CheckedChanged += AirCheckBox_JsonData_CheckedChanged;
            airCheckBox5.CheckedChanged += AirCheckBox_JsonCS_CheckedChanged;
        }

        /// <summary>
        /// 导出页：恢复文件列表填满 tab（日志移到右侧面板）
        /// </summary>
        private void SetupExportTab()
        {
            var exportPage = tabPage1;
            if (exportPage == null) return;

            // 把 excelListBox 和 toolbar 放回 tabPage1
            excelListBox.Parent = exportPage;
            excelListBox.Location = new Point(0, 54);
            excelListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            parrotToolStrip1.Parent = exportPage;
            parrotToolStrip1.Location = new Point(4, 4);

            // resize 时让列表自适应
            exportPage.Resize += (_, _) =>
            {
                excelListBox.Size = new Size(exportPage.Width - 4, exportPage.Height - 58);
            };
        }

        /// <summary>
        /// 右侧日志面板：可停靠 / 弹出为独立窗口
        /// </summary>
        private void SetupLogPanel()
        {
            // ---- 右侧日志面板（Dock = Right，最简单可靠）----
            _logPanel = new Panel
            {
                Width = 360,
                Dock = DockStyle.Right,
                BackColor = Color.FromArgb(40, 40, 40),
                Parent = this,
            };
            // airForm1 已经 Dock=Fill，会自动填满 _logPanel 左边的空间

            // ---- 工具栏 ----
            var logToolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = Color.FromArgb(50, 50, 50),
                Parent = _logPanel,
            };

            var btnClear = new Button
            {
                Text = "清空",
                Size = new Size(50, 24),
                Location = new Point(4, 3),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(180, 180, 180),
                BackColor = Color.FromArgb(70, 70, 70),
                Font = new Font("Segoe UI", 8F),
                Parent = logToolbar,
            };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Click += (_, _) => _logBox.Clear();

            var btnFloat = new Button
            {
                Text = "📌 弹出",
                Size = new Size(60, 24),
                Location = new Point(58, 3),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(180, 180, 180),
                BackColor = Color.FromArgb(70, 70, 70),
                Font = new Font("Segoe UI", 8F),
                Parent = logToolbar,
            };
            btnFloat.FlatAppearance.BorderSize = 0;
            btnFloat.Click += (_, _) => FloatLogWindow();

            // ---- 拖拽调整宽度的手柄 ----
            var resizeHandle = new Panel
            {
                Width = 5,
                Dock = DockStyle.Left,
                Cursor = Cursors.SizeWE,
                BackColor = Color.FromArgb(60, 60, 60),
                Parent = _logPanel,
            };
            int startX = 0, startW = 0;
            resizeHandle.MouseDown += (_, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    _resizingLog = true;
                    startX = MousePosition.X;
                    startW = _logPanel.Width;
                }
            };
            resizeHandle.MouseMove += (_, e) =>
            {
                if (!_resizingLog) return;
                int diff = startX - MousePosition.X;
                int newW = startW + diff;
                if (newW > 150 && newW < Width - 200)
                    _logPanel.Width = newW;
            };
            resizeHandle.MouseUp += (_, _) => _resizingLog = false;
            // 全局松开确保不卡 resize 状态
            MouseUp += (_, _) => _resizingLog = false;

            // ---- 日志文本框 ----
            _logBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.FromArgb(200, 200, 200),
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                WordWrap = true,
                Parent = _logPanel,
            };

            AppendLog("就绪。点击「导出全部」开始。");
        }

        /// <summary>
        /// 把日志弹出为独立窗口
        /// </summary>
        private void FloatLogWindow()
        {
            if (_floatForm != null) return;

            // 从停靠面板取出 RichTextBox
            _logBox.Parent = null;
            _logPanel.Visible = false;

            // 创建浮动窗口
            _floatForm = new Form
            {
                Text = "命令输出 — 关闭即恢复停靠",
                Size = new Size(500, 500),
                StartPosition = FormStartPosition.CenterScreen,
                Icon = SystemIcons.Application,
                Owner = this,
            };

            _logBox.Parent = _floatForm;
            _logBox.Dock = DockStyle.Fill;

            _floatForm.FormClosing += (_, e) =>
            {
                _logBox.Parent = _logPanel;
                _logBox.Dock = DockStyle.Fill;
                _logPanel.Visible = true;
                _floatForm = null;
            };

            _floatForm.Show();
        }

        /// <summary>
        /// 追加日志（线程安全）
        /// </summary>
        private void AppendLog(string text, bool isError = false)
        {
            if (_logBox == null || _logBox.IsDisposed) return;

            if (_logBox.InvokeRequired)
            {
                _logBox.Invoke(() => AppendLog(text, isError));
                return;
            }

            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            var prefix = isError ? "ERR" : "   ";
            var color = isError ? Color.FromArgb(255, 120, 100) : Color.FromArgb(180, 200, 180);

            _logBox.SelectionStart = _logBox.TextLength;
            _logBox.SelectionLength = 0;
            _logBox.SelectionColor = Color.FromArgb(100, 100, 100);
            _logBox.AppendText($"[{timestamp}] ");
            _logBox.SelectionColor = color;
            _logBox.AppendText($"{text}");
            _logBox.SelectionColor = Color.FromArgb(200, 200, 200);
            _logBox.AppendText("\n");
            _logBox.ScrollToCaret();
        }

        /// <summary>
        /// 运行命令，实时捕获输出到日志面板
        /// </summary>
        private async Task<int> RunAndLog(string exePath, string arguments)
        {
            AppendLog($"▶ {Path.GetFileName(exePath)} {arguments}");

            using var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8,
                },
            };

            var tcs = new TaskCompletionSource<int>();

            proc.OutputDataReceived += (_, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    AppendLog(e.Data);
            };
            proc.ErrorDataReceived += (_, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    AppendLog(e.Data, isError: true);
            };

            proc.EnableRaisingEvents = true;
            proc.Exited += (_, _) => tcs.TrySetResult(proc.ExitCode);

            try
            {
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
            }
            catch (Exception ex)
            {
                AppendLog($"启动失败: {ex.Message}", isError: true);
                return -1;
            }

            var exitCode = await tcs.Task;

            if (exitCode == 0)
                AppendLog($"✓ 完成 (exit code: {exitCode})");
            else
                AppendLog($"✗ 失败 (exit code: {exitCode})", isError: true);

            return exitCode;
        }

        /// <summary>
        /// 检测 .NET 8 运行时是否已安装
        /// </summary>
        private static bool IsDotNet8Installed()
        {
            // 方式 1：调用 dotnet --list-runtimes
            try
            {
                using var p = Process.Start(new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "--list-runtimes",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                });
                if (p != null)
                {
                    p.WaitForExit(5000);
                    var output = p.StandardOutput.ReadToEnd();
                    if (output.Contains("Microsoft.NETCore.App 8.0"))
                        return true;
                }
            }
            catch { /* dotnet 不在 PATH，继续方式 2 */ }

            // 方式 2：检查常见安装目录
            var paths = new[]
            {
                @"C:\Program Files\dotnet\shared\Microsoft.NETCore.App",
                @"C:\Program Files (x86)\dotnet\shared\Microsoft.NETCore.App",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "dotnet", "shared", "Microsoft.NETCore.App"),
            };

            foreach (var basePath in paths)
            {
                if (!Directory.Exists(basePath)) continue;
                var dirs = Directory.GetDirectories(basePath, "8.0.*");
                if (dirs.Length > 0) return true;
            }

            return false;
        }

        /// <summary>
        /// 确保 .NET 8 运行时可用；如未安装则自动运行安装包
        /// </summary>
        private async Task<bool> EnsureDotNetRuntimeAsync()
        {
            if (IsDotNet8Installed())
            {
                AppendLog(".NET 8 运行时 ✓ 已安装");
                return true;
            }

            AppendLog("未检测到 .NET 8 运行时", isError: true);

            string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
            var installerPath = Path.Combine(baseDir, "plugin", "dotnet-runtime-8.0.28-win-x64.exe");
            if (!File.Exists(installerPath))
            {
                AppendLog($"找不到运行时安装包：{installerPath}", isError: true);
                MessageBox.Show($"未安装 .NET 8 运行时，且找不到安装包：\n{installerPath}",
                    "运行时缺失", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var result = MessageBox.Show(
                "Luban 需要 .NET 8 运行时，当前系统中未检测到。\n\n是否立即安装？",
                "安装 .NET 8 运行时",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return false;

            AppendLog("▶ 正在安装 .NET 8 运行时（静默安装）...");
            try
            {
                using var p = Process.Start(new ProcessStartInfo
                {
                    FileName = installerPath,
                    Arguments = "/install /quiet /norestart",
                    UseShellExecute = true,
                    Verb = "runas", // 提权
                });
                if (p != null)
                {
                    await Task.Run(() => p.WaitForExit());
                    AppendLog(p.ExitCode == 0
                        ? "✓ .NET 8 运行时安装完成"
                        : $"安装程序退出码: {p.ExitCode}", isError: true);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"安装失败: {ex.Message}", isError: true);
            }

            // 再检查一次
            return IsDotNet8Installed();
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
            airCheckBox1.Checked = JsonConfig.ConfigInstance.json_data;
            airCheckBox5.Checked = JsonConfig.ConfigInstance.json_cs;
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
            // 检查必要路径
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

            // 检查 .NET 8 运行时
            if (!await EnsureDotNetRuntimeAsync())
            {
                AppendLog("未安装 .NET 8 运行时，导出取消。", isError: true);
                return;
            }

            _logBox?.Clear();
            AppendLog("======== 开始导出 ========");

            // 构建 Luban 参数
            var args = new StringBuilder();
            args.Append($"--conf \"{JsonConfig.ConfigInstance.LubanConfigPath}\" ");
            args.Append("-t all ");
            args.Append("-s default ");

            if (JsonConfig.ConfigInstance.json_data)
            {
                args.Append("-d json ");
                args.Append($"-x json.outputDataDir=\"{JsonConfig.ConfigInstance.DataPath}/json\" ");
            }
            if (JsonConfig.ConfigInstance.json_cs)
            {
                args.Append("-c cs-simple-json ");
                args.Append($"-x cs-simple-json.outputCodeDir=\"{JsonConfig.ConfigInstance.ScriptsPath}/cs_json\" ");
            }
            if (JsonConfig.ConfigInstance.cs_bin)
            {
                args.Append("-c cs-bin ");
                args.Append($"-x cs-bin.outputCodeDir=\"{JsonConfig.ConfigInstance.ScriptsPath}/cs_bin\" ");
            }
            if (JsonConfig.ConfigInstance.bin_cs)
            {
                args.Append("-d bin ");
                args.Append($"-x bin.outputDataDir=\"{JsonConfig.ConfigInstance.DataPath}/bin\" ");
            }
            if (JsonConfig.ConfigInstance.protobuf_bin)
            {
                args.Append("-d protobuf3-bin ");
                args.Append($"-x protobuf3-bin.outputDataDir=\"{JsonConfig.ConfigInstance.DataPath}/protobuf3_bin\" ");
            }
            if (JsonConfig.ConfigInstance.protobuf_cs)
            {
                args.Append("-c protobuf3 ");
                args.Append("-c cs-protobuf3 ");
                args.Append($"-x protobuf3.outputCodeDir=\"{JsonConfig.ConfigInstance.ScriptsPath}/protobuf3\" ");
                args.Append($"-x cs-protobuf3.outputCodeDir=\"{JsonConfig.ConfigInstance.ScriptsPath}/cs_protobuf3\" ");
            }

            // --- 第一阶段：运行 Luban ---
            int lubanResult = await RunAndLog(lubanExe, args.ToString());
            if (lubanResult != 0)
            {
                AppendLog("Luban 导出异常，请检查上方红色日志。", isError: true);
                return;
            }

            // --- 第二阶段：protoc C# 代码生成 ---
            if (JsonConfig.ConfigInstance.protobuf_cs)
            {
                var protoDir = $"{JsonConfig.ConfigInstance.ScriptsPath}/protobuf3";
                if (!Directory.Exists(protoDir))
                {
                    AppendLog($"proto 目录不存在，跳过 protoc 生成：{protoDir}", isError: true);
                    return;
                }

                string protocExe = JsonConfig.ConfigInstance.ProtoBufPath;
                if (!File.Exists(protocExe))
                {
                    AppendLog($"找不到 protoc.exe：{protocExe}", isError: true);
                    return;
                }

                var protoFiles = Directory.GetFiles(protoDir, "*.proto");
                AppendLog($"\n======== protoc 生成 ({protoFiles.Length} 个 proto 文件) ========");

                foreach (var protoFile in protoFiles)
                {
                    var protoArgs = $"--csharp_out=\"{protoDir}\" --proto_path=\"{protoDir}\" \"{Path.GetFileName(protoFile)}\"";
                    await RunAndLog(protocExe, protoArgs);
                }
            }

            AppendLog("\n======== 导出完成 ========");
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

        private void AirCheckBox_JsonData_CheckedChanged(object sender)
        {
            JsonConfig.ConfigInstance.json_data = airCheckBox1.Checked;
            JsonConfig.ConfigInstance.SaveConfig();
        }

        private void AirCheckBox_JsonCS_CheckedChanged(object sender)
        {
            JsonConfig.ConfigInstance.json_cs = airCheckBox5.Checked;
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
