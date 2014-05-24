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
            this.cb_TopRightACSCom = new System.Windows.Forms.ComboBox();
            this.cb_TopLeftACSCom = new System.Windows.Forms.ComboBox();
            this.txt_TopRightACSBaud = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_TopLeftACSBaud = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_BottomRightACSCom = new System.Windows.Forms.ComboBox();
            this.cb_BottomLeftACSCom = new System.Windows.Forms.ComboBox();
            this.txt_BottomRightACSBaud = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_BottomLeftACSBaud = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_LeftLaserCom
            // 
            this.lbl_LeftLaserCom.AutoSize = true;
            this.lbl_LeftLaserCom.Location = new System.Drawing.Point(39, 155);
            this.lbl_LeftLaserCom.Name = "lbl_LeftLaserCom";
            this.lbl_LeftLaserCom.Size = new System.Drawing.Size(100, 13);
            this.lbl_LeftLaserCom.TabIndex = 1;
            this.lbl_LeftLaserCom.Text = "Left Laser Com Port";
            // 
            // lbl_LeftLaserBaud
            // 
            this.lbl_LeftLaserBaud.AutoSize = true;
            this.lbl_LeftLaserBaud.Location = new System.Drawing.Point(81, 183);
            this.lbl_LeftLaserBaud.Name = "lbl_LeftLaserBaud";
            this.lbl_LeftLaserBaud.Size = new System.Drawing.Size(58, 13);
            this.lbl_LeftLaserBaud.TabIndex = 3;
            this.lbl_LeftLaserBaud.Text = "Baud Rate";
            // 
            // txt_LeftLaserBaud
            // 
            this.txt_LeftLaserBaud.Location = new System.Drawing.Point(160, 179);
            this.txt_LeftLaserBaud.Name = "txt_LeftLaserBaud";
            this.txt_LeftLaserBaud.Size = new System.Drawing.Size(100, 20);
            this.txt_LeftLaserBaud.TabIndex = 2;
            // 
            // lbl_RightLaserBaud
            // 
            this.lbl_RightLaserBaud.AutoSize = true;
            this.lbl_RightLaserBaud.Location = new System.Drawing.Point(360, 176);
            this.lbl_RightLaserBaud.Name = "lbl_RightLaserBaud";
            this.lbl_RightLaserBaud.Size = new System.Drawing.Size(61, 13);
            this.lbl_RightLaserBaud.TabIndex = 5;
            this.lbl_RightLaserBaud.Text = " Baud Rate";
            // 
            // lbl_RightLaserCom
            // 
            this.lbl_RightLaserCom.AutoSize = true;
            this.lbl_RightLaserCom.Location = new System.Drawing.Point(314, 152);
            this.lbl_RightLaserCom.Name = "lbl_RightLaserCom";
            this.lbl_RightLaserCom.Size = new System.Drawing.Size(107, 13);
            this.lbl_RightLaserCom.TabIndex = 4;
            this.lbl_RightLaserCom.Text = "Right Laser Com Port";
            // 
            // lbl_GPSBaud
            // 
            this.lbl_GPSBaud.AutoSize = true;
            this.lbl_GPSBaud.Location = new System.Drawing.Point(81, 248);
            this.lbl_GPSBaud.Name = "lbl_GPSBaud";
            this.lbl_GPSBaud.Size = new System.Drawing.Size(58, 13);
            this.lbl_GPSBaud.TabIndex = 7;
            this.lbl_GPSBaud.Text = "Baud Rate";
            // 
            // lbl_GPSCom
            // 
            this.lbl_GPSCom.AutoSize = true;
            this.lbl_GPSCom.Location = new System.Drawing.Point(64, 224);
            this.lbl_GPSCom.Name = "lbl_GPSCom";
            this.lbl_GPSCom.Size = new System.Drawing.Size(75, 13);
            this.lbl_GPSCom.TabIndex = 6;
            this.lbl_GPSCom.Text = "GPS Com Port";
            // 
            // txt_RightLaserBaud
            // 
            this.txt_RightLaserBaud.Location = new System.Drawing.Point(427, 176);
            this.txt_RightLaserBaud.Name = "txt_RightLaserBaud";
            this.txt_RightLaserBaud.Size = new System.Drawing.Size(100, 20);
            this.txt_RightLaserBaud.TabIndex = 9;
            // 
            // txt_GPSBaud
            // 
            this.txt_GPSBaud.Location = new System.Drawing.Point(160, 248);
            this.txt_GPSBaud.Name = "txt_GPSBaud";
            this.txt_GPSBaud.Size = new System.Drawing.Size(100, 20);
            this.txt_GPSBaud.TabIndex = 11;
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(324, 329);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(75, 23);
            this.btn_Save.TabIndex = 12;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(460, 329);
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
            this.cb_LeftLaserCom.Location = new System.Drawing.Point(160, 152);
            this.cb_LeftLaserCom.Name = "cb_LeftLaserCom";
            this.cb_LeftLaserCom.Size = new System.Drawing.Size(121, 21);
            this.cb_LeftLaserCom.TabIndex = 14;
            this.cb_LeftLaserCom.DropDown += new System.EventHandler(this.updateComboBoxComPortsList);
            // 
            // cb_GPSCom
            // 
            this.cb_GPSCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_GPSCom.FormattingEnabled = true;
            this.cb_GPSCom.Location = new System.Drawing.Point(160, 216);
            this.cb_GPSCom.Name = "cb_GPSCom";
            this.cb_GPSCom.Size = new System.Drawing.Size(121, 21);
            this.cb_GPSCom.TabIndex = 15;
            this.cb_GPSCom.DropDown += new System.EventHandler(this.updateComboBoxComPortsList);
            // 
            // cb_RightLaserCom
            // 
            this.cb_RightLaserCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_RightLaserCom.FormattingEnabled = true;
            this.cb_RightLaserCom.Location = new System.Drawing.Point(427, 149);
            this.cb_RightLaserCom.Name = "cb_RightLaserCom";
            this.cb_RightLaserCom.Size = new System.Drawing.Size(121, 21);
            this.cb_RightLaserCom.TabIndex = 16;
            this.cb_RightLaserCom.DropDown += new System.EventHandler(this.updateComboBoxComPortsList);
            // 
            // cb_TopRightACSCom
            // 
            this.cb_TopRightACSCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_TopRightACSCom.FormattingEnabled = true;
            this.cb_TopRightACSCom.Location = new System.Drawing.Point(427, 19);
            this.cb_TopRightACSCom.Name = "cb_TopRightACSCom";
            this.cb_TopRightACSCom.Size = new System.Drawing.Size(121, 21);
            this.cb_TopRightACSCom.TabIndex = 24;
            // 
            // cb_TopLeftACSCom
            // 
            this.cb_TopLeftACSCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_TopLeftACSCom.FormattingEnabled = true;
            this.cb_TopLeftACSCom.Location = new System.Drawing.Point(160, 22);
            this.cb_TopLeftACSCom.Name = "cb_TopLeftACSCom";
            this.cb_TopLeftACSCom.Size = new System.Drawing.Size(121, 21);
            this.cb_TopLeftACSCom.TabIndex = 23;
            // 
            // txt_TopRightACSBaud
            // 
            this.txt_TopRightACSBaud.Location = new System.Drawing.Point(427, 46);
            this.txt_TopRightACSBaud.Name = "txt_TopRightACSBaud";
            this.txt_TopRightACSBaud.Size = new System.Drawing.Size(100, 20);
            this.txt_TopRightACSBaud.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(355, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Baud Rate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(289, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Top Right ACS Com Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(81, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Baud Rate";
            // 
            // txt_TopLeftACSBaud
            // 
            this.txt_TopLeftACSBaud.Location = new System.Drawing.Point(160, 49);
            this.txt_TopLeftACSBaud.Name = "txt_TopLeftACSBaud";
            this.txt_TopLeftACSBaud.Size = new System.Drawing.Size(100, 20);
            this.txt_TopLeftACSBaud.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Top Left ACS Com Port";
            // 
            // cb_BottomRightACSCom
            // 
            this.cb_BottomRightACSCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_BottomRightACSCom.FormattingEnabled = true;
            this.cb_BottomRightACSCom.Location = new System.Drawing.Point(427, 82);
            this.cb_BottomRightACSCom.Name = "cb_BottomRightACSCom";
            this.cb_BottomRightACSCom.Size = new System.Drawing.Size(121, 21);
            this.cb_BottomRightACSCom.TabIndex = 32;
            // 
            // cb_BottomLeftACSCom
            // 
            this.cb_BottomLeftACSCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_BottomLeftACSCom.FormattingEnabled = true;
            this.cb_BottomLeftACSCom.Location = new System.Drawing.Point(160, 85);
            this.cb_BottomLeftACSCom.Name = "cb_BottomLeftACSCom";
            this.cb_BottomLeftACSCom.Size = new System.Drawing.Size(121, 21);
            this.cb_BottomLeftACSCom.TabIndex = 31;
            // 
            // txt_BottomRightACSBaud
            // 
            this.txt_BottomRightACSBaud.Location = new System.Drawing.Point(427, 109);
            this.txt_BottomRightACSBaud.Name = "txt_BottomRightACSBaud";
            this.txt_BottomRightACSBaud.Size = new System.Drawing.Size(100, 20);
            this.txt_BottomRightACSBaud.TabIndex = 30;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(363, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Baud Rate";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(289, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Bottom Right ACS Com Port";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(81, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "Baud Rate";
            // 
            // txt_BottomLeftACSBaud
            // 
            this.txt_BottomLeftACSBaud.Location = new System.Drawing.Point(160, 112);
            this.txt_BottomLeftACSBaud.Name = "txt_BottomLeftACSBaud";
            this.txt_BottomLeftACSBaud.Size = new System.Drawing.Size(100, 20);
            this.txt_BottomLeftACSBaud.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(131, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Bottom Left ACS Com Port";
            // 
            // frmComPorts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 407);
            this.Controls.Add(this.cb_BottomRightACSCom);
            this.Controls.Add(this.cb_BottomLeftACSCom);
            this.Controls.Add(this.txt_BottomRightACSBaud);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_BottomLeftACSBaud);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cb_TopRightACSCom);
            this.Controls.Add(this.cb_TopLeftACSCom);
            this.Controls.Add(this.txt_TopRightACSBaud);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_TopLeftACSBaud);
            this.Controls.Add(this.label4);
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
        private System.Windows.Forms.ComboBox cb_TopRightACSCom;
        private System.Windows.Forms.ComboBox cb_TopLeftACSCom;
        private System.Windows.Forms.TextBox txt_TopRightACSBaud;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_TopLeftACSBaud;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_BottomRightACSCom;
        private System.Windows.Forms.ComboBox cb_BottomLeftACSCom;
        private System.Windows.Forms.TextBox txt_BottomRightACSBaud;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_BottomLeftACSBaud;
        private System.Windows.Forms.Label label8;
    }
}