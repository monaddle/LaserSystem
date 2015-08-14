using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using LaserSystemLibrary;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Ports;
using System.Timers;
namespace testcropsensors
{
    class TwoCropCircles
    {
        string TopLeftACSCom = "COM9";
        string TopRightACSCom = "COM17";
        string BottomLeftACSCom = "COM16";
        string BottomRightACSCom = "COM5";
        string GPSCom = "COM7";
        string LeftLMSCom = "COM12";
        string RightLMSCom = "COM11";

        ConcurrentQueue<ACS430Reading> TopLeftACSReadings = new ConcurrentQueue<ACS430Reading>();
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
        

        public void Run()
        {
            timer1.Start();
            timer1.Elapsed += timer_Elapsed;
            foreach(string s in SerialPort.GetPortNames())
            {
                //Console.Write("{0}, ", s);
            }
            ScanningOptions options = new ScanningOptions();
            options.LoadSettings();
            path = new Path2(options, false, false, "none");
            
            shapefile = new ACSShapefileWriter("C:/users/public/test.shp");
            List<Thread> threads = new List<Thread>();
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

            int currentSecond = 0;
            sensorLogger = new SensorLogger("c:\\users\\public\\documents\\", DateTime.Now.ToString("MM-dd-yy H.mm.ss"));
            
            while (true)
            {
                if (currentSecond < sw.Elapsed.TotalSeconds - 1.5)
                {

                }

                if (Console.KeyAvailable)
                {
                    char c = Console.ReadKey().KeyChar;
                    if (c == 's')
                        break;
                }

            }
            shapefile.Close();
            threadstop = true;
        }

        void GetScans()
        {
            NmeaSentence sentence;
            LmsScan2 lmsScan;
            ACS430Reading acsReading;
            ACS430Reading reading;
            LmsScan2 scan;
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
                    sensorLogger.LogTopLeftACS(reading);
                }

                if (BottomLeftACSReadings.TryDequeue(out reading) != false)
                {
                    WriteToConsole(string.Format("BOTTOM LEFT ms: {0}, values: {1},{2},{3},{4},{5}",
                        reading.Milliseconds, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI));
                    LeftScanRepo.ACSBottomReadings.Add(reading);
                    sensorLogger.LogBottomLeftACS(reading);
                }

                if (TopRightACSReadings.TryDequeue(out reading) != false)
                {
                    WriteToConsole(string.Format("TOP RIGHT ms: {0}, values: {1},{2},{3},{4},{5}",
                        reading.Milliseconds, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI));
                    RightScanRepo.ACSTopReadings.Add(reading);
                    sensorLogger.LogTopRightACS(reading);
                }

                if (BottomRightACSReadings.TryDequeue(out reading) != false)
                {
                    WriteToConsole(string.Format("BOTTOM RIGHT ms: {0}, values: {1},{2},{3},{4},{5}",
                        reading.Milliseconds, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI));
                    RightScanRepo.ACSBottomReadings.Add(reading);
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
                    sensorLogger.LogLeftLMS(scan);
                }

                if (RightLMSReadings.TryDequeue(out scan) != false)
                {
                    RightScanRepo.LMSScans.Add(scan);
                    sensorLogger.LogLeftLMS(scan);
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
            List<ScanGroup> scansLeft = new List<ScanGroup>();
            List<ScanGroup> scansRight = new List<ScanGroup>();
            ScanGroup sgL = new ScanGroup();
            ScanGroup sgR = new ScanGroup();
            for (int i = 0; i < path.LeftTicks.Count; i++)
            {
                if (Math.Floor(path.LeftTicks[i].tick / 1000) <= currentSecond)
                {
                    sgL.ScanLoc = path.LeftTicks[i];
                    sgL.LMSScan = LeftScanRepo.LMSScans.Aggregate((x, y) => Math.Abs(x.calculatedMilliseconds - sgL.ScanLoc.tick) < Math.Abs(y.calculatedMilliseconds - sgL.ScanLoc.tick) ? x : y);
                    sgL.TopReading = LeftScanRepo.ACSTopReadings.Aggregate((x, y) => Math.Abs(x.Milliseconds - sgL.ScanLoc.tick) < Math.Abs(y.Milliseconds - sgL.ScanLoc.tick) ? x : y);
                    sgL.BottomReading = LeftScanRepo.ACSBottomReadings.Aggregate((x, y) => Math.Abs(x.Milliseconds - sgL.ScanLoc.tick) < Math.Abs(y.Milliseconds - sgL.ScanLoc.tick) ? x : y);
                    sgL.ScanResults = LaserScanUtilities.GetScanInfo(sgL.LMSScan.buffer, true);
                    scansLeft.Add(sgL);

                }
            }
            for (int i = 0; i < path.RightTicks.Count; i++)
            {
                if (Math.Floor(path.RightTicks[i].tick / 1000) <= currentSecond)
                {
                    sgR.ScanLoc = path.RightTicks[i];
                    sgR.LMSScan = RightScanRepo.LMSScans.Aggregate((x, y) => Math.Abs(x.calculatedMilliseconds - sgR.ScanLoc.tick) < Math.Abs(y.calculatedMilliseconds - sgR.ScanLoc.tick) ? x : y);
                    sgR.TopReading = RightScanRepo.ACSTopReadings.Aggregate((x, y) => Math.Abs(x.Milliseconds - sgR.ScanLoc.tick) < Math.Abs(y.Milliseconds - sgR.ScanLoc.tick) ? x : y);
                    sgR.BottomReading = RightScanRepo.ACSBottomReadings.Aggregate((x, y) => Math.Abs(x.Milliseconds - sgR.ScanLoc.tick) < Math.Abs(y.Milliseconds - sgR.ScanLoc.tick) ? x : y);
                    sgR.ScanResults = LaserScanUtilities.GetScanInfo(sgL.LMSScan.buffer, true);
                    scansRight.Add(sgR);
                }
            }
            foreach (ScanGroup sg in scansLeft)
            {
                shapefile.write(sg);
            }
            foreach (ScanGroup sg in scansRight)
            {
                shapefile.write(sg);
            }
        }
        private void WriteToConsole(string str)
        {
            if (DebugToConsole == true)
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
            LMS291 lms = new LMS291(LeftLMSCom, 500000, sw, true);
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
            LMS291 lms = new LMS291(RightLMSCom, 500000, sw, false);
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
