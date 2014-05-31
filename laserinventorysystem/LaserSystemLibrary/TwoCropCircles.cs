using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Ports;
using System.Timers;
using System.IO;
namespace LaserSystemLibrary
{
    public class TwoCropCircles
    {
        string TopLeftACSCom = "COM9";
        string TopRightACSCom = "COM17";
        string BottomLeftACSCom = "COM16";
        string BottomRightACSCom = "COM5";
        string GPSCom = "COM7";
        string LeftLMSCom = "COM12";
        string RightLMSCom = "COM11";

        ConcurrentQueue<ACS430Reading> TopLeftACSReadings;
        ConcurrentQueue<ACS430Reading> TopRightACSReadings = new ConcurrentQueue<ACS430Reading>();
        ConcurrentQueue<ACS430Reading> BottomLeftACSReadings = new ConcurrentQueue<ACS430Reading>();
        ConcurrentQueue<ACS430Reading> BottomRightACSReadings = new ConcurrentQueue<ACS430Reading>();
        ConcurrentQueue<NmeaSentence> GPSReadings = new ConcurrentQueue<NmeaSentence>();
        ConcurrentQueue<LmsScan2> LeftLMSReadings = new ConcurrentQueue<LmsScan2>();
        ConcurrentQueue<LmsScan2> RightLMSReadings = new ConcurrentQueue<LmsScan2>();
        
        GPSPath gpsPath = new GPSPath();
        ScanRepo LeftScanRepo = new ScanRepo();
        ScanRepo RightScanRepo = new ScanRepo();
        ACSShapefileWriter shapefile;
        static Stopwatch sw = new Stopwatch();
        bool threadstop = false;
        bool DebugToConsole = false;
        SensorLogger sensorLogger;
        Path2 path;
        System.Timers.Timer timer1 = new System.Timers.Timer(1500);
        ScanningOptions Options;

        int currentSecond = 0;

        public TwoCropCircles(
            ConcurrentQueue<ACS430Reading> topLeft,
            ConcurrentQueue<ACS430Reading> bottomLeft,
            ConcurrentQueue<ACS430Reading> topRight,
            ConcurrentQueue<ACS430Reading> bottomRight,

            ConcurrentQueue<NmeaSentence> gpsReadings,
            ConcurrentQueue<LmsScan2> leftLMSreadings,
            ConcurrentQueue<LmsScan2> rightLMSreadings,
            ScanningOptions options,
            Stopwatch stopWatch)
        {
            Options = options;
            TopLeftACSReadings = topLeft;
            BottomLeftACSReadings = bottomLeft;
            TopRightACSReadings = topRight;
            BottomRightACSReadings = bottomRight;
            GPSReadings = gpsReadings;
            LeftLMSReadings = leftLMSreadings;
            RightLMSReadings = rightLMSreadings;
            LaserScanUtilities.min_height = Options.minHeight;
            LaserScanUtilities.max_distance = Options.rowDistance;
            LaserScanUtilities.laserHeight = Options.laserHeight;
            sw = stopWatch;

            path = new Path2(Options, false, false, "none");
            string date = DateTime.Now.ToString("MM-dd-yy H.mm.ss");
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + date);
            shapefile = new ACSShapefileWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + date + "\\" + date + Options.OutputFileName + ".shp");
            sensorLogger = new SensorLogger(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + date + "\\", date);
            timer1.Start();
            timer1.Elapsed += timer_Elapsed;
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.Write("{0}, ", s);
            }

        }

        public TwoCropCircles()
        {
            sw.Start();
            Thread th1 = new Thread(RunTopLeftACS);
            th1.Name = "topleftacs";
            th1.Start();
            Thread th2 = new Thread(RunBottomLeftACS);
            th2.Name = "bottomleftacs";
            th2.Start();
            Thread th4 = new Thread(RunTopRightACS);
            th4.Name = "TopRightACS";
            th4.Start();
            Thread th5 = new Thread(RunBottomRightACS);
            th5.Name = "BottomRightACS";
            th5.Start();

            Thread th3 = new Thread(RunGPS);
            th3.Name = "gps";
            th3.Start();

            Thread th6 = new Thread(RunLeftLMS);
            th6.Name = "LeftLMS";
            th6.Start();

            Thread th7 = new Thread(RunRightLMS);
            th7.Name = "RightLMS";
            th7.Start();
        }
        

        public void Run()
        {
            

        }

        public void Stop()
        {
            timer1.Stop();
            Thread.Sleep(100);
            shapefile.Close();
            sensorLogger.Close();
        }
        void GetScans()
        {
            NmeaSentence sentence;
            LmsScan2 lmsScan;
            ACS430Reading acsReading;
            ACS430Reading reading;
            LmsScan2 scan;
            ScanRepo newLeftScans = new ScanRepo();
            ScanRepo newRightScans = new ScanRepo();
            while (GPSReadings.TryPeek(out sentence) | LeftLMSReadings.TryPeek(out lmsScan) |
                RightLMSReadings.TryPeek(out lmsScan) | TopLeftACSReadings.TryPeek(out acsReading) |
                BottomLeftACSReadings.TryPeek(out acsReading) | TopRightACSReadings.TryPeek(out acsReading) |
                BottomRightACSReadings.TryPeek(out acsReading))
            {
                if (TopLeftACSReadings.TryDequeue(out reading) != false)
                {
                    WriteToConsole(string.Format("TOP LEFT ms: {0}, values: {1},{2},{3},{4},{5}",
                        reading.Milliseconds, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI));
                    LeftScanRepo.ACSTopReadings.Add(reading);
                    newLeftScans.ACSTopReadings.Add(reading);
                    sensorLogger.LogTopLeftACS(reading);
                }

                if (BottomLeftACSReadings.TryDequeue(out reading) != false)
                {
                    WriteToConsole(string.Format("BOTTOM LEFT ms: {0}, values: {1},{2},{3},{4},{5}",
                        reading.Milliseconds, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI));
                    LeftScanRepo.ACSBottomReadings.Add(reading);
                    newLeftScans.ACSBottomReadings.Add(reading);
                    sensorLogger.LogBottomLeftACS(reading);
                }

                if (TopRightACSReadings.TryDequeue(out reading) != false)
                {
                    WriteToConsole(string.Format("TOP RIGHT ms: {0}, values: {1},{2},{3},{4},{5}",
                        reading.Milliseconds, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI));
                    RightScanRepo.ACSTopReadings.Add(reading);
                    newRightScans.ACSTopReadings.Add(reading);
                    sensorLogger.LogTopRightACS(reading);
                }

                if (BottomRightACSReadings.TryDequeue(out reading) != false)
                {
                    WriteToConsole(string.Format("BOTTOM RIGHT ms: {0}, values: {1},{2},{3},{4},{5}",
                        reading.Milliseconds, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI));
                    RightScanRepo.ACSBottomReadings.Add(reading);
                    newRightScans.ACSBottomReadings.Add(reading);
                    sensorLogger.LogBottomRightACS(reading);
                }

                if (GPSReadings.TryDequeue(out sentence) != false)
                {
                    sensorLogger.LogGPS(sentence);
                    pointXYZ point = parseGPSReading(sentence);
                    if (point != null)
                    {
                        gpsPath.AddPoint(point);
                        path.addPoint(point);
                    }
                }


                if (LeftLMSReadings.TryDequeue(out scan) != false)
                {
                    LeftScanRepo.LMSScans.Add(scan);
                    newLeftScans.LMSScans.Add(scan);
                    sensorLogger.LogLeftLMS(scan);
                }

                if (RightLMSReadings.TryDequeue(out scan) != false)
                {
                    RightScanRepo.LMSScans.Add(scan);
                    newRightScans.LMSScans.Add(scan);
                    sensorLogger.LogRightLMS(scan);
                }
            }
        }
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GetScans();
             
            double currentSecond = Math.Floor(sw.Elapsed.TotalSeconds);
            if (timer1.Interval == 1500)
                timer1.Interval = 1000;
            Console.WriteLine("OMFG A TIMER!");
            Console.WriteLine("saving!");
            ACS430Reading reading;
            List<ScanGroup> leftScans = new List<ScanGroup>();
            List<ScanGroup> rightScans = new List<ScanGroup>();
            Console.WriteLine("Left ticks: {0}", path.LeftTicks.Count);
            for (int i = 0; i < path.LeftTicks.Count; i++)
            {
                Console.WriteLine("{0} vs {1}", Math.Floor(path.LeftTicks[i].tick/1000), currentSecond);
                if (Math.Floor(path.LeftTicks[i].tick / 1000) <= currentSecond)
                {
                    ScanGroup sgL = GetScanGroup(LeftScanRepo, path.LeftTicks[i], true);
                    leftScans.Add(sgL);
                    path.LeftTicks.RemoveAt(i);
                    Console.WriteLine("left tick removed");
                    i--;
                }
            
            }
            Console.WriteLine("right ticks: {0}", path.RightTicks.Count);
            for (int i = 0; i < path.RightTicks.Count; i++)
            {
                if (Math.Floor(path.RightTicks[i].tick / 1000) <= currentSecond)
                {
                    ScanGroup sgR = GetScanGroup(RightScanRepo, path.RightTicks[i], false);

                    sgR.ScanResults = LaserScanUtilities.GetScanInfo(sgR.LMSScan.buffer, false);
                    rightScans.Add(sgR);
                    path.RightTicks.RemoveAt(i);
                    i--;
                    Console.WriteLine("right tick removed");
                }
            }
            
            foreach (ScanGroup sg in leftScans)
            {
                shapefile.write(sg, "left");
            }
            foreach (ScanGroup sg in rightScans)
            {
                shapefile.write(sg, "right");
            }
        }

        private ScanGroup GetScanGroup(ScanRepo ScanRepo,ScanLocation scanLocation, bool left)
        {
            ScanGroup SG = new ScanGroup();
            try
            {
                SG.LMSScan = ScanRepo.LMSScans.Aggregate((x, y) => Math.Abs(x.calculatedMilliseconds - SG.ScanLoc.tick) < Math.Abs(y.calculatedMilliseconds - SG.ScanLoc.tick) ? x : y);
                SG.ScanResults = LaserScanUtilities.GetScanInfo(SG.LMSScan.buffer, left);
            
            }
            catch
            {
                SG.LMSScan = new LmsScan2();
                SG.ScanResults = new ScanResults();
            }
            try
            {
                SG.TopReading = ScanRepo.ACSTopReadings.Aggregate((x, y) => Math.Abs(x.Milliseconds - SG.ScanLoc.tick) < Math.Abs(y.Milliseconds - SG.ScanLoc.tick) ? x : y);
            }
            catch
            {
                SG.TopReading = new ACS430Reading(0, 0, 0, 0, 0, 0);
            }
            try
            {
                SG.BottomReading = ScanRepo.ACSBottomReadings.Aggregate((x, y) => Math.Abs(x.Milliseconds - SG.ScanLoc.tick) < Math.Abs(y.Milliseconds - SG.ScanLoc.tick) ? x : y);
            }
            catch
            {
                SG.BottomReading = new ACS430Reading(0, 0, 0, 0, 0, 0);
            }
            SG.ScanResults = LaserScanUtilities.GetScanInfo(SG.LMSScan.buffer, left);
            return SG;
        }
        private void WriteToConsole(string str)
        {
            if (false)
            {
                Console.WriteLine(str);
            }
        }
        private void RunACS(string portName, ConcurrentQueue<ACS430Reading> queue)
        {
            ACS430 acs = new ACS430(portName, 38400, sw);
            ACS430Reading reading = null;
            while (true)
            {
                if (threadstop)
                {
                    break;
                }
                try
                {
                    reading = acs.Read();
                    if (reading != null)
                    {
                        queue.Enqueue(reading);
                    }
                }
                catch (ThreadAbortException err)
                {
                    acs.Close();
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
            NmeaReader gps = new NmeaReader(GPSCom, 38400, sw);
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
            LMS291_3 lms = new LMS291_3(LeftLMSCom, 500000, sw, true);
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
            LMS291_3 lms = new LMS291_3(RightLMSCom, 500000, sw, false);
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
            RunACS(TopLeftACSCom, TopLeftACSReadings);
        }

        public void RunTopRightACS()
        {
            RunACS(TopRightACSCom, TopRightACSReadings);
        }

        public void RunBottomLeftACS()
        {
            RunACS(BottomLeftACSCom, BottomLeftACSReadings);
        }

        public void RunBottomRightACS()
        {
            RunACS(BottomRightACSCom, BottomRightACSReadings);
        }

        public static pointXYZ parseGPSReading(NmeaSentence gpsReading)
        {
            try
            {
                GeoUTMConverter utmConverter = new GeoUTMConverter();
                string[] splitReading = gpsReading.buffer.Split(',');
                string latitude = splitReading[2];
                string longitude = splitReading[4];
                string zString = splitReading[9];
                pointXYZ point = new pointXYZ();

                point.latitude = (Convert.ToDouble(latitude.Substring(0, 2)) + Convert.ToDouble(latitude.Substring(2)) / 60);
                point.longitude = -(Convert.ToDouble(longitude.Substring(0, 3)) + Convert.ToDouble(longitude.Substring(3)) / 60);
                utmConverter.ToUTM(point.latitude, point.longitude);
                point.x = utmConverter.X;
                point.y = utmConverter.Y;
                point.z = Convert.ToDouble(zString);
                point.t = gpsReading.milliseconds;
                return point;
            }

            catch (Exception err)
            {
                return null;
            }
        }
    }
}
