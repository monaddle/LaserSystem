using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LaserSystemLibrary;
namespace LaserSystem
{
    public partial class ProcessScans : Form
    {
        ScanningOptions options;
        public ProcessScans(ScanningOptions settings)
        {
            InitializeComponent();
            options = settings;
            textBox_GDB.Text = settings.OutputFilePath;
        }

        private void button_GPSDialog_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog fd = new OpenFileDialog();
            fd.InitialDirectory = @"C:\";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                textBox_GPSFile.Text = fd.FileName;
                string[] left = fd.FileName.Split('\\');
                left[left.Length - 1] = "left" + left[left.Length - 1].Substring(3);
                string leftstring = left[0];
                for (int i = 1; i < left.Length; i++)
                {
                    leftstring += "\\" + left[i];
                }
                if (File.Exists(leftstring))
                {
                    textBox_LeftLaserFile.Text = leftstring;
                }
                string[] right = fd.FileName.Split('\\');
                right[right.Length - 1] = "right" + right[right.Length - 1].Substring(3);
                string rightstring = right[0];
                for (int i = 1; i < right.Length; i++)
                {
                    rightstring += "\\" + right[i];
                }
                if (File.Exists(rightstring))
                {
                    textBox_RightLaserFile.Text = rightstring;
                }
            }
        }

        private void button_GDB_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.SelectedPath = @"C:\\";
            if(fd.ShowDialog() == DialogResult.OK)
            {
                if(fd.SelectedPath.Split('.').Last() == "gdb")
                {
                    textBox_GDB.Text = fd.SelectedPath;
                }
            }
        }

        private void button_ProcessScans_Click(object sender, EventArgs e)
        {
            if (textBox_GPSFile.Text == "")
            {
                MessageBox.Show("No gps file selected!");
                return;
            }
            if (textBox_LeftLaserFile.Text == "" & textBox_RightLaserFile.Text == "")
            {
                MessageBox.Show("No laser files selected!");
                return;
            }
            foreach (Control control in this.Controls)
            {
                control.Enabled = false;
            }
            button_ProcessScans.Text = "Running Scans...";
            this.Refresh();
            options.OutputFilePath = textBox_GDB.Text;
            LaserScanUtilities.max_distance = options.rowDistance;
            bgw_ProcessScans.RunWorkerAsync();
        }

        private void bgw_ProcessScans_DoWork(object sender, DoWorkEventArgs e)
        {
            LaserSystemLibrary.Path path = new LaserSystemLibrary.Path(options, false, false);
            
            path.LoadSavedScans(textBox_GPSFile.Text, textBox_LeftLaserFile.Text, textBox_RightLaserFile.Text);
            double noScans = path.ProcessSavedScans();
            double currentNoScans = noScans;
            while (currentNoScans != 0)
            {
                bgw_ProcessScans.ReportProgress(Convert.ToInt32((noScans - currentNoScans) / noScans*100));
                currentNoScans = path.ProcessSavedScans();
            }
            path.Close();
        }

        private void bgw_ProcessScans_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            button_ProcessScans.Text = string.Format("Processing Scans: {0}%", e.ProgressPercentage);
        }

        private void bgw_ProcessScans_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                control.Enabled = true;
            }
            textBox_LeftLaserFile.Enabled = false;
            textBox_RightLaserFile.Enabled = false;

            button_ProcessScans.Text = "Scans processed.";
        }
    }
}
