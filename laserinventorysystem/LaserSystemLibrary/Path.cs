using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
namespace LaserSystemLibrary
{
    public class Path
    {
        List<ScanLocation> LeftTicks = new List<ScanLocation>();
        List<ScanLocation> RightTicks = new List<ScanLocation>();
        List<ScanLocation> LeftTicksSave = new List<ScanLocation>();
        List<ScanLocation> RightTicksSave = new List<ScanLocation>();

        List<LmsScan> lmsRightScans = new List<LmsScan>();
        List<LmsScan> lmsLeftScans = new List<LmsScan>();
        List<LmsScan> lmsRightScansSave = new List<LmsScan>();
        List<LmsScan> lmsLeftScansSave = new List<LmsScan>();
        public bool LeftActive = true;
        public bool RightActive = true;
        double offset;
        double TotalDistance = 0;
        bool FakeReadings;
        int totalNumberOfTicks = 0;
        Line line = new Line();
        double SamplingDistance;
        ScanningOptions scanningOptions;
        ShapefileWriter gw;
        bool SaveReadings = false;
        string date = DateTime.Now.ToString("MMMM-dd-yyyyHmmss");
        int filesWritten = 0;
        double FEET_TO_METERS = 0.3048;
        ShapefileWriter points;
        public void SerializeLeftScans(string tag)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(@"c:\users\public\" + string.Format("left{0}.bin", tag), FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, lmsLeftScansSave);
        }

        public void SerializeRightScans(string tag)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(@"c:\users\public\" + string.Format("right{0}.bin", tag), FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, lmsRightScansSave);
        }

        public void SerializeGPSPoints(string tag)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(@"c:\users\public\" + string.Format("gps{0}.bin", tag), FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, line.Points);
            
        }
        public Path(ScanningOptions options, bool fakeReadings, bool saveReadings) 
        {
            FakeReadings = fakeReadings;
            this.SamplingDistance = options.samplingDistance;
            scanningOptions = options;
            gw = new ShapefileWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + date + options.OutputFileName + ".shp");
            points = new ShapefileWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + date + options.OutputFileName + "points.shp");
            LaserScanUtilities.FakeScans = fakeReadings;
            LaserScanUtilities.calculateLowestAngle(options.rowDistance, options.laserHeight);
            SaveReadings = saveReadings;


            
        }
        
        public void addPoint(pointXYZ pointXYZ)
        {   
            //pointXYZ.x += line.Points.Count * 1.5;
            line.Points.Add(pointXYZ);
            if (line.Points.Count >= 10000 & SaveReadings)
            {
                SerializeGPSPoints(date + filesWritten.ToString());
                line.Points.RemoveRange(0, line.Points.Count - 2);
                SerializeLeftScans(date + filesWritten.ToString());
                SerializeRightScans(date + filesWritten.ToString());
                filesWritten++;
            }
            else if (line.Points.Count >= 10000)
                line.Points.RemoveRange(0, line.Points.Count - 2);

            int index = line.Points.Count - 1;
            
            if (line.Points.Count > 1)
            {
                AddNewTicks(line.Points[index - 1], line.Points[index]);
            }
        }

        private void AddNewTicks(pointXYZ pointXYZ1, pointXYZ pointXYZ2)
        {
            //Console.WriteLine("adding a new tick!");
            //ScanLocation lastTick = ticks[ticks.Count - 1];
            ScanLocation tick;
            LineSegment lineSegment = new LineSegment(pointXYZ1, pointXYZ2);
            double length = lineSegment.getLength();
            double NumberOfTicks = Math.Floor(((TotalDistance + length) - totalNumberOfTicks * SamplingDistance) / SamplingDistance);

            //Console.WriteLine("length: {0}", length);
            //Console.WriteLine("adding {0} ticks!", NumberOfTicks);
            if (totalNumberOfTicks == 0)
            {
                tick = lineSegment.GetTickAtDistance(0);
                ScanLocation leftloc = new ScanLocation();
                LaserScanUtilities.TickOffset offset = LaserScanUtilities.CalculateTickOffset(tick.xySlope, scanningOptions.rowDistance * 0.3048);
                leftloc.tick = tick.tick;

                pointXYZ tempPoint = new pointXYZ();
                tempPoint.t = tick.point.t;
                tempPoint.x = tick.point.x - offset.X;
                tempPoint.y = tick.point.y - offset.Y;
                tempPoint.z = tick.point.z;
                leftloc.point = tempPoint;
                LeftTicks.Add(leftloc);
                if (SaveReadings)
                    LeftTicksSave.Add(leftloc);
                
                tempPoint = new pointXYZ();
                tempPoint.t = tick.point.t;
                tempPoint.x = tick.point.x + offset.X;
                tempPoint.y = tick.point.y + offset.Y;
                tempPoint.z = tick.point.z;
                ScanLocation rightloc = new ScanLocation();
                rightloc.tick = tick.tick;
                rightloc.point = tempPoint;
                RightTicks.Add(rightloc);
                if (SaveReadings)
                    RightTicksSave.Add(rightloc);

                totalNumberOfTicks++;
            }
            for (int i = 1; i <= NumberOfTicks; i++)
            {
                //Console.WriteLine("Point2 tick value: {0}", pointXYZ2.t);
                tick = lineSegment.GetTickAtDistance(i * SamplingDistance - offset);
                
                //Console.WriteLine("Tick x: {0}, y: {1}", tick.point.x, tick.point.y);
                //Console.WriteLine("calculated tick value: {0}", tick.tick);
                if (tick.tick < pointXYZ1.t | tick.tick > pointXYZ2.t)
                {
                    throw new Exception();
                }
                ScanLocation leftloc = new ScanLocation();
                LaserScanUtilities.TickOffset tickOffset = LaserScanUtilities.CalculateTickOffset(tick.xySlope, scanningOptions.rowDistance * FEET_TO_METERS);
                leftloc.tick = tick.tick;

                pointXYZ tempPoint = new pointXYZ();
                tempPoint.t = tick.point.t;
                tempPoint.x = tick.point.x - tickOffset.X;
                tempPoint.y = tick.point.y - tickOffset.Y;
                tempPoint.z = tick.point.z;
                leftloc.point = tempPoint;
                LeftTicks.Add(leftloc);

                tempPoint = new pointXYZ();
                tempPoint.t = tick.point.t;
                tempPoint.x = tick.point.x + tickOffset.X;
                tempPoint.y = tick.point.y + tickOffset.Y;
                tempPoint.z = tick.point.z;
                ScanLocation rightloc = new ScanLocation();
                rightloc.tick = tick.tick;
                rightloc.point = tempPoint;
                RightTicks.Add(rightloc);
                totalNumberOfTicks++;
            }
            TotalDistance += length;
            offset = TotalDistance % SamplingDistance;
        }

        public void AddRightScan(LmsScan scan)
        {
            lmsRightScans.Add(scan);
            if (SaveReadings)
                lmsRightScansSave.Add(scan);
        }
        public void AddLeftScan(LmsScan scan)
        {
            lmsLeftScans.Add(scan);
            if (SaveReadings)
                lmsLeftScansSave.Add(scan);
        }
        

        public void removeScansBeforeTime(double p, bool rightScan)
        {
            if (rightScan)
            {
                for (int i = 0; i < lmsRightScans.Count; i++)
                {
                    if (lmsRightScans[i].milliseconds < p)
                    {
                        lmsRightScans.Remove(lmsRightScans[i]);
                    }
                }
            }
        }

        public LmsScan GetClosestScan(ScanLocation tick, List<LmsScan> lmsScans)
        {
            double difference = 10000000;
            List<LmsScan> duplicates = new List<LmsScan>();
            foreach(LmsScan scan in lmsScans)
            {
                if(Math.Abs(scan.milliseconds - tick.tick) < difference)
                {
                    duplicates = new List<LmsScan>();
                    duplicates.Add(scan);
                    difference = Math.Abs(scan.milliseconds - tick.tick);
                }
                else if (Math.Abs(scan.milliseconds - tick.tick) == difference)
                {
                    if (scan.milliseconds == duplicates[0].milliseconds)
                    {
                        duplicates.Add(scan);
                    }
                    if (scan.milliseconds > duplicates[0].milliseconds)
                    {
                        duplicates = new List<LmsScan>();
                        duplicates.Add(scan);
                    }
                }
            }
            return duplicates[0];
        }

        public void ProcessScans()
        {
            List<GeoreferencedScan> rightGeoreferencedScans = MatchTicksWithScans(RightTicks, lmsRightScans, false);
            List<GeoreferencedScan> leftGeoreferencedScans = MatchTicksWithScans(LeftTicks, lmsLeftScans, true);
            gw.writeBatch(rightGeoreferencedScans);
            gw.writeBatch(leftGeoreferencedScans);
        }

        private List<GeoreferencedScan> MatchTicksWithScans(List<ScanLocation> ticks, List<LmsScan> lmsScans, bool LeftScan)
        {
            List<GeoreferencedScan> scansToWrite = new List<GeoreferencedScan>();


            if (lmsScans.Count > 0)
            {

            }
            if (ticks.Count > 0)
                //Console.WriteLine("rscan milliseconds: {0}", ticks.Last().tick / Stopwatch.Frequency);
            //Console.WriteLine("second!: {0}", second);
            if (lmsScans.Count >= 75)
            {
                while (ticks.Count > 0 && Convert.ToDouble(ticks[0].tick) / Stopwatch.Frequency * 1000 < lmsScans[74].milliseconds)
                {
                    //Console.WriteLine("Ticks milliseconds: {0}, scan milliseconds: {1}", Convert.ToDouble(ticks[0].tick) / Stopwatch.Frequency * 1000, lmsRightScans[74].milliseconds);
                    int closestIndex = 0;
                    // set to an arbitrarily large number
                    double closestDistance = 10000;
                    double tickMilliseconds = Convert.ToDouble(ticks[0].tick) * 1000.0 / Stopwatch.Frequency;
                    for (int i = 0; i < 75; i++)
                    {
                        if (Math.Abs(tickMilliseconds - lmsScans[i].milliseconds) < closestDistance)
                        {
                            closestIndex = i;
                            closestDistance = Math.Abs(tickMilliseconds - lmsScans[i].milliseconds);
                        }
                    }

                    GeoreferencedScan geoscan = new GeoreferencedScan();
                    geoscan.Point = ticks[0].point;
                    geoscan.scanResults = LaserScanUtilities.GetScanInfo(lmsScans[closestIndex].buffer, LeftScan);
                        
                    //Console.WriteLine("Scan results Density: {0}, Skirtheight: {1}, treeHeight:{2}", geoscan.scanResults.Density, geoscan.scanResults.SkirtHeight, geoscan.scanResults.TreeHeight);
                    scansToWrite.Add(geoscan);
                    ticks.RemoveAt(0);

                };
                lmsScans.RemoveRange(0, 75);

            }

            return scansToWrite;
        }
        public void Close()
        {
            if (SaveReadings)
            {
                this.SerializeGPSPoints(date + scanningOptions.OutputFileName);
                this.SerializeLeftScans(date + scanningOptions.OutputFileName);
                this.SerializeRightScans(date + scanningOptions.OutputFileName);
            }
            while (lmsRightScans.Count >= 75 & lmsLeftScans.Count >= 75 & (LeftTicks.Count > 0 | RightTicks.Count > 0))
                ProcessScans();
            gw.close();

        }

        public void LoadSavedScans(string gpsloc, string leftloc, string rightloc)
        {
            if (leftloc != "")
                scanningOptions.useLeftLaser = true;
            else
                scanningOptions.useLeftLaser = false;
            if (rightloc != "")
                scanningOptions.useRightLaser = true;
            else
                scanningOptions.useRightLaser = false;

            IFormatter formatter = new BinaryFormatter();

            Stream stream = new FileStream(leftloc, FileMode.Open, FileAccess.Read, FileShare.None);
            lmsLeftScans = (List<LmsScan>)formatter.Deserialize(stream);

            stream = new FileStream(rightloc, FileMode.Open, FileAccess.Read, FileShare.None);
            lmsRightScans = (List<LmsScan>)formatter.Deserialize(stream);

            stream = new FileStream(gpsloc, FileMode.Open, FileAccess.Read, FileShare.None);
            List<pointXYZ> temppoints = (List<pointXYZ>)formatter.Deserialize(stream);

            foreach(pointXYZ point in temppoints)
            {
                addPoint(point);
            }
        }

        public int ProcessSavedScans()
        {
            if (lmsRightScans.Count >= 75 & lmsLeftScans.Count >= 75)
                ProcessScans();
            else
            {
                gw.close();
                return 0;
            }
            return lmsRightScans.Count;
        }
        
    }
}
