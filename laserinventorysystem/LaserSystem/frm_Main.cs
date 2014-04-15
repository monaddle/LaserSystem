using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using LaserSystemLibrary;
using System.Diagnostics;
using System.IO;
namespace LaserSystem
{
    public partial class frm_main : Form
    {
        ScanningOptions settings = new ScanningOptions();

        static LMS291_2 rlaser;
        static LMS291_2 llaser;
        static List<LmsScan2> leftscans = new List<LmsScan2>();
        static List<LmsScan2> rightscans = new List<LmsScan2>();


        NmeaReader gps;
        Stopwatch stopwatch = new Stopwatch();
        EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        public frm_main()
        {
            InitializeComponent();
            stopwatch.Start();
            settings.LoadSettings();
            txtBox_height.Text = Convert.ToString(settings.laserHeight);
            txtBox_rowDistance.Text = Convert.ToString(settings.rowDistance);
            txtBox_OutputFilePath.Text = settings.OutputFilePath;
            txtBox_MinHeight.Text = Convert.ToString(settings.minHeight);

            cb_samplingDistance.SelectedIndex = settings.samplingDistanceSelectedIndex;
            try
            {
                llaser = new LMS291_2(settings.comSettings.lComName, settings.comSettings.lBaudRate, stopwatch, true);
            }
            catch { }
            try
            {
                rlaser = new LMS291_2(settings.comSettings.rComName, settings.comSettings.rBaudRate, stopwatch, false);
            }
            catch { }
            try
            {
                gps = new NmeaReader(settings.comSettings.gpsComName, settings.comSettings.gpsBaudRate, stopwatch);
            }
            catch { }

            gpsStatusChecker.RunWorkerAsync();
            leftLaserStatusChecker.RunWorkerAsync();
            rightLaserStatusChecker.RunWorkerAsync();
        }

        private void btn_saveSettings_Click(object sender, EventArgs e)
        {
            if (validateForm())
            {
                settings.OutputFilePath = txtBox_OutputFilePath.Text;
                settings.laserHeight = Convert.ToDouble(txtBox_height.Text);
                settings.rowDistance = Convert.ToDouble(txtBox_rowDistance.Text);
                settings.samplingDistance = Convert.ToDouble(cb_samplingDistance.SelectedValue);
                settings.samplingDistanceSelectedIndex = cb_samplingDistance.SelectedIndex;
                settings.minHeight = Convert.ToDouble(txtBox_MinHeight.Text);
                settings.SaveSettings();
            }
        }

        private bool validateForm()
        {
            bool isValid = true;
            string text;
            
            text = txtBox_height.Text;
            if (!Regex.IsMatch(text, @"^[0-9\.]*$"))
            {
                isValid = false;
            }

            text = txtBox_rowDistance.Text;
            if (!Regex.IsMatch(text, @"^[0-9\.]*$"))
            {
                isValid = false;
            }

            return isValid;
        }

        private void txtBox_height_TextChanged(object sender, EventArgs e)
        {
            string text = txtBox_height.Text;

            if (Regex.IsMatch(text, @"^[0-9\.]*$"))
            {
                errorProvider1.SetError(txtBox_height, null);
            }
            else
            {
                errorProvider1.SetError(txtBox_height, "Contains non-numeric values.");
            }
        }

        private void txtBox_rowDistance_TextChanged(object sender, EventArgs e)
        {
            string text = txtBox_rowDistance.Text;

            if (Regex.IsMatch(text, @"^[0-9\.]*$"))
            {
                errorProvider1.SetError(txtBox_rowDistance, null);
            }
            else
            {
                errorProvider1.SetError(txtBox_rowDistance, "Contains non-numeric values.");
            }
        }

        private void chkbox_leftLaser_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_leftLaser.Checked)
            {
                panel_leftLaser.BackColor = Color.Green;
            }
            else
            {
                panel_leftLaser.BackColor = Color.Gray;
            }
        }

        private void chkbox_rightLaser_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_rightLaser.Checked)
            {
                panel_rightLaser.BackColor = Color.Green;
            }
            else
            {
                panel_rightLaser.BackColor = Color.Gray;
            }
        }


        private void setCOMPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmComPorts comPortsForm = new frmComPorts(settings);
            comPortsForm.ShowDialog();
            

            if(settings.comSettings.lComName != "" & (llaser == null || llaser.port.PortName != settings.comSettings.lComName))
            {
                leftLaserStatusChecker.CancelAsync();
                Thread.Sleep(5);
                if(llaser != null)
                    llaser.close();
                try
                {
                    llaser = new LMS291_2(settings.comSettings.lComName, settings.comSettings.lBaudRate, stopwatch, true);
                    leftLaserStatusChecker.RunWorkerAsync();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }


            if (settings.comSettings.rComName != "" & (rlaser == null || rlaser.port.PortName != settings.comSettings.rComName))
            {
                rightLaserStatusChecker.CancelAsync();
                Thread.Sleep(5);
                if(rlaser != null)
                    rlaser.close();

                try
                {
                    rlaser = new LMS291_2(settings.comSettings.rComName, settings.comSettings.rBaudRate, stopwatch, false);
                    rightLaserStatusChecker.RunWorkerAsync();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }

            if (settings.comSettings.gpsComName != "" & (gps == null || gps.port.PortName != settings.comSettings.rComName))
            {
                gpsStatusChecker.CancelAsync();
                Thread.Sleep(5);
                if (gps != null)
                    rlaser.close();
                try
                {
                    gps = new NmeaReader(settings.comSettings.gpsComName, settings.comSettings.gpsBaudRate, stopwatch);
                    gpsStatusChecker.RunWorkerAsync();

                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }
        private void LaserCheckerDoWork(Panel panel, CheckBox chkbox, LMS291_2 laser, BackgroundWorker worker, string comName, int baudRate, List<LmsScan2> scanlist, bool leftLaser)
        {
            while (worker.CancellationPending != true)
            {
                try
                {
                    if (!chkbox.Checked)
                    {
                        panel.BackColor = Color.Gray;
                    }
                    
                    if (comName == "")
                    {
                        panel.BackColor = Color.Red;
                    }
                    if(laser == null)
                        laser = new LMS291_2(comName, baudRate, stopwatch, leftLaser);
                    laser.StartContinuousScan();
                    Thread.Sleep(200);
                    laser.StartContinuousScan();
                    Thread.Sleep(200);
                    laser.StartContinuousScan();
                    while (worker.CancellationPending != true)
                    {
                        if (CheckLaser(laser, scanlist))
                        {
                            worker.ReportProgress(100);
                        }
                        else
                        {
                            worker.ReportProgress(0);
                        }
                        if (worker.CancellationPending)
                        {
                            return;
                        }
                        if (chkbox.Checked)
                            Thread.Sleep(1000);
                        else
                        {
                            panel.BackColor = Color.Gray;
                            Thread.Sleep(1000);
                        }
                    }
                }
                catch
                {
                    panel.BackColor = Color.Red;
                    Thread.Sleep(1000);
                }
            }
        }
        private void LeftLaserChecker_DoWork(object sender, DoWorkEventArgs e)
        {
            LaserCheckerDoWork(panel_leftLaser, chkbox_leftLaser, llaser, sender as BackgroundWorker, settings.comSettings.lComName, settings.comSettings.lBaudRate, leftscans, true);
        }

        private void RightLaserChecker_DoWork(object sender, DoWorkEventArgs e)
        {
            LaserCheckerDoWork(panel_rightLaser, chkbox_rightLaser, rlaser, sender as BackgroundWorker, settings.comSettings.rComName, settings.comSettings.rBaudRate, rightscans, false);
        }

        private void GPSChecker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (worker.CancellationPending != true)
            {
                try
                {
                    if(gps == null)
                        gps = new NmeaReader(settings.comSettings.gpsComName, settings.comSettings.gpsBaudRate, stopwatch);

                    while (worker.CancellationPending != true)
                    {
                        Thread.Sleep(1000);
                        if (CheckGPS())
                        {
                            gpsStatusChecker.ReportProgress(100);
                        }
                        else
                        {
                            gpsStatusChecker.ReportProgress(0);
                        }
                    }
                }
                catch
                {
                    panel_GPS.BackColor = Color.Red;
                    Thread.Sleep(1000);
                }
            }
        }

        private bool CheckGPS()
        {
            long elapsedSeconds = stopwatch.ElapsedMilliseconds;

            while (stopwatch.ElapsedMilliseconds < elapsedSeconds + 500)
            {
                gps.read();
            }
            if (gps.readings.Count > 0)
            {
                gps.readings = new System.Collections.Concurrent.ConcurrentQueue<NmeaSentence>();
                return true;
            }
            return false;
        }

        private bool CheckRightLaser()
        {
            long currMilliseconds = stopwatch.ElapsedMilliseconds;
            while (stopwatch.ElapsedMilliseconds < currMilliseconds + 500)
            {
                rlaser.read();
            }
            if (rlaser.scans.Count > 0)
            {
                rlaser.scans = new System.Collections.Concurrent.ConcurrentQueue<LmsScan2>();
                return true;
            }
            else
                return false;
        }
        
        private bool CheckLeftLaser()
        {
            long currMilliseconds = stopwatch.ElapsedMilliseconds;
            while (stopwatch.ElapsedMilliseconds < currMilliseconds + 500)
            {
                llaser.read();
            }
            if (llaser.scans.Count > 0)
            {
                llaser.scans = new System.Collections.Concurrent.ConcurrentQueue<LmsScan2>();
                return true;
            }
            else
                return false;
        }
        private bool CheckLaser(LMS291_2 laser, List<LmsScan2> scanlist)
        {
            LmsScan2 scan;
            long currMilliseconds = stopwatch.ElapsedMilliseconds;
            while (stopwatch.ElapsedMilliseconds < currMilliseconds + 500)
            {
                scan = laser.read();
                if (scan != null)
                {
                    scanlist.Add(scan);
                }
            }
            if (scanlist.Count > 0)
            {
                scanlist.Clear();
                return true;
            }
            else
                return false;
        }

        private void GPSStatusChecker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 100)
            {
                panel_GPS.BackColor = Color.Green;
            }
            else
            {
                panel_GPS.BackColor = Color.Red;
            }
        }

        private void rightLaserChecker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (chkbox_rightLaser.Checked == false)
            {
                panel_rightLaser.BackColor = Color.Gray;
            }
            else if (e.ProgressPercentage == 100)
            {
                panel_rightLaser.BackColor = Color.Green;
            }
            else
            {
                panel_rightLaser.BackColor = Color.Red;
            }
        }

        private void leftLaserStatusChecker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 100)
            {
                panel_leftLaser.BackColor = Color.Green;
            }
            else
            {
                panel_leftLaser.BackColor = Color.Red;
            }
        }

        private void btn_StartScanning_Click(object sender, EventArgs e)
        {

            Thread.Sleep(500);
            if ((panel_leftLaser.BackColor == Color.Red) & chkbox_leftLaser.Checked |
               ((panel_rightLaser.BackColor == Color.Red) & chkbox_rightLaser.Checked) |
                (panel_GPS.BackColor == Color.Red))
            {
                MessageBox.Show("Oops! There's a problem with one of your sensors.");
                return;
            }

            if (llaser == null)
                Console.WriteLine("llasernull");
            if (rlaser == null)
                Console.WriteLine("rlasernull");
            GetSettingsFromForm();
            DisableInterface();
            btn_StopScanning.Enabled = true;
            leftLaserStatusChecker.CancelAsync();
            rightLaserStatusChecker.CancelAsync();
            gpsStatusChecker.CancelAsync();

            ScanRunnerWorker.RunWorkerAsync();
        }

        private void GetSettingsFromForm()
        {
            settings.OutputFilePath = "C:\\temp\\test2.shp";
            settings.OutputFileName = txtBox_OutputFilePath.Text;
            settings.laserHeight = Convert.ToDouble(txtBox_height.Text);
            settings.rowDistance = Convert.ToDouble(txtBox_rowDistance.Text);
            settings.samplingDistance = Convert.ToDouble(cb_samplingDistance.SelectedItem);
            settings.saveData = saveDataToolStripMenuItem.Checked;
            settings.useLeftLaser = chkbox_leftLaser.Checked;
            settings.useRightLaser = chkbox_rightLaser.Checked;
            settings.minHeight = Convert.ToDouble(txtBox_MinHeight.Text);
        }

        private void DisableInterface()
        {
            foreach (Control control in Controls)
            {
                control.Enabled = false;
            }
            btn_StopScanning.Enabled = true;
        }

        private void btn_StopScanning_Click(object sender, EventArgs e)
        {
            if (ScanRunnerWorker.IsBusy)
            {
                ScanRunnerWorker.CancelAsync();
                btn_StopScanning.Text = "Finishing scan...";
            }
        }
        
        private void ScanRunnerWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            Thread.Sleep(1000);
            ScanRunner4 scanRunner = new ScanRunner4(settings, false, saveDataToolStripMenuItem.Checked, llaser, rlaser, gps);
            try
            {
                
                BackgroundWorker worker = sender as BackgroundWorker;
                while (worker.CancellationPending != true)
                {
                    scanRunner.run();
                }
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.Elapsed.Seconds < 3)
                {
                    scanRunner.run();
                }
                scanRunner.Stop();
            }
            catch (Exception err)
            {
                Console.Beep(5000, 1000);
                MessageBox.Show(err.Message);
                MessageBox.Show(err.StackTrace);
                using (StreamWriter w = File.AppendText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "log.txt"))
                {
                    Log(err.Message, w);
                    Log(err.StackTrace, w);
                    Log(err.Source, w);
                    

                }
                MessageBox.Show("Trying to save...");
                try
                {
                    scanRunner.Stop();
                    MessageBox.Show("Save worked!");

                }
                catch (Exception err1)
                {
                    using (StreamWriter w = File.AppendText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "log.txt"))
                    {
                        Log(err1.Message, w);
                        Log(err1.StackTrace, w);
                        Log(err1.Source, w);
                    }
                    MessageBox.Show("Save failed.");
                }
            }
        }


        private void ScanRunnerWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Thread.Sleep(1000);
            try
            {
                leftLaserStatusChecker.RunWorkerAsync();
                rightLaserStatusChecker.RunWorkerAsync();
                gpsStatusChecker.RunWorkerAsync();
            }
            catch (Exception)
            {
                
            }
            foreach (Control control in Controls)
            {
                control.Enabled = true;
            }
            btn_StopScanning.Text = "Stop Scanning";
        }

        private void createGeodatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = @"C:\";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                CreateNewGDB(saveDialog.FileName);
            }
        }

        private void CreateNewGDB(string p)
        {
            throw new NotImplementedException();
        }



        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        public static void Log(string logMessage, StreamWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }
    }
}
