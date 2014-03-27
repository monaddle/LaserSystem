using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
namespace LaserSystemLibrary
{
    public class ScanRunner2
    {
        ScanningOptions Options;
        LMS291 lscanner;
        LMS291 rscanner;
        LMS291 lscanner2;
        LMS291 rscanner2;
        NmeaReader gps;
        Stopwatch stopwatch;
        GeoUTMConverter utmConverter = new GeoUTMConverter();
        int currentSecond;
        Path path;

        List<LmsScan> lScans = new List<LmsScan>();
        List<LmsScan> rScans = new List<LmsScan>();
        List<pointXYZ> gpsReadings = new List<pointXYZ>();
        List<int> times = new List<int>();
        Thread rThread;
        Thread lThread;
        Thread gpsThread;
        public void run()
        {
            for (int i = 0; i < 100; i++)
            {
                CheckScanners();
            }
            NmeaSentence gpsReading = new NmeaSentence();

            if (gps.readings.TryDequeue(out gpsReading))
            {
                gpsReadings.Add(parseGPSReading(gpsReading));
            }
            LmsScan scan = new LmsScan();
            if (rscanner2.scans.TryDequeue(out scan))
            {
                path.AddRightScan(scan);
            }

            scan = new LmsScan();
            if (lscanner2.scans.TryDequeue(out scan))
            {
                path.AddLeftScan(scan);
            }



            if (stopwatch.Elapsed.Seconds != currentSecond)
            {
                path.addPoint(gpsReadings.Last());

                if (stopwatch.Elapsed.TotalSeconds > 3)
                {
                    path.ProcessScans();
                }

                currentSecond = stopwatch.Elapsed.Seconds;
            }
        }

        public void CheckScanners()
        {
            rscanner2.read();
            lscanner2.read();
            gps.read();
        }
        public ScanRunner2(ScanningOptions options, bool fakeReadings, bool saveReadings, LMS291 LScanner, LMS291 RScanner, NmeaReader GPS)
        {
            LaserScanUtilities.min_height = options.minHeight;
            LaserScanUtilities.max_distance = options.rowDistance;
            LaserScanUtilities.laserHeight = options.laserHeight;

            Options = options;
            stopwatch = new Stopwatch();
            stopwatch.Start();
            //rscanner = RScanner;
            //lscanner = LScanner;
            //rscanner.stopWatch = stopwatch;
            //lscanner.stopWatch = stopwatch;
            //gps = GPS;
            lscanner2 = new LMS291(Options.comSettings.lComName, Options.comSettings.lBaudRate, stopwatch);
            rscanner2 = new LMS291(Options.comSettings.rComName, Options.comSettings.rBaudRate, stopwatch);

            Thread.Sleep(200);
            rscanner2.StartContinuousScan();
            lscanner2.StartContinuousScan();
            Thread.Sleep(200);
            rscanner2.StartContinuousScan();
            lscanner2.StartContinuousScan();
            gps = new NmeaReader(options.comSettings.gpsComName, options.comSettings.gpsBaudRate, stopwatch);

            //rThread = new Thread(new ThreadStart(runRightLaser));
            //lThread = new Thread(new ThreadStart(runLeftLaser));
            //gpsThread = new Thread(new ThreadStart(runGPS));
            //lThread.Start();
            //rThread.Start();
            //gpsThread.Start();

            List<LmsScan> lScans = new List<LmsScan>();
            List<LmsScan> rScans = new List<LmsScan>();
            List<pointXYZ> gpsReadings = new List<pointXYZ>();
            List<int> times = new List<int>();

            path = new Path(options, fakeReadings, saveReadings);
            path.LeftActive = options.useLeftLaser;
            path.RightActive = options.useRightLaser;
        }

        void runRightLaser()
        {
            try
            {
                LMS291 rscanner2 = new LMS291(Options.comSettings.rComName, Options.comSettings.rBaudRate, stopwatch);

                rscanner2.StartContinuousScan();
                for (; ; )
                {
                    rscanner2.read();
                }
            }
            catch
            {
            }
        }
        void runLeftLaser()
        {
            try
            {
                LMS291 lscanner2 = new LMS291(Options.comSettings.lComName, Options.comSettings.lBaudRate, stopwatch);

                lscanner2.StartContinuousScan();
                for (; ; )
                {
                    lscanner2.read();
                }
            }
            catch
            {
            }
        }
        void runGPS()
        {
            try
            {
                for (; ; )
                {
                    gps.read();
                }
            }
            catch
            {
                gps.close();
            }
        }
        private pointXYZ parseGPSReading(NmeaSentence gpsReading)
        {
            string[] splitReading = gpsReading.buffer;
            string latitude = splitReading[2];
            string longitude = splitReading[4];
            string zString = splitReading[9];
            pointXYZ point = new pointXYZ();
            utmConverter.ToUTM(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
            point.x = Convert.ToDouble(latitude.Substring(0, 2)) + Convert.ToDouble(latitude.Substring(2)) / 60;
            point.y = Convert.ToDouble(longitude.Substring(0, 3)) + Convert.ToDouble(longitude.Substring(3)) / 60;
            //Console.WriteLine("x:{0}, y:{1}", point.x, point.y);
            utmConverter.ToUTM(point.x, point.y);
            point.x = utmConverter.X;
            point.y = utmConverter.Y;
            //Console.WriteLine("x:{0}, y:{1}", point.x, point.y);

            point.z = Convert.ToDouble(zString);
            point.t = gpsReading.ticks;
            return point;
        }

        public void Stop()
        {
            lThread.Abort();
            rThread.Abort();
            gpsThread.Abort();
            path.Close();


        }
    }
}
