namespace LaserSystem
{
    partial class ProcessScans
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
            this.textBox_GPSFile = new System.Windows.Forms.TextBox();
            this.textBox_LeftLaserFile = new System.Windows.Forms.TextBox();
            this.textBox_RightLaserFile = new System.Windows.Forms.TextBox();
            this.lbl_GPSFile = new System.Windows.Forms.Label();
            this.lbl_LeftLaserFile = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_RightLaserFile = new System.Windows.Forms.Label();
            this.button_GPSDialog = new System.Windows.Forms.Button();
            this.textBox_GDB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_GDB = new System.Windows.Forms.Button();
            this.button_ProcessScans = new System.Windows.Forms.Button();
            this.bgw_ProcessScans = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // textBox_GPSFile
            // 
            this.textBox_GPSFile.Location = new System.Drawing.Point(115, 16);
            this.textBox_GPSFile.Name = "textBox_GPSFile";
            this.textBox_GPSFile.Size = new System.Drawing.Size(211, 20);
            this.textBox_GPSFile.TabIndex = 0;
            // 
            // textBox_LeftLaserFile
            // 
            this.textBox_LeftLaserFile.Enabled = false;
            this.textBox_LeftLaserFile.Location = new System.Drawing.Point(115, 43);
            this.textBox_LeftLaserFile.Name = "textBox_LeftLaserFile";
            this.textBox_LeftLaserFile.Size = new System.Drawing.Size(211, 20);
            this.textBox_LeftLaserFile.TabIndex = 1;
            // 
            // textBox_RightLaserFile
            // 
            this.textBox_RightLaserFile.Enabled = false;
            this.textBox_RightLaserFile.Location = new System.Drawing.Point(115, 69);
            this.textBox_RightLaserFile.Name = "textBox_RightLaserFile";
            this.textBox_RightLaserFile.Size = new System.Drawing.Size(211, 20);
            this.textBox_RightLaserFile.TabIndex = 2;
            // 
            // lbl_GPSFile
            // 
            this.lbl_GPSFile.AutoSize = true;
            this.lbl_GPSFile.Location = new System.Drawing.Point(37, 19);
            this.lbl_GPSFile.Name = "lbl_GPSFile";
            this.lbl_GPSFile.Size = new System.Drawing.Size(77, 13);
            this.lbl_GPSFile.TabIndex = 3;
            this.lbl_GPSFile.Text = "GPS Readings";
            // 
            // lbl_LeftLaserFile
            // 
            this.lbl_LeftLaserFile.AutoSize = true;
            this.lbl_LeftLaserFile.Location = new System.Drawing.Point(22, 46);
            this.lbl_LeftLaserFile.Name = "lbl_LeftLaserFile";
            this.lbl_LeftLaserFile.Size = new System.Drawing.Size(87, 13);
            this.lbl_LeftLaserFile.TabIndex = 4;
            this.lbl_LeftLaserFile.Text = "Left Laser Scans";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 337);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "label3";
            // 
            // lbl_RightLaserFile
            // 
            this.lbl_RightLaserFile.AutoSize = true;
            this.lbl_RightLaserFile.Location = new System.Drawing.Point(15, 72);
            this.lbl_RightLaserFile.Name = "lbl_RightLaserFile";
            this.lbl_RightLaserFile.Size = new System.Drawing.Size(94, 13);
            this.lbl_RightLaserFile.TabIndex = 6;
            this.lbl_RightLaserFile.Text = "Right Laser Scans";
            // 
            // button_GPSDialog
            // 
            this.button_GPSDialog.Location = new System.Drawing.Point(332, 13);
            this.button_GPSDialog.Name = "button_GPSDialog";
            this.button_GPSDialog.Size = new System.Drawing.Size(92, 23);
            this.button_GPSDialog.TabIndex = 7;
            this.button_GPSDialog.Text = "Select File...";
            this.button_GPSDialog.UseVisualStyleBackColor = true;
            this.button_GPSDialog.Click += new System.EventHandler(this.button_GPSDialog_Click);
            // 
            // textBox_GDB
            // 
            this.textBox_GDB.Location = new System.Drawing.Point(115, 95);
            this.textBox_GDB.Name = "textBox_GDB";
            this.textBox_GDB.Size = new System.Drawing.Size(211, 20);
            this.textBox_GDB.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Target Geodatabase";
            // 
            // button_GDB
            // 
            this.button_GDB.Location = new System.Drawing.Point(332, 92);
            this.button_GDB.Name = "button_GDB";
            this.button_GDB.Size = new System.Drawing.Size(92, 23);
            this.button_GDB.TabIndex = 10;
            this.button_GDB.Text = "Select GDB...";
            this.button_GDB.UseVisualStyleBackColor = true;
            this.button_GDB.Click += new System.EventHandler(this.button_GDB_Click);
            // 
            // button_ProcessScans
            // 
            this.button_ProcessScans.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ProcessScans.Location = new System.Drawing.Point(12, 134);
            this.button_ProcessScans.Name = "button_ProcessScans";
            this.button_ProcessScans.Size = new System.Drawing.Size(412, 47);
            this.button_ProcessScans.TabIndex = 11;
            this.button_ProcessScans.Text = "Process Scans";
            this.button_ProcessScans.UseVisualStyleBackColor = true;
            this.button_ProcessScans.Click += new System.EventHandler(this.button_ProcessScans_Click);
            // 
            // bgw_ProcessScans
            // 
            this.bgw_ProcessScans.WorkerReportsProgress = true;
            this.bgw_ProcessScans.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgw_ProcessScans_DoWork);
            this.bgw_ProcessScans.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgw_ProcessScans_ProgressChanged);
            this.bgw_ProcessScans.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgw_ProcessScans_RunWorkerCompleted);
            // 
            // ProcessScans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 203);
            this.Controls.Add(this.button_ProcessScans);
            this.Controls.Add(this.button_GDB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_GDB);
            this.Controls.Add(this.button_GPSDialog);
            this.Controls.Add(this.lbl_RightLaserFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbl_LeftLaserFile);
            this.Controls.Add(this.lbl_GPSFile);
            this.Controls.Add(this.textBox_RightLaserFile);
            this.Controls.Add(this.textBox_LeftLaserFile);
            this.Controls.Add(this.textBox_GPSFile);
            this.Name = "ProcessScans";
            this.Text = "Process Scans";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_GPSFile;
        private System.Windows.Forms.TextBox textBox_LeftLaserFile;
        private System.Windows.Forms.TextBox textBox_RightLaserFile;
        private System.Windows.Forms.Label lbl_GPSFile;
        private System.Windows.Forms.Label lbl_LeftLaserFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_RightLaserFile;
        private System.Windows.Forms.Button button_GPSDialog;
        private System.Windows.Forms.TextBox textBox_GDB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_GDB;
        private System.Windows.Forms.Button button_ProcessScans;
        private System.ComponentModel.BackgroundWorker bgw_ProcessScans;
    }
}

