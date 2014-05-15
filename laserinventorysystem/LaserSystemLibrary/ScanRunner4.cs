using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using DotSpatial.Projections;

namespace LaserSystemLibrary
{
    public class ScanRunner4
    {
        ScanningOptions Options;
        LMS291_2 lscanner;
        LMS291_2 rscanner;
        NmeaReader gps;
        Stopwatch stopwatch;
        GeoUTMConverter utmConverter = new GeoUTMConverter();
        Path2 path;
        string date = DateTime.Now.ToString("MMMM-dd-yyyyHmmss");

        Dictionary<int, List<LmsScan2>> lScanDict = new Dictionary<int, List<LmsScan2>>();
        Dictionary<int, List<LmsScan2>> rScanDict = new Dictionary<int, List<LmsScan2>>();
        List<NmeaSentence> GPSReadings = new List<NmeaSentence>();
        LmsScan2 scan;
        System.Collections.Concurrent.ConcurrentQueue<LmsScan2> LeftScans = new System.Collections.Concurrent.ConcurrentQueue<LmsScan2>();
        System.Collections.Concurrent.ConcurrentQueue<LmsScan2> RightScans = new System.Collections.Concurrent.ConcurrentQueue<LmsScan2>();
        System.Collections.Concurrent.ConcurrentQueue<NmeaSentence> gpsReadingsQueue = new System.Collections.Concurrent.ConcurrentQueue<NmeaSentence>();
        NmeaSentence tempSentence;
        public void run()
        {
            try
            {
                scan = rscanner.read();

                if (scan != null)
                {
                    RightScans.Enqueue(scan);
                    if (rScanDict.ContainsKey(scan.second))
                    {
                        rScanDict[scan.second].Add(scan);
                    }
                    else
                    {
                        rScanDict.Add(scan.second, new List<LmsScan2>());
                        rScanDict[scan.second].Add(scan);
                    }
                }
                scan = null;
            }
            catch
            {
                throw;
            }
            try
            {
                
                scan = lscanner.read();
                if (scan != null)
                {
                    LeftScans.Enqueue(scan);
                    if (lScanDict.ContainsKey(scan.second))
                    {
                        lScanDict[scan.second].Add(scan);
                    }
                    else
                    {
                        lScanDict.Add(scan.second, new List<LmsScan2>());
                        lScanDict[scan.second].Add(scan);
                    }
                }
            }
            catch
            {
                throw;
            }

            try
            {
                gps.read();
                
            }
            catch
            {
                throw;
            }
        }
        public ScanRunner4(ScanningOptions options, bool fakeReadings, bool saveReadings, LMS291_2 LScanner, LMS291_2 RScanner, NmeaReader GPS)
        {
            LaserScanUtilities.min_height = options.minHeight;
            LaserScanUtilities.max_distance = options.rowDistance;
            LaserScanUtilities.laserHeight = options.laserHeight;

            Options = options;


            rscanner = RScanner;
            lscanner = LScanner;
            gps = GPS;

            if(lscanner != null)
                lscanner.saveScans = saveReadings;
            if(rscanner != null)
                rscanner.saveScans = saveReadings;

            List<LmsScan> lScans = new List<LmsScan>();
            List<LmsScan> rScans = new List<LmsScan>();
            List<pointXYZ> gpsReadings = new List<pointXYZ>();
            List<int> times = new List<int>();

            path = new Path2(options, fakeReadings, saveReadings, date);
            path.LeftActive = options.useLeftLaser;
            path.RightActive = options.useRightLaser;

            if(gps != null)
                gps.readings = new System.Collections.Concurrent.ConcurrentQueue<NmeaSentence>();
            if(rscanner != null)
                rscanner.Reset();
            if (lscanner != null)
            {
                lscanner.Reset();

                lscanner.stopWatch.Restart();
            }
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
            }
        }
        public pointXYZ parseGPSReading(NmeaSentence gpsReading)
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

        public void Stop()
        {
            NmeaSentence sentence;
            NmeaSentence nextSentence;
            int count = 0;
            pointXYZ scan;
            Save();
            while(gps.readings.TryDequeue(out sentence))
            {
                GPSReadings.Add(sentence);
                if(count % 5 == 0 && (scan = parseGPSReading(sentence)) != null)
                    path.addPoint(scan);
                count++;
                if (!gps.readings.TryPeek(out nextSentence))
                {
                    if ((count - 1) % 5 != 0)
                    {
                        path.addPoint(parseGPSReading(sentence));
                    }
                }
            }

            path.LeftScanDict = lScanDict;
            path.RightScanDict = rScanDict;
            path.Close();
        }
        public void Save()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(@"c:\users\public\" + string.Format("right{0}.bin", date), FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, rScanDict);
            stream = new FileStream(@"c:\users\public\" + string.Format("left{0}.bin", date), FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, lScanDict);
            stream = new FileStream(@"c:\users\public\" + string.Format("gps{0}.bin", date), FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, GPSReadings);
            
        }

        public void LoadSavedScans(string gpsloc, string leftloc, string rightloc)
        {
            if (leftloc != "")
                Options.useLeftLaser = true;
            else
                Options.useLeftLaser = false;
            if (rightloc != "")
                Options.useRightLaser = true;
            else
                Options.useRightLaser = false;

            IFormatter formatter = new BinaryFormatter();

            Stream stream = new FileStream(leftloc, FileMode.Open, FileAccess.Read, FileShare.None);
            lScanDict = (Dictionary<int, List<LmsScan2>>)formatter.Deserialize(stream);

            stream = new FileStream(rightloc, FileMode.Open, FileAccess.Read, FileShare.None);
            rScanDict = (Dictionary<int, List<LmsScan2>>)formatter.Deserialize(stream);

            stream = new FileStream(gpsloc, FileMode.Open, FileAccess.Read, FileShare.None);
            GPSReadings = (List<NmeaSentence>)formatter.Deserialize(stream);
        }

        public void ProcessSavedScans()
        {
            for (int i = 0; i < GPSReadings.Count; i++)
            {
                if(i % 5 == 0)
                    path.addPoint(parseGPSReading(GPSReadings[i]));
            }
            if ((GPSReadings.Count - 1) % 5 != 0)
            {
                path.addPoint(parseGPSReading(GPSReadings[GPSReadings.Count - 1]));
            }
            path.RightScanDict = rScanDict;
            path.LeftScanDict = lScanDict;
            path.Close();
        }

        public void SaveRunner()
        {

        }
    }
}
