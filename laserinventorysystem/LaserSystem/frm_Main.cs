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
using System.Timers;
using System.Collections.Concurrent;

namespace LaserSystem
{
    public partial class frm_main : Form
    {
        ScanningOptions settings = new ScanningOptions();

        static LMS291_2 rlaser;
        static LMS291_2 llaser;
        static List<LmsScan2> leftscans = new List<LmsScan2>();
        static List<LmsScan2> rightscans = new List<LmsScan2>();

        System.Timers.Timer timer;
        bool threadstop = false;
        NmeaReader gps;
        Stopwatch stopwatch = new Stopwatch();
        EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

        ConcurrentQueue<ACS430Reading> TopLeftACSReadings = new ConcurrentQueue<ACS430Reading>();
        ConcurrentQueue<ACS430Reading> TopRightACSReadings = new ConcurrentQueue<ACS430Reading>();
        ConcurrentQueue<ACS430Reading> BottomLeftACSReadings = new ConcurrentQueue<ACS430Reading>();
        ConcurrentQueue<ACS430Reading> BottomRightACSReadings = new ConcurrentQueue<ACS430Reading>();
        ConcurrentQueue<NmeaSentence> GPSReadings = new ConcurrentQueue<NmeaSentence>();
        ConcurrentQueue<LmsScan2> LeftLMSReadings = new ConcurrentQueue<LmsScan2>();
        ConcurrentQueue<LmsScan2> RightLMSReadings = new ConcurrentQueue<LmsScan2>();
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
                ScannerRepo.LeftLMS = new LMS291_3(settings.comSettings.lComName, settings.comSettings.lBaudRate, stopwatch, true);
                Thread th6 = new Thread(RunLeftLMS);
                th6.Name = "LeftLMS";
                th6.Priority = ThreadPriority.AboveNormal;
                th6.Start();
            }
            catch { }
            try
            {
                ScannerRepo.RightLMS = new LMS291_3(settings.comSettings.rComName, settings.comSettings.rBaudRate, stopwatch, false);
                Thread th7 = new Thread(RunRightLMS);
                th7.Name = "RightLMS";
                th7.Priority = ThreadPriority.AboveNormal;
                th7.Start();
            }
            catch { }
            try
            {
                ScannerRepo.GPS = new NmeaReader(settings.comSettings.gpsComName, 38400, stopwatch);
                Thread th3 = new Thread(RunGPS);
                th3.Priority = ThreadPriority.AboveNormal;
                th3.Name = "gps";
                th3.Start();
            }
            catch { }

            try
            {
                ScannerRepo.TopLeftACS = new ACS430(settings.comSettings.TopLeftACSComName, 38400, stopwatch);
                Thread th1 = new Thread(RunTopLeftACS);
                th1.Name = "topleftacs";
                th1.Priority = ThreadPriority.AboveNormal;
                th1.Start();
            }
            catch { } 

            List<ACS430> ACSs = new List<ACS430>();
            for (int i = 9; i <= 16; i++)
            {

            }

            try
            {
                ScannerRepo.BottomLeftACS = new ACS430(settings.comSettings.BottomLeftACSComName, 38400, stopwatch);
                Thread th2 = new Thread(RunBottomLeftACS);
                th2.Name = "bottomleftacs";
                th2.Priority = ThreadPriority.AboveNormal;
                th2.Start();

            }
            catch { }
            try
            {
                ScannerRepo.TopRightACS = new ACS430(settings.comSettings.TopRightACSComName, 38400, stopwatch);
                Thread th4 = new Thread(RunTopRightACS);
                th4.Name = "TopRightACS";
                th4.Priority = ThreadPriority.AboveNormal;
                th4.Start();
            }
            catch { }
            try
            {
                ScannerRepo.BottomRightACS = new ACS430(settings.comSettings.BottomRightACSComName, 38400, stopwatch);
                Thread th5 = new Thread(RunBottomRightACS);
                th5.Name = "BottomRightACS";
                th5.Priority = ThreadPriority.AboveNormal;
                th5.Start();
            }
            catch { }


            timer = new System.Timers.Timer(1000);
            
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GetScans();
        }

        void GetScans()
        {
            NmeaSentence sentence;
            LmsScan2 lmsScan;
            ACS430Reading acsReading;
            ACS430Reading reading;
            LmsScan2 scan;
            bool topleft = false;
            bool topright = false;
            bool bottomleft = false;
            bool bottomright = false;
            bool gps = false;
            bool leftlms = false;
            bool rightlms = false;
            
            while (GPSReadings.TryPeek(out sentence) | LeftLMSReadings.TryPeek(out lmsScan) |
                RightLMSReadings.TryPeek(out lmsScan) | TopLeftACSReadings.TryPeek(out acsReading) |
                BottomLeftACSReadings.TryPeek(out acsReading) | TopRightACSReadings.TryPeek(out acsReading) |
                BottomRightACSReadings.TryPeek(out acsReading))
            {
                if (TopLeftACSReadings.TryDequeue(out reading) != false)
                {
                    topleft = true;
                }

                if (BottomLeftACSReadings.TryDequeue(out reading) != false)
                {
                    bottomleft = true;
                }

                if (TopRightACSReadings.TryDequeue(out reading) != false)
                {
                    topright = true;
                }

                if (BottomRightACSReadings.TryDequeue(out reading) != false)
                {
                    bottomright = true;
                }

                if (GPSReadings.TryDequeue(out sentence) != false)
                {
                    gps = true;
                }

                if (LeftLMSReadings.TryDequeue(out scan) != false)
                {
                    leftlms = true;
                }

                if (RightLMSReadings.TryDequeue(out scan) != false)
                {
                    rightlms = true;
                }
            }

            SetSensorPanel(panel_leftLaser, leftlms, chkbox_leftLaser.Checked);
            SetSensorPanel(panel_topLeftACS, topleft, chkbox_leftLaser.Checked);
            SetSensorPanel(panel_BottomLeftACS, bottomleft, chkbox_leftLaser.Checked);
            SetSensorPanel(panel_rightLaser, rightlms, chkbox_rightLaser.Checked);
            SetSensorPanel(panel_TopRightACS, topright, chkbox_rightLaser.Checked);
            SetSensorPanel(panel_BottomRightACS, bottomright, chkbox_rightLaser.Checked);
            SetSensorPanel(panel_GPS, gps, true);

            
        }

        private void SetSensorPanel(Panel panel, bool working, bool enabled)
        {
            if (!enabled)
            {
                panel.BackColor = Color.Gray;
            }
            else if (working)
            {
                panel.BackColor = Color.Green;
            }
            else
            {
                panel.BackColor = Color.Red;
            }
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



        private void setCOMPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmComPorts comPortsForm = new frmComPorts(settings);
            comPortsForm.ShowDialog();
        }


        private void btn_StartScanning_Click(object sender, EventArgs e)
        {

            Thread.Sleep(500);
            GetSettingsFromForm();
            DisableInterface();
            btn_StopScanning.Enabled = true;

            ScanRunnerWorker.RunWorkerAsync();
            
        }



        private void ScanRunnerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            TwoCropCircles tcc = new TwoCropCircles(
                TopLeftACSReadings,
                BottomLeftACSReadings,
                TopRightACSReadings,
                BottomRightACSReadings,
                GPSReadings,
                LeftLMSReadings,
                RightLMSReadings,
                settings,
                stopwatch);
            timer.Stop();
            stopwatch.Restart();
            timer_Elapsed(null, null);
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                while (worker.CancellationPending != true)
                {
                    tcc.Run();
                }
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.Elapsed.Seconds < 2)
                {
                    tcc.Run();
                }
                tcc.Stop();
            }
            catch (Exception err)
            {
                timer.Start();
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
        private void btn_StopScanning_Click(object sender, EventArgs e)
        {
            timer.Start();
            if (ScanRunnerWorker.IsBusy)
            {
                ScanRunnerWorker.CancelAsync();
                btn_StopScanning.Text = "Finishing scan...";
            }
        }
        

        private void ScanRunnerWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Thread.Sleep(1000);
            try
            {
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
            settings.UsePolygonLayer = usePolygonConstraintToolStripMenuItem.Checked;
        }

        private void DisableInterface()
        {
            foreach (Control control in Controls)
            {
                control.Enabled = false;
            }
            btn_StopScanning.Enabled = true;
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

        private void frm_main_Load(object sender, EventArgs e)
        {

        }

        private void RunACS(string portName, ConcurrentQueue<ACS430Reading> queue, ACS430 acs)
        {
            ACS430 ACS = acs;
            ACS430Reading reading = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (threadstop)
                {
                    break;
                }
                try
                {
                    reading = ACS.Read();
                    if (reading != null)
                    {
                        queue.Enqueue(reading);
                    }
                }
                catch (ThreadAbortException err)
                {
                    ACS.Close();
                    break;
                }
                catch (ObjectDisposedException err)
                {
                    break;
                }
            }
        }
        private void RunGPS()
        {
            NmeaReader gps = ScannerRepo.GPS;
            NmeaSentence sentence;
            while (true)
            {
                if (threadstop)
                {
                    break;
                }
                try
                {
                    gps.read();
                    if (gps.readings.TryDequeue(out sentence) != false)
                    {
                        GPSReadings.Enqueue(sentence);
                    }
                }
                catch (ThreadAbortException err)
                {
                    gps.close();
                    break;
                }

                catch (ObjectDisposedException err)
                {
                    break;
                }

            }
        }
        public void RunLeftLMS()
        {
            LMS291_3 lms = ScannerRepo.LeftLMS;
            LmsScan2 scan;
            lms.StartContinuousScan();
            Thread.Sleep(1);
            lms.StartContinuousScan();
            Thread.Sleep(1);
            while (true)
            {
                if (threadstop)
                {
                    break;
                }
                scan = lms.read();
                if (scan != null)
                {
                    LeftLMSReadings.Enqueue(scan);
                }
            }
        }

        public void RunRightLMS()
        {
            LMS291_3 lms = ScannerRepo.RightLMS;
            LmsScan2 scan;
            lms.StartContinuousScan();
            Thread.Sleep(1);
            lms.StartContinuousScan();
            Thread.Sleep(1);

            while (true)
            {
                if (threadstop)
                {
                    break;
                }
                scan = lms.read();
                if (scan != null)
                {
                    RightLMSReadings.Enqueue(scan);
                }
            }
        }

        public void RunTopLeftACS()
        {
            RunACS(settings.comSettings.TopLeftACSComName, TopLeftACSReadings, ScannerRepo.TopLeftACS);
        }

        public void RunTopRightACS()
        {
            RunACS(settings.comSettings.TopRightACSComName, TopRightACSReadings, ScannerRepo.TopRightACS);
        }

        public void RunBottomLeftACS()
        {
            RunACS(settings.comSettings.BottomLeftACSComName, BottomLeftACSReadings, ScannerRepo.BottomLeftACS);
        }

        public void RunBottomRightACS()
        {
            RunACS(settings.comSettings.BottomRightACSComName, BottomRightACSReadings, ScannerRepo.BottomRightACS);
        }

        private void frm_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            threadstop = true;
        }

        private void usePolygonConstraintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            usePolygonConstraintToolStripMenuItem.Checked = !usePolygonConstraintToolStripMenuItem.Checked;
        }


    }
}
