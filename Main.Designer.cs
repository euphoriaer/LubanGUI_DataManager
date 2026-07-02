namespace ExcelDataExport
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            airTabPage1 = new ReaLTaiizor.Controls.AirTabPage();
            tabPage1 = new TabPage();
            parrotToolStrip1 = new ReaLTaiizor.Controls.ParrotToolStrip();
            toolStripButton1 = new ToolStripButton();
            toolStripProgressBar1 = new ToolStripProgressBar();
            excelListBox = new ListView();
            columnHeader1 = new ColumnHeader();
            设置 = new TabPage();
            aloneButton5ProtoBuf = new ReaLTaiizor.Controls.AloneButton();
            aloneTextBox1ProtoBufPath = new ReaLTaiizor.Controls.AloneTextBox();
            aloneButton5Luban_Config = new ReaLTaiizor.Controls.AloneButton();
            aloneTextBox1LubanConfig = new ReaLTaiizor.Controls.AloneTextBox();
            aloneButton4 = new ReaLTaiizor.Controls.AloneButton();
            aloneTextBox1Excel = new ReaLTaiizor.Controls.AloneTextBox();
            airCheckBox6Protobuf_bin = new ReaLTaiizor.Controls.AirCheckBox();
            airCheckBox5 = new ReaLTaiizor.Controls.AirCheckBox();
            airCheckBox4Protobuf_cs = new ReaLTaiizor.Controls.AirCheckBox();
            airCheckBox3bin_cs = new ReaLTaiizor.Controls.AirCheckBox();
            dungeonLabel2 = new ReaLTaiizor.Controls.DungeonLabel();
            dungeonLabel1 = new ReaLTaiizor.Controls.DungeonLabel();
            airCheckBox2_cs_bin = new ReaLTaiizor.Controls.AirCheckBox();
            airCheckBox1 = new ReaLTaiizor.Controls.AirCheckBox();
            aloneButton3 = new ReaLTaiizor.Controls.AloneButton();
            aloneButton2 = new ReaLTaiizor.Controls.AloneButton();
            aloneButton1 = new ReaLTaiizor.Controls.AloneButton();
            aloneTextBox3Script = new ReaLTaiizor.Controls.AloneTextBox();
            aloneTextBox2Data = new ReaLTaiizor.Controls.AloneTextBox();
            aloneTextBox1Luban = new ReaLTaiizor.Controls.AloneTextBox();
            tabPage2 = new TabPage();
            dungeonRichTextBox1ToolTips = new ReaLTaiizor.Controls.DungeonRichTextBox();
            airForm1 = new ReaLTaiizor.Forms.AirForm();
            airTabPage1.SuspendLayout();
            tabPage1.SuspendLayout();
            parrotToolStrip1.SuspendLayout();
            设置.SuspendLayout();
            tabPage2.SuspendLayout();
            airForm1.SuspendLayout();
            SuspendLayout();
            // 
            // airTabPage1
            // 
            airTabPage1.Alignment = TabAlignment.Left;
            airTabPage1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            airTabPage1.BaseColor = Color.White;
            airTabPage1.Controls.Add(tabPage1);
            airTabPage1.Controls.Add(设置);
            airTabPage1.Controls.Add(tabPage2);
            airTabPage1.ItemSize = new Size(30, 115);
            airTabPage1.Location = new Point(4, 40);
            airTabPage1.Margin = new Padding(4);
            airTabPage1.Multiline = true;
            airTabPage1.Name = "airTabPage1";
            airTabPage1.NormalTextColor = Color.DimGray;
            airTabPage1.SelectedIndex = 0;
            airTabPage1.SelectedTabBackColor = Color.White;
            airTabPage1.SelectedTextColor = Color.Black;
            airTabPage1.ShowOuterBorders = false;
            airTabPage1.Size = new Size(1021, 662);
            airTabPage1.SizeMode = TabSizeMode.Fixed;
            airTabPage1.SquareColor = Color.FromArgb(78, 87, 100);
            airTabPage1.TabCursor = Cursors.Hand;
            airTabPage1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.White;
            tabPage1.Controls.Add(parrotToolStrip1);
            tabPage1.Controls.Add(excelListBox);
            tabPage1.Location = new Point(119, 4);
            tabPage1.Margin = new Padding(4);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4);
            tabPage1.Size = new Size(898, 654);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "导出";
            // 
            // parrotToolStrip1
            // 
            parrotToolStrip1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            parrotToolStrip1.BackColor = Color.White;
            parrotToolStrip1.BorderColor = Color.DodgerBlue;
            parrotToolStrip1.Dock = DockStyle.None;
            parrotToolStrip1.ForeColor = Color.Black;
            parrotToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            parrotToolStrip1.ImageScalingSize = new Size(20, 20);
            parrotToolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1, toolStripProgressBar1 });
            parrotToolStrip1.Location = new Point(4, 4);
            parrotToolStrip1.Name = "parrotToolStrip1";
            parrotToolStrip1.RenderMode = ToolStripRenderMode.System;
            parrotToolStrip1.RightToLeft = RightToLeft.No;
            parrotToolStrip1.Size = new Size(178, 40);
            parrotToolStrip1.TabIndex = 1;
            parrotToolStrip1.Text = "parrotToolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(73, 37);
            toolStripButton1.Text = "导出全部";
            toolStripButton1.TextImageRelation = TextImageRelation.ImageAboveText;
            toolStripButton1.Click += ExportAllExcel_Click;
            // 
            // toolStripProgressBar1
            // 
            toolStripProgressBar1.Name = "toolStripProgressBar1";
            toolStripProgressBar1.Size = new Size(100, 37);
            // 
            // excelListBox
            // 
            excelListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            excelListBox.BackColor = Color.White;
            excelListBox.BorderStyle = BorderStyle.None;
            excelListBox.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
            excelListBox.Font = new Font("Segoe UI", 11F);
            excelListBox.FullRowSelect = true;
            excelListBox.HeaderStyle = ColumnHeaderStyle.None;
            excelListBox.Location = new Point(0, 54);
            excelListBox.Margin = new Padding(4);
            excelListBox.MultiSelect = false;
            excelListBox.Name = "excelListBox";
            excelListBox.Size = new Size(858, 595);
            excelListBox.TabIndex = 0;
            excelListBox.UseCompatibleStateImageBehavior = false;
            excelListBox.View = View.Details;
            excelListBox.DoubleClick += excelListBox_DoubleClick;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "文件名";
            columnHeader1.Width = 850;
            // 
            // 设置
            // 
            设置.BackColor = Color.White;
            设置.Controls.Add(aloneButton5ProtoBuf);
            设置.Controls.Add(aloneTextBox1ProtoBufPath);
            设置.Controls.Add(aloneButton5Luban_Config);
            设置.Controls.Add(aloneTextBox1LubanConfig);
            设置.Controls.Add(aloneButton4);
            设置.Controls.Add(aloneTextBox1Excel);
            设置.Controls.Add(airCheckBox6Protobuf_bin);
            设置.Controls.Add(airCheckBox5);
            设置.Controls.Add(airCheckBox4Protobuf_cs);
            设置.Controls.Add(airCheckBox3bin_cs);
            设置.Controls.Add(dungeonLabel2);
            设置.Controls.Add(dungeonLabel1);
            设置.Controls.Add(airCheckBox2_cs_bin);
            设置.Controls.Add(airCheckBox1);
            设置.Controls.Add(aloneButton3);
            设置.Controls.Add(aloneButton2);
            设置.Controls.Add(aloneButton1);
            设置.Controls.Add(aloneTextBox3Script);
            设置.Controls.Add(aloneTextBox2Data);
            设置.Controls.Add(aloneTextBox1Luban);
            设置.Location = new Point(119, 4);
            设置.Margin = new Padding(4);
            设置.Name = "设置";
            设置.Padding = new Padding(4);
            设置.Size = new Size(898, 654);
            设置.TabIndex = 1;
            设置.Text = "设置";
            // 
            // aloneButton5ProtoBuf
            // 
            aloneButton5ProtoBuf.BackColor = Color.Transparent;
            aloneButton5ProtoBuf.EnabledCalc = true;
            aloneButton5ProtoBuf.Font = new Font("Segoe UI", 9F);
            aloneButton5ProtoBuf.ForeColor = Color.FromArgb(124, 133, 142);
            aloneButton5ProtoBuf.Location = new Point(572, 401);
            aloneButton5ProtoBuf.Margin = new Padding(4);
            aloneButton5ProtoBuf.Name = "aloneButton5ProtoBuf";
            aloneButton5ProtoBuf.Size = new Size(135, 33);
            aloneButton5ProtoBuf.TabIndex = 19;
            aloneButton5ProtoBuf.Text = "protobuf.exe路径";
            aloneButton5ProtoBuf.Click += aloneButton5ProtoBuf_Click;
            // 
            // aloneTextBox1ProtoBufPath
            // 
            aloneTextBox1ProtoBufPath.BackColor = Color.Transparent;
            aloneTextBox1ProtoBufPath.EnabledCalc = true;
            aloneTextBox1ProtoBufPath.Font = new Font("Segoe UI", 9F);
            aloneTextBox1ProtoBufPath.ForeColor = Color.FromArgb(124, 133, 142);
            aloneTextBox1ProtoBufPath.Location = new Point(8, 386);
            aloneTextBox1ProtoBufPath.Margin = new Padding(4);
            aloneTextBox1ProtoBufPath.MaxLength = 32767;
            aloneTextBox1ProtoBufPath.MultiLine = false;
            aloneTextBox1ProtoBufPath.Name = "aloneTextBox1ProtoBufPath";
            aloneTextBox1ProtoBufPath.ReadOnly = false;
            aloneTextBox1ProtoBufPath.Size = new Size(557, 62);
            aloneTextBox1ProtoBufPath.TabIndex = 18;
            aloneTextBox1ProtoBufPath.Text = "aloneTextBox1";
            aloneTextBox1ProtoBufPath.TextAlign = HorizontalAlignment.Left;
            aloneTextBox1ProtoBufPath.UseSystemPasswordChar = false;
            aloneTextBox1ProtoBufPath.Leave += textBox_Leave;
            // 
            // aloneButton5Luban_Config
            // 
            aloneButton5Luban_Config.BackColor = Color.Transparent;
            aloneButton5Luban_Config.EnabledCalc = true;
            aloneButton5Luban_Config.Font = new Font("Segoe UI", 9F);
            aloneButton5Luban_Config.ForeColor = Color.FromArgb(124, 133, 142);
            aloneButton5Luban_Config.Location = new Point(575, 332);
            aloneButton5Luban_Config.Margin = new Padding(4);
            aloneButton5Luban_Config.Name = "aloneButton5Luban_Config";
            aloneButton5Luban_Config.Size = new Size(135, 33);
            aloneButton5Luban_Config.TabIndex = 17;
            aloneButton5Luban_Config.Text = "luban配置路径";
            aloneButton5Luban_Config.Click += aloneButton5Luban_Config_Click;
            // 
            // aloneTextBox1LubanConfig
            // 
            aloneTextBox1LubanConfig.BackColor = Color.Transparent;
            aloneTextBox1LubanConfig.EnabledCalc = true;
            aloneTextBox1LubanConfig.Font = new Font("Segoe UI", 9F);
            aloneTextBox1LubanConfig.ForeColor = Color.FromArgb(124, 133, 142);
            aloneTextBox1LubanConfig.Location = new Point(10, 316);
            aloneTextBox1LubanConfig.Margin = new Padding(4);
            aloneTextBox1LubanConfig.MaxLength = 32767;
            aloneTextBox1LubanConfig.MultiLine = false;
            aloneTextBox1LubanConfig.Name = "aloneTextBox1LubanConfig";
            aloneTextBox1LubanConfig.ReadOnly = false;
            aloneTextBox1LubanConfig.Size = new Size(557, 62);
            aloneTextBox1LubanConfig.TabIndex = 16;
            aloneTextBox1LubanConfig.Text = "aloneTextBox1";
            aloneTextBox1LubanConfig.TextAlign = HorizontalAlignment.Left;
            aloneTextBox1LubanConfig.UseSystemPasswordChar = false;
            aloneTextBox1LubanConfig.Leave += textBox_Leave;
            // 
            // aloneButton4
            // 
            aloneButton4.BackColor = Color.Transparent;
            aloneButton4.EnabledCalc = true;
            aloneButton4.Font = new Font("Segoe UI", 9F);
            aloneButton4.ForeColor = Color.FromArgb(124, 133, 142);
            aloneButton4.Location = new Point(572, 53);
            aloneButton4.Margin = new Padding(4);
            aloneButton4.Name = "aloneButton4";
            aloneButton4.Size = new Size(135, 33);
            aloneButton4.TabIndex = 15;
            aloneButton4.Text = "Excel路径";
            aloneButton4.Click += aloneButton4_Click;
            // 
            // aloneTextBox1Excel
            // 
            aloneTextBox1Excel.BackColor = Color.Transparent;
            aloneTextBox1Excel.EnabledCalc = true;
            aloneTextBox1Excel.Font = new Font("Segoe UI", 9F);
            aloneTextBox1Excel.ForeColor = Color.FromArgb(124, 133, 142);
            aloneTextBox1Excel.Location = new Point(8, 39);
            aloneTextBox1Excel.Margin = new Padding(4);
            aloneTextBox1Excel.MaxLength = 32767;
            aloneTextBox1Excel.MultiLine = false;
            aloneTextBox1Excel.Name = "aloneTextBox1Excel";
            aloneTextBox1Excel.ReadOnly = false;
            aloneTextBox1Excel.Size = new Size(557, 62);
            aloneTextBox1Excel.TabIndex = 14;
            aloneTextBox1Excel.Text = "aloneTextBox1";
            aloneTextBox1Excel.TextAlign = HorizontalAlignment.Left;
            aloneTextBox1Excel.UseSystemPasswordChar = false;
            aloneTextBox1Excel.Leave += textBox_Leave;
            // 
            // airCheckBox6Protobuf_bin
            // 
            airCheckBox6Protobuf_bin.Checked = false;
            airCheckBox6Protobuf_bin.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8=";
            airCheckBox6Protobuf_bin.Font = new Font("Segoe UI", 9F);
            airCheckBox6Protobuf_bin.Image = null;
            airCheckBox6Protobuf_bin.Location = new Point(14, 582);
            airCheckBox6Protobuf_bin.Margin = new Padding(4);
            airCheckBox6Protobuf_bin.Name = "airCheckBox6Protobuf_bin";
            airCheckBox6Protobuf_bin.NoRounding = false;
            airCheckBox6Protobuf_bin.Size = new Size(118, 17);
            airCheckBox6Protobuf_bin.TabIndex = 13;
            airCheckBox6Protobuf_bin.Text = "protobuf3";
            airCheckBox6Protobuf_bin.Transparent = false;
            airCheckBox6Protobuf_bin.CheckedChanged += airCheckBox6_CheckedChanged;
            // 
            // airCheckBox5
            // 
            airCheckBox5.Checked = false;
            airCheckBox5.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8=";
            airCheckBox5.Font = new Font("Segoe UI", 9F);
            airCheckBox5.Image = null;
            airCheckBox5.Location = new Point(206, 528);
            airCheckBox5.Margin = new Padding(4);
            airCheckBox5.Name = "airCheckBox5";
            airCheckBox5.NoRounding = false;
            airCheckBox5.Size = new Size(118, 17);
            airCheckBox5.TabIndex = 12;
            airCheckBox5.Text = "json-cs";
            airCheckBox5.Transparent = false;
            // 
            // airCheckBox4Protobuf_cs
            // 
            airCheckBox4Protobuf_cs.Checked = false;
            airCheckBox4Protobuf_cs.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8=";
            airCheckBox4Protobuf_cs.Font = new Font("Segoe UI", 9F);
            airCheckBox4Protobuf_cs.Image = null;
            airCheckBox4Protobuf_cs.Location = new Point(206, 582);
            airCheckBox4Protobuf_cs.Margin = new Padding(4);
            airCheckBox4Protobuf_cs.Name = "airCheckBox4Protobuf_cs";
            airCheckBox4Protobuf_cs.NoRounding = false;
            airCheckBox4Protobuf_cs.Size = new Size(118, 17);
            airCheckBox4Protobuf_cs.TabIndex = 11;
            airCheckBox4Protobuf_cs.Text = "protobuf3-cs";
            airCheckBox4Protobuf_cs.Transparent = false;
            airCheckBox4Protobuf_cs.CheckedChanged += airCheckBox4_CheckedChanged;
            // 
            // airCheckBox3bin_cs
            // 
            airCheckBox3bin_cs.Checked = false;
            airCheckBox3bin_cs.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8=";
            airCheckBox3bin_cs.Font = new Font("Segoe UI", 9F);
            airCheckBox3bin_cs.Image = null;
            airCheckBox3bin_cs.Location = new Point(206, 555);
            airCheckBox3bin_cs.Margin = new Padding(4);
            airCheckBox3bin_cs.Name = "airCheckBox3bin_cs";
            airCheckBox3bin_cs.NoRounding = false;
            airCheckBox3bin_cs.Size = new Size(118, 17);
            airCheckBox3bin_cs.TabIndex = 10;
            airCheckBox3bin_cs.Text = "bin-cs";
            airCheckBox3bin_cs.Transparent = false;
            airCheckBox3bin_cs.CheckedChanged += airCheckBox3bin_cs_CheckedChanged;
            // 
            // dungeonLabel2
            // 
            dungeonLabel2.AutoSize = true;
            dungeonLabel2.BackColor = Color.Transparent;
            dungeonLabel2.Font = new Font("Segoe UI", 11F);
            dungeonLabel2.ForeColor = Color.FromArgb(76, 76, 77);
            dungeonLabel2.Location = new Point(206, 485);
            dungeonLabel2.Margin = new Padding(4, 0, 4, 0);
            dungeonLabel2.Name = "dungeonLabel2";
            dungeonLabel2.Size = new Size(52, 25);
            dungeonLabel2.TabIndex = 9;
            dungeonLabel2.Text = "代码";
            // 
            // dungeonLabel1
            // 
            dungeonLabel1.AutoSize = true;
            dungeonLabel1.BackColor = Color.Transparent;
            dungeonLabel1.Font = new Font("Segoe UI", 11F);
            dungeonLabel1.ForeColor = Color.FromArgb(76, 76, 77);
            dungeonLabel1.Location = new Point(10, 485);
            dungeonLabel1.Margin = new Padding(4, 0, 4, 0);
            dungeonLabel1.Name = "dungeonLabel1";
            dungeonLabel1.Size = new Size(52, 25);
            dungeonLabel1.TabIndex = 8;
            dungeonLabel1.Text = "数据";
            // 
            // airCheckBox2_cs_bin
            // 
            airCheckBox2_cs_bin.Checked = false;
            airCheckBox2_cs_bin.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8=";
            airCheckBox2_cs_bin.Font = new Font("Segoe UI", 9F);
            airCheckBox2_cs_bin.Image = null;
            airCheckBox2_cs_bin.Location = new Point(14, 555);
            airCheckBox2_cs_bin.Margin = new Padding(4);
            airCheckBox2_cs_bin.Name = "airCheckBox2_cs_bin";
            airCheckBox2_cs_bin.NoRounding = false;
            airCheckBox2_cs_bin.Size = new Size(118, 17);
            airCheckBox2_cs_bin.TabIndex = 7;
            airCheckBox2_cs_bin.Text = "cs-bin";
            airCheckBox2_cs_bin.Transparent = false;
            airCheckBox2_cs_bin.CheckedChanged += airCheckBox2_CheckedChanged;
            // 
            // airCheckBox1
            // 
            airCheckBox1.Checked = false;
            airCheckBox1.Customization = "7e3t//Ly8v/r6+v/5ubm/+vr6//f39//p6en/zw8PP8=";
            airCheckBox1.Font = new Font("Segoe UI", 9F);
            airCheckBox1.Image = null;
            airCheckBox1.Location = new Point(14, 528);
            airCheckBox1.Margin = new Padding(4);
            airCheckBox1.Name = "airCheckBox1";
            airCheckBox1.NoRounding = false;
            airCheckBox1.Size = new Size(118, 17);
            airCheckBox1.TabIndex = 6;
            airCheckBox1.Text = "Json";
            airCheckBox1.Transparent = false;
            // 
            // aloneButton3
            // 
            aloneButton3.BackColor = Color.Transparent;
            aloneButton3.EnabledCalc = true;
            aloneButton3.Font = new Font("Segoe UI", 9F);
            aloneButton3.ForeColor = Color.FromArgb(124, 133, 142);
            aloneButton3.Location = new Point(575, 261);
            aloneButton3.Margin = new Padding(4);
            aloneButton3.Name = "aloneButton3";
            aloneButton3.Size = new Size(135, 33);
            aloneButton3.TabIndex = 5;
            aloneButton3.Text = "代码导出路径";
            aloneButton3.Click += aloneButton3_Click;
            // 
            // aloneButton2
            // 
            aloneButton2.BackColor = Color.Transparent;
            aloneButton2.EnabledCalc = true;
            aloneButton2.Font = new Font("Segoe UI", 9F);
            aloneButton2.ForeColor = Color.FromArgb(124, 133, 142);
            aloneButton2.Location = new Point(575, 189);
            aloneButton2.Margin = new Padding(4);
            aloneButton2.Name = "aloneButton2";
            aloneButton2.Size = new Size(135, 33);
            aloneButton2.TabIndex = 4;
            aloneButton2.Text = "数据导出路径";
            aloneButton2.Click += aloneButton2_Click;
            // 
            // aloneButton1
            // 
            aloneButton1.BackColor = Color.Transparent;
            aloneButton1.EnabledCalc = true;
            aloneButton1.Font = new Font("Segoe UI", 9F);
            aloneButton1.ForeColor = Color.FromArgb(124, 133, 142);
            aloneButton1.Location = new Point(575, 121);
            aloneButton1.Margin = new Padding(4);
            aloneButton1.Name = "aloneButton1";
            aloneButton1.Size = new Size(135, 33);
            aloneButton1.TabIndex = 3;
            aloneButton1.Text = "luban路径";
            aloneButton1.Click += LubanPathSet_Click;
            // 
            // aloneTextBox3Script
            // 
            aloneTextBox3Script.BackColor = Color.Transparent;
            aloneTextBox3Script.EnabledCalc = true;
            aloneTextBox3Script.Font = new Font("Segoe UI", 9F);
            aloneTextBox3Script.ForeColor = Color.FromArgb(124, 133, 142);
            aloneTextBox3Script.Location = new Point(10, 247);
            aloneTextBox3Script.Margin = new Padding(4);
            aloneTextBox3Script.MaxLength = 32767;
            aloneTextBox3Script.MultiLine = false;
            aloneTextBox3Script.Name = "aloneTextBox3Script";
            aloneTextBox3Script.ReadOnly = false;
            aloneTextBox3Script.Size = new Size(557, 62);
            aloneTextBox3Script.TabIndex = 2;
            aloneTextBox3Script.Text = "aloneTextBox3";
            aloneTextBox3Script.TextAlign = HorizontalAlignment.Left;
            aloneTextBox3Script.UseSystemPasswordChar = false;
            aloneTextBox3Script.Leave += textBox_Leave;
            // 
            // aloneTextBox2Data
            // 
            aloneTextBox2Data.BackColor = Color.Transparent;
            aloneTextBox2Data.EnabledCalc = true;
            aloneTextBox2Data.Font = new Font("Segoe UI", 9F);
            aloneTextBox2Data.ForeColor = Color.FromArgb(124, 133, 142);
            aloneTextBox2Data.Location = new Point(10, 178);
            aloneTextBox2Data.Margin = new Padding(4);
            aloneTextBox2Data.MaxLength = 32767;
            aloneTextBox2Data.MultiLine = false;
            aloneTextBox2Data.Name = "aloneTextBox2Data";
            aloneTextBox2Data.ReadOnly = false;
            aloneTextBox2Data.Size = new Size(557, 62);
            aloneTextBox2Data.TabIndex = 1;
            aloneTextBox2Data.Text = "aloneTextBox2";
            aloneTextBox2Data.TextAlign = HorizontalAlignment.Left;
            aloneTextBox2Data.UseSystemPasswordChar = false;
            aloneTextBox2Data.Leave += textBox_Leave;
            // 
            // aloneTextBox1Luban
            // 
            aloneTextBox1Luban.BackColor = Color.Transparent;
            aloneTextBox1Luban.EnabledCalc = true;
            aloneTextBox1Luban.Font = new Font("Segoe UI", 9F);
            aloneTextBox1Luban.ForeColor = Color.FromArgb(124, 133, 142);
            aloneTextBox1Luban.Location = new Point(10, 108);
            aloneTextBox1Luban.Margin = new Padding(4);
            aloneTextBox1Luban.MaxLength = 32767;
            aloneTextBox1Luban.MultiLine = false;
            aloneTextBox1Luban.Name = "aloneTextBox1Luban";
            aloneTextBox1Luban.ReadOnly = false;
            aloneTextBox1Luban.Size = new Size(557, 62);
            aloneTextBox1Luban.TabIndex = 0;
            aloneTextBox1Luban.Text = "aloneTextBox1";
            aloneTextBox1Luban.TextAlign = HorizontalAlignment.Left;
            aloneTextBox1Luban.UseSystemPasswordChar = false;
            aloneTextBox1Luban.Leave += textBox_Leave;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.White;
            tabPage2.Controls.Add(dungeonRichTextBox1ToolTips);
            tabPage2.Location = new Point(119, 4);
            tabPage2.Margin = new Padding(4);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(4);
            tabPage2.Size = new Size(898, 654);
            tabPage2.TabIndex = 2;
            tabPage2.Text = "工具提示";
            // 
            // dungeonRichTextBox1ToolTips
            // 
            dungeonRichTextBox1ToolTips.AutoWordSelection = false;
            dungeonRichTextBox1ToolTips.BackColor = Color.Transparent;
            dungeonRichTextBox1ToolTips.BorderColor = Color.FromArgb(180, 180, 180);
            dungeonRichTextBox1ToolTips.EdgeColor = Color.White;
            dungeonRichTextBox1ToolTips.Font = new Font("Tahoma", 10F);
            dungeonRichTextBox1ToolTips.ForeColor = Color.FromArgb(76, 76, 76);
            dungeonRichTextBox1ToolTips.Location = new Point(0, 4);
            dungeonRichTextBox1ToolTips.Margin = new Padding(4);
            dungeonRichTextBox1ToolTips.Name = "dungeonRichTextBox1ToolTips";
            dungeonRichTextBox1ToolTips.ReadOnly = false;
            dungeonRichTextBox1ToolTips.Size = new Size(855, 585);
            dungeonRichTextBox1ToolTips.TabIndex = 0;
            dungeonRichTextBox1ToolTips.Text = "1.protobuf,不支持中文路径";
            dungeonRichTextBox1ToolTips.TextBackColor = Color.White;
            dungeonRichTextBox1ToolTips.WordWrap = true;
            // 
            // airForm1
            // 
            airForm1.BackColor = Color.White;
            airForm1.BorderStyle = FormBorderStyle.None;
            airForm1.Controls.Add(airTabPage1);
            airForm1.Customization = "AAAA/1paWv9ycnL/";
            airForm1.Dock = DockStyle.Fill;
            airForm1.Font = new Font("Segoe UI", 9F);
            airForm1.Image = null;
            airForm1.Location = new Point(0, 0);
            airForm1.Margin = new Padding(4);
            airForm1.MinimumSize = new Size(144, 41);
            airForm1.Movable = true;
            airForm1.Name = "airForm1";
            airForm1.NoRounding = false;
            airForm1.Sizable = true;
            airForm1.Size = new Size(1029, 706);
            airForm1.SmartBounds = true;
            airForm1.StartPosition = FormStartPosition.CenterScreen;
            airForm1.TabIndex = 0;
            airForm1.Text = "Excel";
            airForm1.TransparencyKey = Color.Empty;
            airForm1.Transparent = false;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1029, 706);
            Controls.Add(airForm1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4);
            MinimumSize = new Size(144, 41);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            airTabPage1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            parrotToolStrip1.ResumeLayout(false);
            parrotToolStrip1.PerformLayout();
            设置.ResumeLayout(false);
            设置.PerformLayout();
            tabPage2.ResumeLayout(false);
            airForm1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ReaLTaiizor.Controls.AirTabPage airTabPage1;
        private TabPage tabPage1;
        private TabPage 设置;
        private ReaLTaiizor.Controls.AloneButton aloneButton3;
        private ReaLTaiizor.Controls.AloneButton aloneButton2;
        private ReaLTaiizor.Controls.AloneButton aloneButton1;
        private ReaLTaiizor.Controls.AloneTextBox aloneTextBox3Script;
        private ReaLTaiizor.Controls.AloneTextBox aloneTextBox2Data;
        private ReaLTaiizor.Controls.AloneTextBox aloneTextBox1Luban;
        private ReaLTaiizor.Forms.AirForm airForm1;
        private ListView excelListBox;
        private ColumnHeader columnHeader1;
        private ReaLTaiizor.Controls.ParrotToolStrip parrotToolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripProgressBar toolStripProgressBar1;
        private ReaLTaiizor.Controls.AirCheckBox airCheckBox6Protobuf_bin;
        private ReaLTaiizor.Controls.AirCheckBox airCheckBox5;
        private ReaLTaiizor.Controls.AirCheckBox airCheckBox4Protobuf_cs;
        private ReaLTaiizor.Controls.AirCheckBox airCheckBox3bin_cs;
        private ReaLTaiizor.Controls.DungeonLabel dungeonLabel2;
        private ReaLTaiizor.Controls.DungeonLabel dungeonLabel1;
        private ReaLTaiizor.Controls.AirCheckBox airCheckBox2_cs_bin;
        private ReaLTaiizor.Controls.AirCheckBox airCheckBox1;
        private ReaLTaiizor.Controls.AloneButton aloneButton4;
        private ReaLTaiizor.Controls.AloneTextBox aloneTextBox1Excel;
        private ReaLTaiizor.Controls.AloneButton aloneButton5Luban_Config;
        private ReaLTaiizor.Controls.AloneTextBox aloneTextBox1LubanConfig;
        private ReaLTaiizor.Controls.AloneButton aloneButton5ProtoBuf;
        private ReaLTaiizor.Controls.AloneTextBox aloneTextBox1ProtoBufPath;
        private TabPage tabPage2;
        private ReaLTaiizor.Controls.DungeonRichTextBox dungeonRichTextBox1ToolTips;
    }
}
