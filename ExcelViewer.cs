using ClosedXML.Excel;
using System.Data;

namespace ExcelDataExport;

/// <summary>
/// Excel 查看/编辑窗口 — 双击文件列表时打开，修改单元格自动保存
/// </summary>
public class ExcelViewer : Form
{
    private readonly string _filePath;
    private readonly string _fileName;
    private DataGridView _grid = null!;
    private TextBox _txtSearch = null!;
    private Label _lblStatus = null!;
    private System.Windows.Forms.Timer _saveTimer = null!;
    private bool _saving;

    public ExcelViewer(string filePath, string fileName)
    {
        _filePath = filePath;
        _fileName = fileName;

        BuildUI();
        LoadExcel();
    }

    // -------------------------------------------------------
    // UI 构建
    // -------------------------------------------------------
    private void BuildUI()
    {
        Text = _fileName;
        Size = new Size(1100, 700);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.White;

        // ---- 顶部栏：搜索 + 状态 ----
        var topBar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 36,
            BackColor = Color.FromArgb(245, 245, 245),
            Parent = this,
        };

        _txtSearch = new TextBox
        {
            Text = "🔍 输入关键词过滤行...",
            Size = new Size(260, 26),
            Location = new Point(8, 6),
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.Gray,
            BorderStyle = BorderStyle.None,
            BackColor = Color.White,
            Parent = topBar,
        };
        _txtSearch.Enter += (_, _) =>
        {
            if (_txtSearch.Text == "🔍 输入关键词过滤行...")
            {
                _txtSearch.Text = "";
                _txtSearch.ForeColor = Color.Black;
            }
        };
        _txtSearch.Leave += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(_txtSearch.Text))
            {
                _txtSearch.Text = "🔍 输入关键词过滤行...";
                _txtSearch.ForeColor = Color.Gray;
                ShowAllRows();
            }
        };
        _txtSearch.TextChanged += OnSearchTextChanged;

        _lblStatus = new Label
        {
            Text = $"📂 {_fileName}",
            AutoSize = true,
            Location = new Point(280, 9),
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.FromArgb(120, 120, 120),
            Parent = topBar,
        };

        var sep = new Label
        {
            Text = "|",
            AutoSize = true,
            Location = new Point(272, 9),
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.FromArgb(200, 200, 200),
            Parent = topBar,
        };

        // ---- 自动保存定时器 ----
        _saveTimer = new System.Windows.Forms.Timer { Interval = 500 };
        _saveTimer.Tick += (_, _) =>
        {
            _saveTimer.Stop();
            DoSave();
        };

        // ---- DataGridView ----
        _grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(78, 87, 100),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
            },
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.Black,
                SelectionBackColor = Color.FromArgb(0, 120, 215),
                SelectionForeColor = Color.White,
            },
            EnableHeadersVisualStyles = false,
            GridColor = Color.FromArgb(230, 230, 230),
            RowHeadersVisible = false,
            RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing,
            SelectionMode = DataGridViewSelectionMode.CellSelect,
            Parent = this,
        };
        _grid.CellValueChanged += OnCellChanged;
        _grid.RowPostPaint += OnRowPostPaint;

        // 确保 DataGridView 在顶部栏下方
        _grid.Top = 0;
    }

    // -------------------------------------------------------
    // 行号绘制
    // -------------------------------------------------------
    private void OnRowPostPaint(object? sender, DataGridViewRowPostPaintEventArgs e)
    {
        // 绘制行号（Excel 风格）
        var num = (e.RowIndex + 1).ToString();
        var rect = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, 40, e.RowBounds.Height);
        using var brush = new SolidBrush(Color.FromArgb(160, 160, 160));
        using var font = new Font("Segoe UI", 8.5F);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        e.Graphics.DrawString(num, font, brush, rect, sf);
    }

    // -------------------------------------------------------
    // Excel 加载
    // -------------------------------------------------------
    private void LoadExcel()
    {
        try
        {
            using var wb = new XLWorkbook(_filePath);
            var ws = wb.Worksheet(1);

            var usedRange = ws.RangeUsed();
            if (usedRange is null)
            {
                _grid.Columns.Add("A", "A");
                return;
            }

            int rowCount = usedRange.RowCount();
            int colCount = usedRange.ColumnCount();

            // 建列
            _grid.Columns.Clear();
            for (int c = 0; c < colCount; c++)
            {
                _grid.Columns.Add(GetExcelColumnName(c + 1), GetExcelColumnName(c + 1));
            }

            // 读取行数据
            for (int r = 1; r <= rowCount; r++)
            {
                var values = new object[colCount];
                bool hasData = false;

                for (int c = 0; c < colCount; c++)
                {
                    var cell = ws.Cell(r, c + 1);
                    string? value = null;

                    if (!cell.IsEmpty())
                    {
                        value = cell.DataType switch
                        {
                            XLDataType.Text => cell.GetText(),
                            XLDataType.Number => cell.Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                            XLDataType.DateTime => cell.GetDateTime().ToString("yyyy-MM-dd HH:mm:ss"),
                            XLDataType.Boolean => cell.GetBoolean() ? "true" : "false",
                            _ => cell.GetText(),
                        };
                        hasData = true;
                    }
                    else if (r == 1)
                    {
                        // 第一行保留原标题，即使有空格
                        value = cell.GetText();
                        hasData = true;
                    }

                    values[c] = value ?? "";
                }

                if (hasData && r <= rowCount)
                {
                    _grid.Rows.Add(values);
                }
            }

            // 隐藏全部为空的尾部列
            for (int c = colCount - 1; c >= 0; c--)
            {
                bool allEmpty = true;
                foreach (DataGridViewRow row in _grid.Rows)
                {
                    if (row.Cells[c].Value != null && !string.IsNullOrWhiteSpace(row.Cells[c].Value?.ToString()))
                    {
                        allEmpty = false;
                        break;
                    }
                }
                if (allEmpty && c > 3)
                {
                    _grid.Columns[c].Visible = false;
                }
            }

            // 调整行边距给行号留空间
            _grid.RowTemplate.DefaultCellStyle.Padding = new Padding(44, 0, 0, 0);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"加载 Excel 文件失败：{ex.Message}", "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // -------------------------------------------------------
    // 保存
    // -------------------------------------------------------
    private void DoSave()
    {
        if (_saving) return;
        _saving = true;

        try
        {
            using var wb = new XLWorkbook(_filePath);
            var ws = wb.Worksheet(1);

            // 清空旧数据
            var used = ws.RangeUsed();
            if (used != null)
            {
                // 先扩展范围以覆盖 grid 中的所有行
                int maxRow = Math.Max(used.RowCount(), _grid.Rows.Count);
                int maxCol = Math.Max(used.ColumnCount(), _grid.Columns.Count);
                ws.Range(1, 1, maxRow, maxCol).Clear();
            }

            // 写入新数据
            for (int r = 0; r < _grid.Rows.Count; r++)
            {
                for (int c = 0; c < _grid.Columns.Count; c++)
                {
                    if (!_grid.Columns[c].Visible) continue;

                    var value = _grid.Rows[r].Cells[c].Value;
                    var str = value?.ToString()?.Trim() ?? "";

                    if (string.IsNullOrEmpty(str)) continue;

                    if (decimal.TryParse(str,
                        System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out var num))
                    {
                        ws.Cell(r + 1, c + 1).SetValue(num);
                    }
                    else if (bool.TryParse(str, out var b))
                    {
                        ws.Cell(r + 1, c + 1).SetValue(b);
                    }
                    else
                    {
                        ws.Cell(r + 1, c + 1).SetValue(str);
                    }
                }
            }

            wb.Save();

            _lblStatus.Text = $"✓ 已保存 {DateTime.Now:HH:mm:ss}";
            _lblStatus.ForeColor = Color.FromArgb(0, 150, 100);
        }
        catch (Exception ex)
        {
            _lblStatus.Text = $"✗ 保存失败: {ex.Message}";
            _lblStatus.ForeColor = Color.Red;
        }
        finally
        {
            _saving = false;
        }
    }

    // -------------------------------------------------------
    // 单元格修改 → 触发延迟自动保存
    // -------------------------------------------------------
    private void OnCellChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        _lblStatus.Text = "● 修改中...";
        _lblStatus.ForeColor = Color.FromArgb(200, 150, 0);

        // 重置定时器：500ms 内无新修改即保存
        _saveTimer.Stop();
        _saveTimer.Start();
    }

    // -------------------------------------------------------
    // 搜索 / 过滤
    // -------------------------------------------------------
    private void OnSearchTextChanged(object? sender, EventArgs e)
    {
        string query = _txtSearch.Text.Trim().ToLowerInvariant();
        if (query == "🔍 输入关键词过滤行..." || string.IsNullOrEmpty(query))
        {
            ShowAllRows();
            return;
        }

        foreach (DataGridViewRow row in _grid.Rows)
        {
            bool match = false;
            foreach (DataGridViewCell cell in row.Cells)
            {
                var text = cell.Value?.ToString()?.ToLowerInvariant() ?? "";
                if (text.Contains(query))
                {
                    match = true;
                    break;
                }
            }
            row.Visible = match;
        }
    }

    private void ShowAllRows()
    {
        foreach (DataGridViewRow row in _grid.Rows)
            row.Visible = true;
    }

    // -------------------------------------------------------
    // 快捷键
    // -------------------------------------------------------
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Control | Keys.S:
                _saveTimer.Stop();
                DoSave();
                return true;
            case Keys.Escape:
                _txtSearch.Text = "";
                _grid.Focus();
                return true;
            case Keys.Control | Keys.F:
                _txtSearch.Focus();
                _txtSearch.SelectAll();
                return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    // -------------------------------------------------------
    // 关闭时确保保存
    // -------------------------------------------------------
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _saveTimer.Stop();
        DoSave(); // 最后保存一次
        base.OnFormClosing(e);
    }

    // -------------------------------------------------------
    // 工具方法
    // -------------------------------------------------------
    private static string GetExcelColumnName(int columnNumber)
    {
        string result = "";
        while (columnNumber > 0)
        {
            columnNumber--;
            result = (char)('A' + columnNumber % 26) + result;
            columnNumber /= 26;
        }
        return result;
    }
}
