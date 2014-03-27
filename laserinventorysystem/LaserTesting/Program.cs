using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserSystemLibrary;
using System.Diagnostics;
using System.Threading;

namespace LaserTesting
{
    class Program
    {
        static LMS291 lscanner;
        static LMS291 rscanner;
        static NmeaReader gps;
        static Stopwatch stopwatch;
        static GeoUTMConverter utmConverter = new GeoUTMConverter();

        static int currentSecond = 0;
        static void Main(string[] args)
        {

            System.DateTime starttime = System.DateTime.Now;
            ScanningOptions options = new ScanningOptions();
            options.comSettings = new ComSettings();
            options.comSettings.lComName = "COM6";
            options.comSettings.lBaudRate = 500000;
            options.comSettings.rComName = "COM5";
            options.comSettings.rBaudRate = 500000;
            options.comSettings.gpsComName = "COM3";
            options.comSettings.gpsBaudRate = 38000;
            options.laserHeight = 4;
            options.rowDistance = 5;
            options.minHeight = 2;
            options.samplingDistance = 1.0;
            options.OutputFilePath = "c:\\users\\daniel\\documents\\lms_data.gdb";
            options.outputTableName = "The table";
            options.useLeftLaser = false;
            options.useRightLaser = true;
            ScanRunner scanRunner = new ScanRunner(options, true, true);
            while (true)
            {
                if (Console.KeyAvailable)
                    break;
                scanRunner.run();
            }
            scanRunner.Stop();
            return;
            stopwatch = new Stopwatch();
            stopwatch.Start();
            rscanner = new LMS291(options.comSettings.rComName, options.comSettings.rBaudRate, stopwatch);
            lscanner = new LMS291(options.comSettings.lComName, options.comSettings.lBaudRate, stopwatch);
            gps = new NmeaReader(options.comSettings.gpsComName, options.comSettings.gpsBaudRate, stopwatch);
            Thread rThread = new Thread(new ThreadStart(runRightLaser));
            Thread lThread = new Thread(new ThreadStart(runLeftLaser));
            Thread gpsThread = new Thread(new ThreadStart(runGPS));
            lThread.Start();
            rThread.Start();
            gpsThread.Start();

            List<LmsScan> lScans = new List<LmsScan>();
            List<LmsScan> rScans = new List<LmsScan>();
            List<pointXYZ> gpsReadings = new List<pointXYZ>();
            List<int> times = new List<int>();
            Path path = new Path(options, true, false);

            for (; ; )
            {

                NmeaSentence gpsReading = new NmeaSentence();

                if (gps.readings.TryDequeue(out gpsReading))
                {
                    gpsReadings.Add(parseGPSReading(gpsReading));

                    //Console.WriteLine("{0}, {1}, {2}", gpsReadings.Last().x, gpsReadings.Last().y, gpsReadings.Last().z);
                    path.addPoint(gpsReadings.Last());
                }
                LmsScan scan = new LmsScan();
                if (rscanner.scans.TryDequeue(out scan))
                {
                    path.AddRightScan(scan);
                }

                scan = new LmsScan();
                if (lscanner.scans.TryDequeue(out scan))
                {
                    path.AddLeftScan(scan);
                }

                if (stopwatch.Elapsed.TotalSeconds > 10 & stopwatch.Elapsed.Seconds != currentSecond)
                {
                    currentSecond = stopwatch.Elapsed.Seconds;
                    path.ProcessScans();
                }
            }
        }

        private static pointXYZ parseGPSReading(NmeaSentence gpsReading)
        {
            string[] splitReading = gpsReading.buffer;
            string xString = splitReading[2];
            string yString = splitReading[4];
            string zString = splitReading[9];
            pointXYZ point = new pointXYZ();
            utmConverter.ToUTM(Convert.ToDouble(xString), Convert.ToDouble(yString));
            point.y = Convert.ToDouble(xString.Substring(0, 2)) + Convert.ToDouble(xString.Substring(2)) / 60;
            point.x = Convert.ToDouble(yString.Substring(0, 3)) + Convert.ToDouble(yString.Substring(3)) / 60;
            utmConverter.ToUTM(point.y, -point.x);
            point.x = utmConverter.X;
            point.y = utmConverter.Y;
            point.z = Convert.ToDouble(zString);
            point.t = gpsReading.ticks;

            Console.WriteLine("gcs x: {0}, y: {1}", xString, yString);
            return point;
        }

        static void runRightLaser()
        {
            rscanner.StartContinuousScan();
            for (; ; )
            {
                rscanner.read();
            }
        }
        static void runLeftLaser()
        {
            lscanner.StartContinuousScan();
            for (; ; )
            {
                lscanner.read();
            }
        }
        static void runGPS()
        {
            for (; ; )
            {
                gps.read();
            }
        }
    }
}
