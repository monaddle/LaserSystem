namespace LaserSystem
{
    partial class frm_main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtBox_height = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBox_rowDistance = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_samplingDistance = new System.Windows.Forms.ComboBox();
            this.lbl_PointDistance = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_MinHeight = new System.Windows.Forms.Label();
            this.txtBox_OutputFilePath = new System.Windows.Forms.TextBox();
            this.btn_saveSettings = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbl_BottomRightACS = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lbl_BottomLeftACS = new System.Windows.Forms.Label();
            this.lbl_TopLeftACS = new System.Windows.Forms.Label();
            this.panel_TopRightACS = new System.Windows.Forms.Panel();
            this.panel_BottomLeftACS = new System.Windows.Forms.Panel();
            this.panel_BottomRightACS = new System.Windows.Forms.Panel();
            this.panel_topLeftACS = new System.Windows.Forms.Panel();
            this.chkbox_rightLaser = new System.Windows.Forms.CheckBox();
            this.chkbox_leftLaser = new System.Windows.Forms.CheckBox();
            this.panel_rightLaser = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panel_leftLaser = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel_GPS = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setCOMPortsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usePolygonConstraintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_StartScanning = new System.Windows.Forms.Button();
            this.btn_StopScanning = new System.Windows.Forms.Button();
            this.ScanRunnerWorker = new System.ComponentModel.BackgroundWorker();
            this.txt_laserOffset = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBox_height
            // 
            this.txtBox_height.BackColor = System.Drawing.SystemColors.Window;
            this.txtBox_height.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtBox_height.Location = new System.Drawing.Point(169, 46);
            this.txtBox_height.Name = "txtBox_height";
            this.txtBox_height.Size = new System.Drawing.Size(90, 29);
            this.txtBox_height.TabIndex = 0;
            this.txtBox_height.TextChanged += new System.EventHandler(this.txtBox_height_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label1.Location = new System.Drawing.Point(10, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Laser Height (ft)";
            // 
            // txtBox_rowDistance
            // 
            this.txtBox_rowDistance.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtBox_rowDistance.Location = new System.Drawing.Point(169, 97);
            this.txtBox_rowDistance.Name = "txtBox_rowDistance";
            this.txtBox_rowDistance.Size = new System.Drawing.Size(90, 29);
            this.txtBox_rowDistance.TabIndex = 2;
            this.txtBox_rowDistance.TextChanged += new System.EventHandler(this.txtBox_rowDistance_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label2.Location = new System.Drawing.Point(10, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Row Distance (ft)";
            // 
            // cb_samplingDistance
            // 
            this.cb_samplingDistance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_samplingDistance.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.cb_samplingDistance.FormattingEnabled = true;
            this.cb_samplingDistance.Items.AddRange(new object[] {
            "0.5",
            "1.0",
            "2.0"});
            this.cb_samplingDistance.Location = new System.Drawing.Point(441, 42);
            this.cb_samplingDistance.Name = "cb_samplingDistance";
            this.cb_samplingDistance.Size = new System.Drawing.Size(90, 32);
            this.cb_samplingDistance.TabIndex = 4;
            // 
            // lbl_PointDistance
            // 
            this.lbl_PointDistance.AutoSize = true;
            this.lbl_PointDistance.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lbl_PointDistance.Location = new System.Drawing.Point(283, 47);
            this.lbl_PointDistance.Name = "lbl_PointDistance";
            this.lbl_PointDistance.Size = new System.Drawing.Size(152, 24);
            this.lbl_PointDistance.TabIndex = 5;
            this.lbl_PointDistance.Text = "Sample Width (ft)";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txt_laserOffset);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.lbl_MinHeight);
            this.panel1.Controls.Add(this.txtBox_OutputFilePath);
            this.panel1.Controls.Add(this.btn_saveSettings);
            this.panel1.Controls.Add(this.cb_samplingDistance);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtBox_height);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lbl_PointDistance);
            this.panel1.Controls.Add(this.txtBox_rowDistance);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(25, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(553, 304);
            this.panel1.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(10, 147);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 24);
            this.label8.TabIndex = 14;
            this.label8.Text = "File Tag";
            // 
            // lbl_MinHeight
            // 
            this.lbl_MinHeight.AutoSize = true;
            this.lbl_MinHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lbl_MinHeight.Location = new System.Drawing.Point(293, 99);
            this.lbl_MinHeight.Name = "lbl_MinHeight";
            this.lbl_MinHeight.Size = new System.Drawing.Size(133, 24);
            this.lbl_MinHeight.TabIndex = 12;
            this.lbl_MinHeight.Text = "Laser Offset (ft)";
            // 
            // txtBox_OutputFilePath
            // 
            this.txtBox_OutputFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.txtBox_OutputFilePath.Location = new System.Drawing.Point(169, 147);
            this.txtBox_OutputFilePath.Name = "txtBox_OutputFilePath";
            this.txtBox_OutputFilePath.Size = new System.Drawing.Size(362, 29);
            this.txtBox_OutputFilePath.TabIndex = 10;
            this.txtBox_OutputFilePath.Text = "output";
            // 
            // btn_saveSettings
            // 
            this.btn_saveSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_saveSettings.Location = new System.Drawing.Point(14, 199);
            this.btn_saveSettings.Name = "btn_saveSettings";
            this.btn_saveSettings.Size = new System.Drawing.Size(517, 80);
            this.btn_saveSettings.TabIndex = 9;
            this.btn_saveSettings.Text = "Save Current Settings";
            this.btn_saveSettings.UseVisualStyleBackColor = true;
            this.btn_saveSettings.Click += new System.EventHandler(this.btn_saveSettings_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(20, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(174, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Data Collection Settings";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lbl_BottomRightACS);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.lbl_BottomLeftACS);
            this.panel2.Controls.Add(this.lbl_TopLeftACS);
            this.panel2.Controls.Add(this.panel_TopRightACS);
            this.panel2.Controls.Add(this.panel_BottomLeftACS);
            this.panel2.Controls.Add(this.panel_BottomRightACS);
            this.panel2.Controls.Add(this.panel_topLeftACS);
            this.panel2.Controls.Add(this.chkbox_rightLaser);
            this.panel2.Controls.Add(this.chkbox_leftLaser);
            this.panel2.Controls.Add(this.panel_rightLaser);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.panel_leftLaser);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.panel_GPS);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(649, 36);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(297, 304);
            this.panel2.TabIndex = 8;
            // 
            // lbl_BottomRightACS
            // 
            this.lbl_BottomRightACS.AutoSize = true;
            this.lbl_BottomRightACS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_BottomRightACS.Location = new System.Drawing.Point(68, 214);
            this.lbl_BottomRightACS.Name = "lbl_BottomRightACS";
            this.lbl_BottomRightACS.Size = new System.Drawing.Size(114, 16);
            this.lbl_BottomRightACS.TabIndex = 12;
            this.lbl_BottomRightACS.Text = "Bottom Right ACS";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(67, 185);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 16);
            this.label11.TabIndex = 11;
            this.label11.Text = "Top Right ACS";
            // 
            // lbl_BottomLeftACS
            // 
            this.lbl_BottomLeftACS.AutoSize = true;
            this.lbl_BottomLeftACS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_BottomLeftACS.Location = new System.Drawing.Point(67, 104);
            this.lbl_BottomLeftACS.Name = "lbl_BottomLeftACS";
            this.lbl_BottomLeftACS.Size = new System.Drawing.Size(104, 16);
            this.lbl_BottomLeftACS.TabIndex = 10;
            this.lbl_BottomLeftACS.Text = "Bottom Left ACS";
            // 
            // lbl_TopLeftACS
            // 
            this.lbl_TopLeftACS.AutoSize = true;
            this.lbl_TopLeftACS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TopLeftACS.Location = new System.Drawing.Point(69, 70);
            this.lbl_TopLeftACS.Name = "lbl_TopLeftACS";
            this.lbl_TopLeftACS.Size = new System.Drawing.Size(87, 16);
            this.lbl_TopLeftACS.TabIndex = 9;
            this.lbl_TopLeftACS.Text = "Top Left ACS";
            // 
            // panel_TopRightACS
            // 
            this.panel_TopRightACS.BackColor = System.Drawing.Color.Red;
            this.panel_TopRightACS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_TopRightACS.Location = new System.Drawing.Point(21, 178);
            this.panel_TopRightACS.Name = "panel_TopRightACS";
            this.panel_TopRightACS.Size = new System.Drawing.Size(24, 24);
            this.panel_TopRightACS.TabIndex = 4;
            // 
            // panel_BottomLeftACS
            // 
            this.panel_BottomLeftACS.BackColor = System.Drawing.Color.Red;
            this.panel_BottomLeftACS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_BottomLeftACS.Location = new System.Drawing.Point(21, 100);
            this.panel_BottomLeftACS.Name = "panel_BottomLeftACS";
            this.panel_BottomLeftACS.Size = new System.Drawing.Size(24, 24);
            this.panel_BottomLeftACS.TabIndex = 4;
            // 
            // panel_BottomRightACS
            // 
            this.panel_BottomRightACS.BackColor = System.Drawing.Color.Red;
            this.panel_BottomRightACS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_BottomRightACS.Location = new System.Drawing.Point(21, 208);
            this.panel_BottomRightACS.Name = "panel_BottomRightACS";
            this.panel_BottomRightACS.Size = new System.Drawing.Size(24, 24);
            this.panel_BottomRightACS.TabIndex = 4;
            // 
            // panel_topLeftACS
            // 
            this.panel_topLeftACS.BackColor = System.Drawing.Color.Red;
            this.panel_topLeftACS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_topLeftACS.Location = new System.Drawing.Point(21, 70);
            this.panel_topLeftACS.Name = "panel_topLeftACS";
            this.panel_topLeftACS.Size = new System.Drawing.Size(24, 24);
            this.panel_topLeftACS.TabIndex = 3;
            // 
            // chkbox_rightLaser
            // 
            this.chkbox_rightLaser.AutoSize = true;
            this.chkbox_rightLaser.Checked = true;
            this.chkbox_rightLaser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbox_rightLaser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbox_rightLaser.Location = new System.Drawing.Point(167, 153);
            this.chkbox_rightLaser.Name = "chkbox_rightLaser";
            this.chkbox_rightLaser.Size = new System.Drawing.Size(64, 20);
            this.chkbox_rightLaser.TabIndex = 8;
            this.chkbox_rightLaser.Text = "Active";
            this.chkbox_rightLaser.UseVisualStyleBackColor = true;
            // 
            // chkbox_leftLaser
            // 
            this.chkbox_leftLaser.AutoSize = true;
            this.chkbox_leftLaser.Checked = true;
            this.chkbox_leftLaser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbox_leftLaser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbox_leftLaser.Location = new System.Drawing.Point(166, 46);
            this.chkbox_leftLaser.Name = "chkbox_leftLaser";
            this.chkbox_leftLaser.Size = new System.Drawing.Size(64, 20);
            this.chkbox_leftLaser.TabIndex = 7;
            this.chkbox_leftLaser.Text = "Active";
            this.chkbox_leftLaser.UseVisualStyleBackColor = true;
            this.chkbox_leftLaser.CheckedChanged += new System.EventHandler(this.chkbox_leftLaser_CheckedChanged);
            // 
            // panel_rightLaser
            // 
            this.panel_rightLaser.BackColor = System.Drawing.Color.Red;
            this.panel_rightLaser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_rightLaser.Location = new System.Drawing.Point(21, 148);
            this.panel_rightLaser.Name = "panel_rightLaser";
            this.panel_rightLaser.Size = new System.Drawing.Size(24, 24);
            this.panel_rightLaser.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(67, 154);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 16);
            this.label7.TabIndex = 5;
            this.label7.Text = "Right Laser";
            // 
            // panel_leftLaser
            // 
            this.panel_leftLaser.BackColor = System.Drawing.Color.Red;
            this.panel_leftLaser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_leftLaser.Location = new System.Drawing.Point(21, 40);
            this.panel_leftLaser.Name = "panel_leftLaser";
            this.panel_leftLaser.Size = new System.Drawing.Size(24, 24);
            this.panel_leftLaser.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(68, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 16);
            this.label6.TabIndex = 3;
            this.label6.Text = "Left Laser";
            // 
            // panel_GPS
            // 
            this.panel_GPS.BackColor = System.Drawing.Color.Red;
            this.panel_GPS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_GPS.Location = new System.Drawing.Point(21, 255);
            this.panel_GPS.Name = "panel_GPS";
            this.panel_GPS.Size = new System.Drawing.Size(24, 24);
            this.panel_GPS.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(68, 255);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "GPS";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "System Status";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 33);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(53, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(114, 30);
            this.quitToolStripMenuItem.Text = "Exit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setCOMPortsToolStripMenuItem,
            this.saveDataToolStripMenuItem,
            this.usePolygonConstraintToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(91, 29);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // setCOMPortsToolStripMenuItem
            // 
            this.setCOMPortsToolStripMenuItem.Name = "setCOMPortsToolStripMenuItem";
            this.setCOMPortsToolStripMenuItem.Size = new System.Drawing.Size(281, 30);
            this.setCOMPortsToolStripMenuItem.Text = "Set COM Ports";
            this.setCOMPortsToolStripMenuItem.Click += new System.EventHandler(this.setCOMPortsToolStripMenuItem_Click);
            // 
            // saveDataToolStripMenuItem
            // 
            this.saveDataToolStripMenuItem.CheckOnClick = true;
            this.saveDataToolStripMenuItem.Name = "saveDataToolStripMenuItem";
            this.saveDataToolStripMenuItem.Size = new System.Drawing.Size(281, 30);
            this.saveDataToolStripMenuItem.Text = "Save Data";
            // 
            // usePolygonConstraintToolStripMenuItem
            // 
            this.usePolygonConstraintToolStripMenuItem.Name = "usePolygonConstraintToolStripMenuItem";
            this.usePolygonConstraintToolStripMenuItem.Size = new System.Drawing.Size(281, 30);
            this.usePolygonConstraintToolStripMenuItem.Text = "Use Polygon Constraint";
            this.usePolygonConstraintToolStripMenuItem.Click += new System.EventHandler(this.usePolygonConstraintToolStripMenuItem_Click);
            // 
            // btn_StartScanning
            // 
            this.btn_StartScanning.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_StartScanning.Location = new System.Drawing.Point(29, 348);
            this.btn_StartScanning.Name = "btn_StartScanning";
            this.btn_StartScanning.Size = new System.Drawing.Size(450, 159);
            this.btn_StartScanning.TabIndex = 10;
            this.btn_StartScanning.Text = "Start Scanning";
            this.btn_StartScanning.UseVisualStyleBackColor = true;
            this.btn_StartScanning.Click += new System.EventHandler(this.btn_StartScanning_Click);
            // 
            // btn_StopScanning
            // 
            this.btn_StopScanning.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_StopScanning.Location = new System.Drawing.Point(505, 348);
            this.btn_StopScanning.Name = "btn_StopScanning";
            this.btn_StopScanning.Size = new System.Drawing.Size(444, 159);
            this.btn_StopScanning.TabIndex = 11;
            this.btn_StopScanning.Text = "Stop Scanning";
            this.btn_StopScanning.UseVisualStyleBackColor = true;
            this.btn_StopScanning.Click += new System.EventHandler(this.btn_StopScanning_Click);
            // 
            // ScanRunnerWorker
            // 
            this.ScanRunnerWorker.WorkerReportsProgress = true;
            this.ScanRunnerWorker.WorkerSupportsCancellation = true;
            this.ScanRunnerWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ScanRunnerWorker_DoWork);
            this.ScanRunnerWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ScanRunnerWorker_RunWorkerCompleted);
            // 
            // txt_laserOffset
            // 
            this.txt_laserOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_laserOffset.Location = new System.Drawing.Point(441, 99);
            this.txt_laserOffset.Name = "txt_laserOffset";
            this.txt_laserOffset.Size = new System.Drawing.Size(90, 29);
            this.txt_laserOffset.TabIndex = 15;
            // 
            // frm_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 562);
            this.Controls.Add(this.btn_StopScanning);
            this.Controls.Add(this.btn_StartScanning);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frm_main";
            this.Text = "Folear Inventory System";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_main_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBox_height;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBox_rowDistance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_samplingDistance;
        private System.Windows.Forms.Label lbl_PointDistance;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkbox_rightLaser;
        private System.Windows.Forms.CheckBox chkbox_leftLaser;
        private System.Windows.Forms.Panel panel_rightLaser;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel_leftLaser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel_GPS;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_saveSettings;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox txtBox_OutputFilePath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setCOMPortsToolStripMenuItem;
        private System.Windows.Forms.Button btn_StopScanning;
        private System.Windows.Forms.Button btn_StartScanning;
        private System.ComponentModel.BackgroundWorker ScanRunnerWorker;
        private System.Windows.Forms.ToolStripMenuItem saveDataToolStripMenuItem;
        private System.Windows.Forms.Label lbl_MinHeight;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel_topLeftACS;
        private System.Windows.Forms.Panel panel_TopRightACS;
        private System.Windows.Forms.Panel panel_BottomLeftACS;
        private System.Windows.Forms.Panel panel_BottomRightACS;
        private System.Windows.Forms.Label lbl_BottomRightACS;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbl_BottomLeftACS;
        private System.Windows.Forms.Label lbl_TopLeftACS;
        private System.Windows.Forms.ToolStripMenuItem usePolygonConstraintToolStripMenuItem;
        private System.Windows.Forms.TextBox txt_laserOffset;
    }
}

