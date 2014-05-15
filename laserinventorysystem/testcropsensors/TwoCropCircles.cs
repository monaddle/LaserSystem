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
namespace testcropsensors
{
    class TwoCropCircles
    {
        string TopLeftACSCom = "COM17";
        string TopRightACSCom = "COM11";
        string BottomLeftACSCom = "COM16";
        string BottomRightACSCom = "COM12";
        string GPSCom = "COM9";

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
        Stopwatch sw = new Stopwatch();
        bool threadstop = false;
        SensorLogger sensorLogger;

        

        public void Run()
        {
            foreach(string s in SerialPort.GetPortNames())
            {
                Console.Write("{0}, ", s);
            }
            shapefile = new ACSShapefileWriter("C:/users/public/test.shp");
            List<Thread> threads = new List<Thread>();
            sw.Start();
            Thread th1 = new Thread(RunTopLeftACS);
            th1.Name = "topleftacs";
            th1.Start();
            threads.Add(th1);
            Thread th2 = new Thread(RunBottomLeftACS);
            th2.Name = "bottomleftacs";
            th2.Start();
            threads.Add(th2);
            Thread th3 = new Thread(RunGPS);
            th3.Name = "gps";
            th3.Start();
            threads.Add(th3);
            int currentSecond = 0;
            sensorLogger = new SensorLogger("c:\\users\\public\\documents\\", DateTime.Now.ToString("MM-dd-yy H.mm.ss"));
            ACS430Reading reading;
            NmeaSentence sentence;
            LmsScan2 scan;
            while (true)
            {
                if (currentSecond < sw.Elapsed.TotalSeconds - 1.5)
                {
                    Console.WriteLine("saving!");
                    currentSecond = Convert.ToInt32(Math.Floor(sw.Elapsed.TotalSeconds));
                    foreach (ACS430Reading r in LeftScanRepo.ACSTopReadings)
                    {
                        shapefile.write(0, 0, 0, r.RedEdge, r.NIR, r.Red, r.NDRE, r.NDVI, "now");
                    }
                    LeftScanRepo.ACSTopReadings.Clear();
                    foreach (ACS430Reading r in LeftScanRepo.ACSBottomReadings)
                    {
                        shapefile.write(0, 0, 0, r.RedEdge, r.NIR, r.Red, r.NDRE, r.NDVI, "now");
                    }
                    LeftScanRepo.ACSBottomReadings.Clear();

                    foreach (ACS430Reading r in RightScanRepo.ACSTopReadings)
                    {

                    }
                }

                if (Console.KeyAvailable)
                {
                    char c = Console.ReadKey().KeyChar;
                    if (c == 's')
                        break;
                }
                if (TopLeftACSReadings.TryDequeue(out reading) != false)
                {
                    Console.WriteLine(string.Format("TOP LEFT ms: {0}, values: {1},{2},{3},{4},{5}",
                        reading.Milliseconds, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI));
                    LeftScanRepo.ACSTopReadings.Add(reading);
                    sensorLogger.LogTopLeftACS(reading);
                }

                if (BottomLeftACSReadings.TryDequeue(out reading) != false)
                {
                    Console.WriteLine(string.Format("BOTTOM LEFT ms: {0}, values: {1},{2},{3},{4},{5}",
                        reading.Milliseconds, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI));
                    LeftScanRepo.ACSBottomReadings.Add(reading);
                    sensorLogger.LogBottomLeftACS(reading);
                }

                if (TopRightACSReadings.TryDequeue(out reading) != false)
                {
                    Console.WriteLine(string.Format("TOP RIGHT ms: {0}, values: {1},{2},{3},{4},{5}",
                        reading.Milliseconds, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI));
                    RightScanRepo.ACSTopReadings.Add(reading);
                    sensorLogger.LogTopRightACS(reading);
                }

                if (BottomLeftACSReadings.TryDequeue(out reading) != false)
                {
                    Console.WriteLine(string.Format("BOTTOM RIGHT ms: {0}, values: {1},{2},{3},{4},{5}",
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
            shapefile.close();
            threadstop = true;
        }

        class ACSRunner
        {
            bool requestStop = false;
            public void RunACS(string portName, ConcurrentQueue<ACS430Reading> queue)
            {
                ACS430 acs = new ACS430(portName, 38400);
                ACS430Reading reading = null;
                while (true)
                {
                    if(requestStop)
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

            public void RequestStop()
            {
                requestStop = true;
            }

        }
        private void RunACS(string portName, ConcurrentQueue<ACS430Reading> queue)
        {
            ACS430 acs = new ACS430(portName, 38400);
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
                string[] splitReading = gpsReading.buffer;
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
                point.t = gpsReading.ticks;
                return point;
            }

            catch (Exception err)
            {
                return null;
            }
        }
    }
}
