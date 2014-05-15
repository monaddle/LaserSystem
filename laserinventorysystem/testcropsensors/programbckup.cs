using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using System.Collections.Concurrent;
using LaserSystemLibrary;
namespace testcropsensors
{
    class Program1
    {
        static string TopLeftACSCom = "COM5";
        static string TopRightACSCom = "COM11";
        static string BottomLeftACSCom = "COM9";
        static string BottomRightACSCom = "COM12";
        static string GPSCom = "COM6";

        static ConcurrentQueue<ACS430Reading> TopLeftACSReadings = new ConcurrentQueue<ACS430Reading>();
        static ConcurrentQueue<ACS430Reading> TopRightACSReadings = new ConcurrentQueue<ACS430Reading>();
        static ConcurrentQueue<ACS430Reading> BottomLeftACSReadings = new ConcurrentQueue<ACS430Reading>();
        static ConcurrentQueue<ACS430Reading> BottomRightACSReadings = new ConcurrentQueue<ACS430Reading>();
        static ConcurrentQueue<NmeaSentence> GPSReadings = new ConcurrentQueue<NmeaSentence>();

        static GPSPath gpsPath = new GPSPath();
        static ScanRepo LeftScanRepo = new ScanRepo();
        static ACSShapefileWriter shapefile;
        static Stopwatch sw = new Stopwatch();

        static void Main1(string[] args)
        {
            TwoCropCircles tcc = new TwoCropCircles();
            tcc.Run();

            return;

            shapefile = new ACSShapefileWriter("C:/users/public/test.shp");
            List<Thread> threads = new List<Thread>();
            String.Join(", ", SerialPort.GetPortNames());
            Console.WriteLine(String.Join(", ", SerialPort.GetPortNames()));
            sw.Start();
            Thread th1 = new Thread(RunTopLeftACS);
            th1.Start();
            threads.Add(th1);
            Thread th2 = new Thread(RunBottomLeftACS);
            th2.Start();
            threads.Add(th2);
            Thread th3 = new Thread(RunGPS);
            th3.Start();
            threads.Add(th3);
            int currentSecond = 0;


            ACS430Reading reading;
            NmeaSentence sentence;
            while (true)
            {
                if (currentSecond != sw.Elapsed.Seconds)
                {
                    currentSecond = sw.Elapsed.Seconds;
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
                    shapefile.write(0, 0, 0, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI, "now");
                }

                if (BottomLeftACSReadings.TryDequeue(out reading) != false)
                {
                    Console.WriteLine(string.Format("BOTTOM LEFT ms: {0}, values: {1},{2},{3},{4},{5}",
                        reading.Milliseconds, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI));
                    LeftScanRepo.ACSBottomReadings.Add(reading);
                    shapefile.write(0, 0, 0, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI, "now");
                }

                if (TopRightACSReadings.TryDequeue(out reading) != false)
                {
                    Console.WriteLine(string.Format("TOP RIGHT ms: {0}, values: {1},{2},{3},{4},{5}",
                        reading.Milliseconds, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI));
                }

                if (BottomLeftACSReadings.TryDequeue(out reading) != false)
                {
                    Console.WriteLine(string.Format("BOTTOM RIGHT ms: {0}, values: {1},{2},{3},{4},{5}",
                        reading.Milliseconds, reading.RedEdge, reading.NIR, reading.Red, reading.NDRE, reading.NDVI));
                }

                if (GPSReadings.TryDequeue(out sentence) != false)
                {
                    pointXYZ point = parseGPSReading(sentence);
                    if (point != null)
                    {
                        gpsPath.AddPoint(point);
                    }
                }
            }
            shapefile.close();
            Thread.Sleep(1000);
            foreach (Thread th in threads)
            {
                th.Abort();
            }
        }

        private static void RunACS(string portName, ConcurrentQueue<ACS430Reading> queue)
        {
            ACS430 acs = new ACS430(portName, 38400);
            ACS430Reading reading = null;
            while (true)
            {
                reading = acs.Read();
                if (reading != null)
                {
                    queue.Enqueue(reading);
                }
            }
        }
        public static void RunGPS()
        {
            NmeaReader gps = new NmeaReader(GPSCom, 9600, sw);
            NmeaSentence sentence;
            while (true)
            {
                gps.read();
                if (gps.readings.TryDequeue(out sentence) != false)
                {
                    GPSReadings.Enqueue(sentence);
                }
            }
        }
        public static void RunTopLeftACS()
        {
            RunACS(TopLeftACSCom, TopLeftACSReadings);
        }

        public static void RunTopRightACS()
        {
            RunACS(TopRightACSCom, TopRightACSReadings);
        }

        public static void RunBottomLeftACS()
        {
            RunACS(BottomLeftACSCom, BottomLeftACSReadings);
        }

        public static void RunBottomRightACS()
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
