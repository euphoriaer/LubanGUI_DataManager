using System.Diagnostics;
using System.Text;

namespace ExcelDataExport
{
    public partial class Main : Form
    {
        private RichTextBox _logBox = null!;
        private Form _logForm = null!;
        private CheckBox _chkUseSubfolders = null!;

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
            // ====== 日志独立弹窗 ======
            SetupLogWindow();
        }

        private void SetupSettingsTab()
        {
            var tab = 设置;
            if (tab == null) return;

            const int gx = 5, gy = 5;

            // ---- GroupBox 1：路径设置 ----
            var groupPaths = new GroupBox
            {
                Text = "  路径设置  ",
                Location = new Point(gx, gy),
                Size = new Size(tab.Width - gx * 2, 450),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(78, 87, 100),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Parent = tab
            };

            var pathRows = new (Control TB, Control BrowseBtn, string FolderGetter)[]
            {
                (aloneTextBox1Excel,      aloneButton4,              nameof(JsonConfig.ConfigInstance.ExcelsPath)),
                (aloneTextBox1Luban,      aloneButton1,              nameof(JsonConfig.ConfigInstance.LubanPath)),
                (aloneTextBox2Data,       aloneButton2,              nameof(JsonConfig.ConfigInstance.DataPath)),
                (aloneTextBox3Script,     aloneButton3,              nameof(JsonConfig.ConfigInstance.ScriptsPath)),
                (aloneTextBox1LubanConfig,aloneButton5Luban_Config,  nameof(JsonConfig.ConfigInstance.LubanConfigPath)),
                (aloneTextBox1ProtoBufPath,aloneButton5ProtoBuf,     nameof(JsonConfig.ConfigInstance.ProtoBufPath)),
            };

            // 按钮列宽度（锚点在右侧，距右边缘固定）
            const int btnColWidth = 175; // BrowseBtn + gap + OpenBtn + margin

            foreach (var (TB, BrowseBtn, folderGetter) in pathRows)
            {
                var tbPos = new Point(TB.Left - groupPaths.Left, TB.Top - groupPaths.Top);
                var btnPos = new Point(BrowseBtn.Left - groupPaths.Left, BrowseBtn.Top - groupPaths.Top);

                TB.Parent = groupPaths;
                TB.Location = tbPos;
                TB.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                TB.Width = Math.Max(200, groupPaths.Width - tbPos.X - btnColWidth);

                BrowseBtn.Parent = groupPaths;
                BrowseBtn.Location = btnPos;
                BrowseBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;

                // 📁 放在浏览按钮右边
                var openBtn = new Button
                {
                    Text = "📁",
                    Size = new Size(32, BrowseBtn.Height),
                    Location = new Point(btnPos.X + BrowseBtn.Width + 4, btnPos.Y),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(124, 133, 142),
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand,
                    Parent = groupPaths,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                };
                openBtn.FlatAppearance.BorderSize = 0;

                openBtn.Click += (_, _) =>
                {
                    var path = folderGetter switch
                    {
                        nameof(JsonConfig.ConfigInstance.ExcelsPath) => JsonConfig.ConfigInstance.ExcelsPath,
                        nameof(JsonConfig.ConfigInstance.LubanPath) => JsonConfig.ConfigInstance.LubanPath,
                        nameof(JsonConfig.ConfigInstance.DataPath) => JsonConfig.ConfigInstance.DataPath,
                        nameof(JsonConfig.ConfigInstance.ScriptsPath) => JsonConfig.ConfigInstance.ScriptsPath,
                        nameof(JsonConfig.ConfigInstance.LubanConfigPath) =>
                            Path.GetDirectoryName(JsonConfig.ConfigInstance.LubanConfigPath) ?? "",
                        nameof(JsonConfig.ConfigInstance.ProtoBufPath) =>
                            Path.GetDirectoryName(JsonConfig.ConfigInstance.ProtoBufPath) ?? "",
                        _ => "",
                    };
                    if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                        Process.Start("explorer.exe", path);
                    else if (!string.IsNullOrEmpty(path) && File.Exists(path))
                        Process.Start("explorer.exe", $"/select,\"{path}\"");
                };
            }

            // GroupBox 拉伸时同步输入框宽度
            groupPaths.Resize += (_, _) =>
            {
                foreach (var (TB, _, _) in pathRows)
                    TB.Width = Math.Max(200, groupPaths.Width - TB.Left - btnColWidth);
            };
            // 初始 + Load 各触发一次确保宽度正确
            void FixWidths()
            {
                foreach (var (TB, _, _) in pathRows)
                    TB.Width = Math.Max(200, groupPaths.Width - TB.Left - btnColWidth);
            }
            FixWidths();
            Load += (_, _) => FixWidths();

            // ---- GroupBox 2：导出格式 ----
            var groupFormats = new GroupBox
            {
                Text = "  导出格式  ",
                Location = new Point(gx, groupPaths.Bottom + 6),
                Size = new Size(tab.Width - gx * 2, 170),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(78, 87, 100),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Parent = tab
            };

            var formatControls = new Control[]
            {
                dungeonLabel1, dungeonLabel2,
                airCheckBox1, airCheckBox2_cs_bin, airCheckBox6Protobuf_bin,
                airCheckBox5, airCheckBox3bin_cs, airCheckBox4Protobuf_cs,
            };
            foreach (var ctrl in formatControls)
            {
                ctrl.Parent = groupFormats;
                ctrl.Location = new Point(ctrl.Left - groupFormats.Left, ctrl.Top - groupFormats.Top);
            }

            _chkUseSubfolders = new CheckBox
            {
                Text = "每种格式建立子文件夹（推荐）",
                Checked = true,
                Location = new Point(8, 134),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(76, 76, 77),
                Parent = groupFormats,
            };
            _chkUseSubfolders.CheckedChanged += (_, _) =>
            {
                if (JsonConfig.ConfigInstance == null) return;
                JsonConfig.ConfigInstance.use_subfolders = _chkUseSubfolders.Checked;
                JsonConfig.ConfigInstance.SaveConfig();
            };

            // ---- 全局美化 ----
            Text = "Luban 数据导出管理器";
            airForm1.Text = "Luban 数据导出管理器";
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
        /// 命令行输出 — 独立弹窗，自动跟随主窗口
        /// </summary>
        private void SetupLogWindow()
        {
            _logForm = new Form
            {
                Text = "命令输出",
                Size = new Size(550, 480),
                StartPosition = FormStartPosition.Manual,
                Owner = this,
                Icon = SystemIcons.Application,
            };
            // 关闭 = 隐藏；主窗口关闭时（FormOwnerClosing）自动允许
            _logForm.FormClosing += (_, e) =>
            {
                if (e.CloseReason == CloseReason.FormOwnerClosing)
                    return; // 主窗口在关 → 允许一起关
                e.Cancel = true;
                _logForm.Hide();
            };
            _logForm.FormClosed += (_, _) => { _logForm = null!; };
            // 主窗口移动时同步
            Move += (_, _) =>
            {
                if (_logForm?.Visible == true)
                    _logForm.Location = new Point(Right + 4, Top);
            };

            // 工具栏
            var toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 32,
                BackColor = Color.FromArgb(50, 50, 50),
                Parent = _logForm,
            };
            var btnClear = new Button
            {
                Text = "清空",
                Size = new Size(50, 24),
                Location = new Point(4, 4),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(180, 180, 180),
                BackColor = Color.FromArgb(70, 70, 70),
                Font = new Font("Segoe UI", 8F),
                Parent = toolbar,
            };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Click += (_, _) => _logBox.Clear();

            var lbl = new Label
            {
                Text = "导出时自动打开  ·  关闭窗口即隐藏",
                AutoSize = true,
                Location = new Point(62, 7),
                ForeColor = Color.FromArgb(120, 120, 120),
                Font = new Font("Segoe UI", 8F),
                Parent = toolbar,
            };

            // 日志文本框
            _logBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.FromArgb(200, 200, 200),
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                WordWrap = true,
                Parent = _logForm,
            };

            AppendLog("就绪。点击「导出全部」开始。");
        }

        /// <summary>
        /// 确保日志窗口可见
        /// </summary>
        private void ShowLogWindow()
        {
            if (_logForm == null || _logForm.IsDisposed)
            {
                SetupLogWindow();
            }
            if (_logForm.Visible) return;
            _logForm.Location = new Point(Right + 4, Top);
            _logForm.Show();
            _logForm.BringToFront();
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
        /// 检测 .NET 8 运行时是否已安装（不阻塞，最多等 2 秒）
        /// </summary>
        private static async Task<bool> IsDotNet8InstalledAsync()
        {
            try
            {
                using var p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = "--list-runtimes",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                    },
                };
                p.Start();
                // 最多等 3 秒，防止 dotnet 进程卡死
                var readTask = p.StandardOutput.ReadToEndAsync();
                var timeoutTask = Task.Delay(3000);
                var completed = await Task.WhenAny(readTask, timeoutTask);
                if (completed == timeoutTask) return false;
                return readTask.Result.Contains("Microsoft.NETCore.App 8.0");
            }
            catch
            {
                // dotnet 不在 PATH，扫目录
                var dirs = new[]
                {
                    @"C:\Program Files\dotnet\shared\Microsoft.NETCore.App",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "dotnet", "shared", "Microsoft.NETCore.App"),
                };
                foreach (var d in dirs)
                {
                    if (!Directory.Exists(d)) continue;
                    if (Directory.GetDirectories(d, "8.0.*").Length > 0) return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 确保 .NET 8 运行时可用；缺少时提示并打开文件夹让用户手动安装
        /// </summary>
        private async Task<bool> EnsureDotNetRuntimeAsync()
        {
            // 异步检测，不卡 UI
            var installed = await IsDotNet8InstalledAsync();
            if (installed)
            {
                AppendLog(".NET 8 运行时 ✓ 已安装");
                return true;
            }

            AppendLog("未检测到 .NET 8 运行时", isError: true);

            string baseDir = Path.GetDirectoryName(Application.ExecutablePath);
            var installerPath = Path.Combine(baseDir, "plugin", "dotnet-runtime-8.0.28-win-x64.exe");
            var pluginDir = Path.Combine(baseDir, "plugin");

            var result = MessageBox.Show(
                $"未检测到 .NET 8 运行时，Luban 需要它才能运行。\n\n" +
                $"安装包位置：\n{installerPath}\n\n" +
                $"点击「确定」打开所在文件夹，请手动双击安装。",
                ".NET 8 运行时缺失",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                // 打开文件夹定位到安装包
                if (File.Exists(installerPath))
                    Process.Start("explorer.exe", $"/select,\"{installerPath}\"");
                else
                    Process.Start("explorer.exe", pluginDir);
            }

            return false;
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
            if (_chkUseSubfolders != null)
                _chkUseSubfolders.Checked = JsonConfig.ConfigInstance.use_subfolders;

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

            ShowLogWindow();
            _logBox?.Clear();
            AppendLog("======== 开始导出 ========");

            // 同步 Excel 路径到 luban_config.json 的 dataDir
            SyncLubanConfDataDir();

            // 构建 Luban 参数
            var args = new StringBuilder();
            args.Append($"--conf \"{JsonConfig.ConfigInstance.LubanConfigPath}\" ");
            args.Append("-t all ");
            args.Append("-s default ");

            // 子文件夹开关：勾选 → DataPath/json/，不勾选 → DataPath/
            bool sf = JsonConfig.ConfigInstance.use_subfolders;
            string dd(string name) => sf ? $"\u0022{JsonConfig.ConfigInstance.DataPath}/{name}\u0022" : $"\u0022{JsonConfig.ConfigInstance.DataPath}\u0022";
            string cd(string name) => sf ? $"\u0022{JsonConfig.ConfigInstance.ScriptsPath}/{name}\u0022" : $"\u0022{JsonConfig.ConfigInstance.ScriptsPath}\u0022";

            if (JsonConfig.ConfigInstance.json_data)
            {
                args.Append("-d json ");
                args.Append($"-x json.outputDataDir={dd("json")} ");
            }
            if (JsonConfig.ConfigInstance.json_cs)
            {
                args.Append("-c cs-simple-json ");
                args.Append($"-x cs-simple-json.outputCodeDir={cd("cs_json")} ");
            }
            if (JsonConfig.ConfigInstance.cs_bin)
            {
                args.Append("-c cs-bin ");
                args.Append($"-x cs-bin.outputCodeDir={cd("cs_bin")} ");
            }
            if (JsonConfig.ConfigInstance.bin_cs)
            {
                args.Append("-d bin ");
                args.Append($"-x bin.outputDataDir={dd("bin")} ");
            }
            if (JsonConfig.ConfigInstance.protobuf_bin)
            {
                args.Append("-d protobuf3-bin ");
                args.Append($"-x protobuf3-bin.outputDataDir={dd("protobuf3_bin")} ");
            }
            if (JsonConfig.ConfigInstance.protobuf_cs)
            {
                args.Append("-c protobuf3 ");
                args.Append("-c cs-protobuf3 ");
                args.Append($"-x protobuf3.outputCodeDir={cd("protobuf3")} ");
                args.Append($"-x cs-protobuf3.outputCodeDir={cd("cs_protobuf3")} ");
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
                var protoDir = sf
                    ? Path.Combine(JsonConfig.ConfigInstance.ScriptsPath, "protobuf3")
                    : JsonConfig.ConfigInstance.ScriptsPath;
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

        /// <summary>
        /// 把当前 Excel 路径的绝对路径写入 luban_config.json 的 dataDir 字段
        /// </summary>
        private void SyncLubanConfDataDir()
        {
            try
            {
                var confPath = JsonConfig.ConfigInstance.LubanConfigPath;
                if (!File.Exists(confPath)) return;

                var text = File.ReadAllText(confPath);
                var json = System.Text.Json.Nodes.JsonNode.Parse(text);
                if (json == null) return;

                var newDir = JsonConfig.ConfigInstance.ExcelsPath;
                var oldDir = (string?)json["dataDir"];

                if (oldDir == newDir) return; // 没变化

                json["dataDir"] = newDir;
                File.WriteAllText(confPath,
                    json.ToJsonString(new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
                AppendLog($"config dataDir: {newDir}");
            }
            catch (Exception ex)
            {
                AppendLog($"更新 luban_config 失败: {ex.Message}", isError: true);
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
