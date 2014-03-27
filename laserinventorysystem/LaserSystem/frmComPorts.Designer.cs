namespace LaserSystem
{
    partial class frmComPorts
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
            this.lbl_LeftLaserCom = new System.Windows.Forms.Label();
            this.lbl_LeftLaserBaud = new System.Windows.Forms.Label();
            this.txt_LeftLaserBaud = new System.Windows.Forms.TextBox();
            this.lbl_RightLaserBaud = new System.Windows.Forms.Label();
            this.lbl_RightLaserCom = new System.Windows.Forms.Label();
            this.lbl_GPSBaud = new System.Windows.Forms.Label();
            this.lbl_GPSCom = new System.Windows.Forms.Label();
            this.txt_RightLaserBaud = new System.Windows.Forms.TextBox();
            this.txt_GPSBaud = new System.Windows.Forms.TextBox();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.cb_LeftLaserCom = new System.Windows.Forms.ComboBox();
            this.cb_GPSCom = new System.Windows.Forms.ComboBox();
            this.cb_RightLaserCom = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lbl_LeftLaserCom
            // 
            this.lbl_LeftLaserCom.AutoSize = true;
            this.lbl_LeftLaserCom.Location = new System.Drawing.Point(30, 34);
            this.lbl_LeftLaserCom.Name = "lbl_LeftLaserCom";
            this.lbl_LeftLaserCom.Size = new System.Drawing.Size(100, 13);
            this.lbl_LeftLaserCom.TabIndex = 1;
            this.lbl_LeftLaserCom.Text = "Left Laser Com Port";
            // 
            // lbl_LeftLaserBaud
            // 
            this.lbl_LeftLaserBaud.AutoSize = true;
            this.lbl_LeftLaserBaud.Location = new System.Drawing.Point(30, 57);
            this.lbl_LeftLaserBaud.Name = "lbl_LeftLaserBaud";
            this.lbl_LeftLaserBaud.Size = new System.Drawing.Size(132, 13);
            this.lbl_LeftLaserBaud.TabIndex = 3;
            this.lbl_LeftLaserBaud.Text = "Left Laser Com Baud Rate";
            // 
            // txt_LeftLaserBaud
            // 
            this.txt_LeftLaserBaud.Location = new System.Drawing.Point(168, 54);
            this.txt_LeftLaserBaud.Name = "txt_LeftLaserBaud";
            this.txt_LeftLaserBaud.Size = new System.Drawing.Size(100, 20);
            this.txt_LeftLaserBaud.TabIndex = 2;
            // 
            // lbl_RightLaserBaud
            // 
            this.lbl_RightLaserBaud.AutoSize = true;
            this.lbl_RightLaserBaud.Location = new System.Drawing.Point(297, 50);
            this.lbl_RightLaserBaud.Name = "lbl_RightLaserBaud";
            this.lbl_RightLaserBaud.Size = new System.Drawing.Size(139, 13);
            this.lbl_RightLaserBaud.TabIndex = 5;
            this.lbl_RightLaserBaud.Text = "Right Laser Com Baud Rate";
            // 
            // lbl_RightLaserCom
            // 
            this.lbl_RightLaserCom.AutoSize = true;
            this.lbl_RightLaserCom.Location = new System.Drawing.Point(297, 27);
            this.lbl_RightLaserCom.Name = "lbl_RightLaserCom";
            this.lbl_RightLaserCom.Size = new System.Drawing.Size(107, 13);
            this.lbl_RightLaserCom.TabIndex = 4;
            this.lbl_RightLaserCom.Text = "Right Laser Com Port";
            // 
            // lbl_GPSBaud
            // 
            this.lbl_GPSBaud.AutoSize = true;
            this.lbl_GPSBaud.Location = new System.Drawing.Point(30, 122);
            this.lbl_GPSBaud.Name = "lbl_GPSBaud";
            this.lbl_GPSBaud.Size = new System.Drawing.Size(107, 13);
            this.lbl_GPSBaud.TabIndex = 7;
            this.lbl_GPSBaud.Text = "GPS Com Baud Rate";
            // 
            // lbl_GPSCom
            // 
            this.lbl_GPSCom.AutoSize = true;
            this.lbl_GPSCom.Location = new System.Drawing.Point(30, 99);
            this.lbl_GPSCom.Name = "lbl_GPSCom";
            this.lbl_GPSCom.Size = new System.Drawing.Size(75, 13);
            this.lbl_GPSCom.TabIndex = 6;
            this.lbl_GPSCom.Text = "GPS Com Port";
            // 
            // txt_RightLaserBaud
            // 
            this.txt_RightLaserBaud.Location = new System.Drawing.Point(435, 51);
            this.txt_RightLaserBaud.Name = "txt_RightLaserBaud";
            this.txt_RightLaserBaud.Size = new System.Drawing.Size(100, 20);
            this.txt_RightLaserBaud.TabIndex = 9;
            // 
            // txt_GPSBaud
            // 
            this.txt_GPSBaud.Location = new System.Drawing.Point(168, 123);
            this.txt_GPSBaud.Name = "txt_GPSBaud";
            this.txt_GPSBaud.Size = new System.Drawing.Size(100, 20);
            this.txt_GPSBaud.TabIndex = 11;
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(309, 99);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(75, 23);
            this.btn_Save.TabIndex = 12;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(445, 99);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 13;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // cb_LeftLaserCom
            // 
            this.cb_LeftLaserCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_LeftLaserCom.FormattingEnabled = true;
            this.cb_LeftLaserCom.Location = new System.Drawing.Point(168, 27);
            this.cb_LeftLaserCom.Name = "cb_LeftLaserCom";
            this.cb_LeftLaserCom.Size = new System.Drawing.Size(121, 21);
            this.cb_LeftLaserCom.TabIndex = 14;
            this.cb_LeftLaserCom.DropDown += new System.EventHandler(this.updateComboBoxComPortsList);
            // 
            // cb_GPSCom
            // 
            this.cb_GPSCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_GPSCom.FormattingEnabled = true;
            this.cb_GPSCom.Location = new System.Drawing.Point(168, 91);
            this.cb_GPSCom.Name = "cb_GPSCom";
            this.cb_GPSCom.Size = new System.Drawing.Size(121, 21);
            this.cb_GPSCom.TabIndex = 15;
            this.cb_GPSCom.DropDown += new System.EventHandler(this.updateComboBoxComPortsList);
            // 
            // cb_RightLaserCom
            // 
            this.cb_RightLaserCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_RightLaserCom.FormattingEnabled = true;
            this.cb_RightLaserCom.Location = new System.Drawing.Point(435, 24);
            this.cb_RightLaserCom.Name = "cb_RightLaserCom";
            this.cb_RightLaserCom.Size = new System.Drawing.Size(121, 21);
            this.cb_RightLaserCom.TabIndex = 16;
            this.cb_RightLaserCom.DropDown += new System.EventHandler(this.updateComboBoxComPortsList);
            // 
            // frmComPorts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 175);
            this.Controls.Add(this.cb_RightLaserCom);
            this.Controls.Add(this.cb_GPSCom);
            this.Controls.Add(this.cb_LeftLaserCom);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.txt_GPSBaud);
            this.Controls.Add(this.txt_RightLaserBaud);
            this.Controls.Add(this.lbl_GPSBaud);
            this.Controls.Add(this.lbl_GPSCom);
            this.Controls.Add(this.lbl_RightLaserBaud);
            this.Controls.Add(this.lbl_RightLaserCom);
            this.Controls.Add(this.lbl_LeftLaserBaud);
            this.Controls.Add(this.txt_LeftLaserBaud);
            this.Controls.Add(this.lbl_LeftLaserCom);
            this.Name = "frmComPorts";
            this.Text = "frmComPorts";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_LeftLaserCom;
        private System.Windows.Forms.Label lbl_LeftLaserBaud;
        private System.Windows.Forms.TextBox txt_LeftLaserBaud;
        private System.Windows.Forms.Label lbl_RightLaserBaud;
        private System.Windows.Forms.Label lbl_RightLaserCom;
        private System.Windows.Forms.Label lbl_GPSBaud;
        private System.Windows.Forms.Label lbl_GPSCom;
        private System.Windows.Forms.TextBox txt_RightLaserBaud;
        private System.Windows.Forms.TextBox txt_GPSBaud;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.ComboBox cb_LeftLaserCom;
        private System.Windows.Forms.ComboBox cb_GPSCom;
        private System.Windows.Forms.ComboBox cb_RightLaserCom;
    }
}