using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
namespace LaserSystemLibrary
{
    public class ScanRunner3
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
        Thread ScannerThread;
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
        public ScanRunner3(ScanningOptions options, bool fakeReadings, bool saveReadings, LMS291 LScanner, LMS291 RScanner, NmeaReader GPS)
        {
            LaserScanUtilities.min_height = options.minHeight;
            LaserScanUtilities.max_distance = options.rowDistance;
            LaserScanUtilities.laserHeight = options.laserHeight;

            Options = options;
            stopwatch = new Stopwatch();
            stopwatch.Start();
            //rscanner = new LMS291(options.comSettings.rComName, options.comSettings.rBaudRate, stopwatch);
            //lscanner = new LMS291(options.comSettings.lComName, options.comSettings.lBaudRate, stopwatch);
            //gps = new NmeaReader(options.comSettings.gpsComName, options.comSettings.gpsBaudRate, stopwatch);

            rscanner = RScanner;
            lscanner = LScanner;
            gps = GPS;

            ScannerThread = new Thread(runScanners);
            ScannerThread.Priority = ThreadPriority.Highest;
            
            List<LmsScan> lScans = new List<LmsScan>();
            List<LmsScan> rScans = new List<LmsScan>();
            List<pointXYZ> gpsReadings = new List<pointXYZ>();
            List<int> times = new List<int>();

            path = new Path(options, fakeReadings, saveReadings);
            path.LeftActive = options.useLeftLaser;
            path.RightActive = options.useRightLaser;

            rscanner.scans = new System.Collections.Concurrent.ConcurrentQueue<LmsScan>();
            lscanner.scans = new System.Collections.Concurrent.ConcurrentQueue<LmsScan>();
            gps.readings = new System.Collections.Concurrent.ConcurrentQueue<NmeaSentence>();
            rscanner.Reset();
            lscanner.Reset();
            
            lscanner.stopWatch.Restart();
            stopwatch.Restart();
            ScannerThread.Start();
        }

        void runScanners()
        {
            while (true)
            {
                rscanner.read();
                lscanner.read();
                gps.read();
            }
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
        public pointXYZ parseGPSReading(NmeaSentence gpsReading)
        {
            GeoUTMConverter utmConverter = new GeoUTMConverter();
            string[] splitReading = gpsReading.buffer;
            string latitude = splitReading[2];
            string longitude = splitReading[4];
            string zString = splitReading[9];
            pointXYZ point = new pointXYZ();

            try
            {
                utmConverter.ToUTM(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
                point.latitude = (Convert.ToDouble(latitude.Substring(0, 2)) + Convert.ToDouble(latitude.Substring(2)) / 60);
                point.longitude = -(Convert.ToDouble(longitude.Substring(0, 3)) + Convert.ToDouble(longitude.Substring(3)) / 60);
                //point.longitude = -(Convert.ToDouble(longitude.Substring(0, 3)) + Convert.ToDouble(longitude.Substring(3, 2)) / 60) - Convert.ToDouble(longitude.Substring(5)) / 60;
                //Console.WriteLine("x:{0}, y:{1}", point.x, point.y);
                utmConverter.ToUTM(point.latitude, point.longitude);
                point.x = utmConverter.X;
                point.y = utmConverter.Y;
                //Console.WriteLine("x:{0}, y:{1}", point.x, point.y);
                point.z = Convert.ToDouble(zString);
                point.t = gpsReading.ticks;
                return point;
            }
            catch (FormatException err)
            {
                return null;
            }
        }

        //public pointXYZ parseGPSReading(NmeaSentence gpsReading)
        //{
        //    GeoUTMConverter utmConverter = new GeoUTMConverter();
        //    string[] splitReading = gpsReading.buffer;
        //    string latitude = splitReading[2];
        //    string longitude = splitReading[4];
        //    string zString = splitReading[9];
        //    pointXYZ point = new pointXYZ();
        //    try
        //    {
        //        utmConverter.ToUTM(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
        //        point.latitude = (Convert.ToDouble(latitude.Substring(0, 2)) + Convert.ToDouble(latitude.Substring(2)) / 60);
        //        point.longitude = -(Convert.ToDouble(longitude.Substring(0, 3)) + Convert.ToDouble(longitude.Substring(3, 2)) / 60) - Convert.ToDouble(longitude.Substring(5)) / 60;

        //        //Console.WriteLine("x:{0}, y:{1}", point.x, point.y);
        //        utmConverter.ToUTM(point.latitude, point.longitude);
        //        point.x = utmConverter.X;
        //        point.y = utmConverter.Y;
        //        //Console.WriteLine("x:{0}, y:{1}", point.x, point.y);
        //        point.z = Convert.ToDouble(zString);
        //        point.t = gpsReading.ticks;
        //        return point;
        //    }
        //    catch (FormatException err)
        //    {
        //        return null;
        //    }
        //}

        public void Stop()
        {
            ScannerThread.Abort();
            path.Close();
        }
    }
}
