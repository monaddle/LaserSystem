using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using LaserSystemLibrary;
namespace LaserSystem
{
    public partial class frmComPorts : Form
    {
        ScanningOptions settings;
        public frmComPorts(ScanningOptions settings1)
        {
            InitializeComponent();
            settings = settings1;
            cb_GPSCom.DataSource = SerialPort.GetPortNames();
            cb_LeftLaserCom.DataSource = SerialPort.GetPortNames();
            cb_RightLaserCom.DataSource = SerialPort.GetPortNames();
            if (SerialPort.GetPortNames().Length == 0)
            {
                MessageBox.Show("The system is showing that there aren't any COM ports available. Do you have the USB cable attached?");
            }

            if (settings.comSettings == null)
            {
                settings.comSettings = new LaserSystemLibrary.ComSettings();
                settings.comSettings.gpsBaudRate = 30000;
                settings.comSettings.gpsComName = "COM4";
                settings.comSettings.lBaudRate = 500000;
                settings.comSettings.lComName = "COM5";
                settings.comSettings.rBaudRate = 500000;
                settings.comSettings.rComName = "COM6";
            }

            txt_GPSBaud.Text = Convert.ToString(settings.comSettings.gpsBaudRate);
            txt_LeftLaserBaud.Text = Convert.ToString(settings.comSettings.lBaudRate);
            txt_RightLaserBaud.Text = Convert.ToString(settings.comSettings.rBaudRate);
            cb_GPSCom.SelectedItem = settings.comSettings.gpsComName;
            cb_LeftLaserCom.SelectedItem = settings.comSettings.lComName;
            cb_RightLaserCom.SelectedItem = settings.comSettings.rComName;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            settings.comSettings.gpsBaudRate = Convert.ToInt32(txt_GPSBaud.Text);
            settings.comSettings.lBaudRate = Convert.ToInt32(txt_LeftLaserBaud.Text);
            settings.comSettings.rBaudRate = Convert.ToInt32(txt_RightLaserBaud.Text);
            settings.comSettings.gpsComName = cb_GPSCom.Text;
            settings.comSettings.lComName = cb_LeftLaserCom.Text;
            settings.comSettings.rComName = cb_RightLaserCom.Text;
            settings.SaveSettings();
            this.Close();
        }

        private void updateComboBoxComPortsList(object sender, EventArgs e)
        {
            updateComboBoxCompPortsList((ComboBox)sender);
        }

        private void updateComboBoxCompPortsList(ComboBox combo)
        {
            combo.DataSource = SerialPort.GetPortNames();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
