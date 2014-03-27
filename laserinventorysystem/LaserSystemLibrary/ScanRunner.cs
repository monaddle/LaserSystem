using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
namespace LaserSystemLibrary
{
    public class ScanRunner
    {
        ScanningOptions Options;
        LMS291 lscanner;
        LMS291 rscanner;
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
            NmeaSentence gpsReading = new NmeaSentence();

            if (gps.readings.TryDequeue(out gpsReading))
            {
                gpsReadings.Add(parseGPSReading(gpsReading));
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



            if (stopwatch.Elapsed.Seconds != currentSecond)
            {
                path.addPoint(gpsReadings.Last());

                if (stopwatch.Elapsed.TotalSeconds > 5)
                {
                    path.ProcessScans();
                }

                currentSecond = stopwatch.Elapsed.Seconds;
            }
        }
        public ScanRunner (ScanningOptions options, bool fakeReadings, bool saveReadings)
        {
            LaserScanUtilities.min_height = 0;
            LaserScanUtilities.max_distance = options.rowDistance;
            LaserScanUtilities.laserHeight = options.laserHeight;
            
            Options = options;
            stopwatch = new Stopwatch();
            stopwatch.Start();
            rscanner = new LMS291(options.comSettings.rComName, options.comSettings.rBaudRate, stopwatch);
            lscanner = new LMS291(options.comSettings.lComName, options.comSettings.lBaudRate, stopwatch);
            gps = new NmeaReader(options.comSettings.gpsComName, options.comSettings.gpsBaudRate, stopwatch);
            rThread = new Thread(new ThreadStart(runRightLaser));
            lThread = new Thread(new ThreadStart(runLeftLaser));
            gpsThread = new Thread(new ThreadStart(runGPS));
            lThread.Start();
            //rThread.Start();
            gpsThread.Start();

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
                rscanner.StartContinuousScan();
                for (; ; )
                {
                    rscanner.read();
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
                lscanner.StartContinuousScan();
                for (; ; )
                {
                    lscanner.read();
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
