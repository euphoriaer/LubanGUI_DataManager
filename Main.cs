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
        internal void RefreshSetting()
        {
            airCheckBox2_cs_bin.Checked = JsonConfig.ConfigInstance.cs_bin;
            airCheckBox3bin_cs.Checked = JsonConfig.ConfigInstance.bin_cs;
            aloneTextBox1Luban.Text = JsonConfig.ConfigInstance.LubanPath;
            aloneTextBox2Data.Text = JsonConfig.ConfigInstance.DataPath;
            aloneTextBox3Script.Text = JsonConfig.ConfigInstance.ScriptsPath;
            aloneTextBox1Excel.Text = JsonConfig.ConfigInstance.ExcelsPath;
            //excel 文件夹存在
            if(!string.IsNullOrEmpty(JsonConfig.ConfigInstance.ExcelsPath))
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
