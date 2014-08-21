using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.Generic;
using DotSpatial.Data;
namespace LaserSystemLibrary
{
    public class Path2
    {
        public List<ScanLocation> LeftTicks = new List<ScanLocation>();
        public List<ScanLocation> RightTicks = new List<ScanLocation>();
        List<ScanLocation> LeftTicksSave = new List<ScanLocation>();
        List<ScanLocation> RightTicksSave = new List<ScanLocation>();
        public Dictionary<int, List<LmsScan2>> LeftScanDict;
        public Dictionary<int, List<LmsScan2>> RightScanDict;
        List<LmsScan> lmsRightScans = new List<LmsScan>();
        List<LmsScan> lmsLeftScans = new List<LmsScan>();
        List<LmsScan> lmsRightScansSave = new List<LmsScan>();
        List<LmsScan> lmsLeftScansSave = new List<LmsScan>();
        List<GeoreferencedScan> GeoScans = new List<GeoreferencedScan>();
        List<GeoreferencedScan> RightGeoScans = new List<GeoreferencedScan>();
        public Queue<ScanLocation> LeftTickQueue = new Queue<ScanLocation>();
        public Queue<ScanLocation> RightTickQueue = new Queue<ScanLocation>();
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
        LocationService LocationServiceObject;
        bool UsePolygonLayerForConstraint;
        public Path2(ScanningOptions options, bool fakeReadings, bool saveReadings, string tag)
        {
            this.UsePolygonLayerForConstraint = options.UsePolygonLayer;
            this.date = tag;
            LocationServiceObject = new LocationService("SOURCEPOLYGON.shp");
            FakeReadings = fakeReadings;
            this.SamplingDistance = options.samplingDistance * FEET_TO_METERS;
            scanningOptions = options;
            LaserScanUtilities.FakeScans = fakeReadings;
            LaserScanUtilities.calculateLowestAngle(options.rowDistance, options.laserHeight);
            SaveReadings = saveReadings;
        }

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
            ScanLocation tick;
            LineSegment lineSegment = new LineSegment(pointXYZ1, pointXYZ2);
            double length = lineSegment.getLength();
            double NumberOfTicks = Math.Floor(((TotalDistance + length) - totalNumberOfTicks * SamplingDistance) / SamplingDistance);

            if (totalNumberOfTicks == 0)
            {
                tick = lineSegment.GetTickAtDistance(0);
                ScanLocation leftloc = new ScanLocation();
                LaserScanUtilities.TickOffset offset = LaserScanUtilities.CalculateTickOffset(tick.xySlope, scanningOptions.rowDistance * FEET_TO_METERS, pointXYZ1, pointXYZ2);
                leftloc.tick = tick.tick;

                pointXYZ tempPoint = new pointXYZ();
                tempPoint.t = tick.point.t;
                tempPoint.x = tick.point.x - offset.X;
                tempPoint.y = tick.point.y - offset.Y;
                tempPoint.z = tick.point.z;
                leftloc.point = tempPoint;
                if (LeftActive)
                    LeftTicks.Add(leftloc);
                if (SaveReadings)
                    LeftTicksSave.Add(leftloc);
                LeftTickQueue.Enqueue(leftloc);
                tempPoint = new pointXYZ();
                tempPoint.t = tick.point.t;
                tempPoint.x = tick.point.x + offset.X;
                tempPoint.y = tick.point.y + offset.Y;
                tempPoint.z = tick.point.z;
                ScanLocation rightloc = new ScanLocation();
                rightloc.tick = tick.tick;
                rightloc.point = tempPoint;
                
                if(RightActive)
                    RightTicks.Add(rightloc);
                if (SaveReadings)
                    RightTicksSave.Add(rightloc);
                RightTickQueue.Enqueue(rightloc);
                totalNumberOfTicks++;
            }
            for (int i = 1; i <= NumberOfTicks; i++)
            {
                string HarBlkId;
                totalNumberOfTicks++;
                Feature feature;
                //Console.WriteLine("Point2 tick value: {0}", pointXYZ2.t);
                tick = lineSegment.GetTickAtDistance(i * SamplingDistance - offset);
                if (UsePolygonLayerForConstraint)
                {
                    DotSpatial.Topology.Point p = new DotSpatial.Topology.Point(tick.point.x, tick.point.y);
                    Feature point = new Feature(p);
                    feature = LocationServiceObject.GetLocation(point, tick.point.x, tick.point.y);
                    if (feature == null)
                    {
                        continue;
                    }
                    HarBlkId = Convert.ToString(feature.DataRow["HARBLKID"]);

                }
                else
                    HarBlkId = "";
                
                //Console.WriteLine("Tick x: {0}, y: {1}", tick.point.x, tick.point.y);
                //Console.WriteLine("calculated tick value: {0}", tick.tick);
                if (tick.tick < pointXYZ1.t | tick.tick > pointXYZ2.t)
                {
                    throw new Exception();
                }
                ScanLocation leftloc = new ScanLocation();
                leftloc.HarBlkID = HarBlkId;
                LaserScanUtilities.TickOffset tickOffset = LaserScanUtilities.CalculateTickOffset(tick.xySlope, scanningOptions.rowDistance * FEET_TO_METERS, pointXYZ1, pointXYZ2);

                leftloc.tick = tick.tick;

                pointXYZ tempPoint = new pointXYZ();
                tempPoint.t = tick.point.t;
                tempPoint.x = tick.point.x + tickOffset.X;
                tempPoint.y = tick.point.y + tickOffset.Y;
                tempPoint.z = tick.point.z;
                leftloc.point = tempPoint;
                if(LeftActive)
                    LeftTicks.Add(leftloc);
                LeftTickQueue.Enqueue(leftloc);
                tempPoint = new pointXYZ();
                tempPoint.t = tick.point.t;
                tempPoint.x = tick.point.x - tickOffset.X;
                tempPoint.y = tick.point.y - tickOffset.Y;
                tempPoint.z = tick.point.z;
                ScanLocation rightloc = new ScanLocation();
                rightloc.HarBlkID = HarBlkId;
                rightloc.tick = tick.tick;
                rightloc.point = tempPoint;
                RightTickQueue.Enqueue(rightloc);
                if(RightActive)
                    RightTicks.Add(rightloc);
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

        //public void ProcessScans()
        //{
        //    List<GeoreferencedScan> rightGeoreferencedScans = MatchTicksWithScans(RightTicks, lmsRightScans, false);
        //    List<GeoreferencedScan> leftGeoreferencedScans = MatchTicksWithScans(LeftTicks, lmsLeftScans, true);
        //    gw.writeBatch(rightGeoreferencedScans);
        //    gw.writeBatch(leftGeoreferencedScans);
        //}

        public void Close()
        {

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

        public void ProcessScans()
        {
            PairTicksWithScans(LeftScanDict, LeftTicks, true);
            PairTicksWithScans(RightScanDict, RightTicks, false);
            gw.WriteBatchWithoutSave(GeoScans);

            //WriteLeftTicks();

        }
        public void WriteLeftTicks()
        {
            gw.writePoints(LeftTicks);
        }

        private void PairTicksWithScans(Dictionary<int, List<LmsScan2>> scanDict, List<ScanLocation> Ticks, bool leftScan)
        {
            foreach (KeyValuePair<int, List<LmsScan2>> item in scanDict)
            {
                Console.WriteLine("{0}:{1}, ", item.Key, item.Value.Count);
            }
            int second;
            double interval;
            double tickMilliseconds;
            List<LmsScan2> currentList;
            double closestTimeInterval;
            int closestTimeIndex;
            // assign normalized times to each scan
            foreach (KeyValuePair<int, List<LmsScan2>> item in scanDict)
            {
                interval = 1.0 / item.Value.Count;
                for (int i = 0; i < item.Value.Count; i++)
                {
                    item.Value[i].milliseconds = interval * i * 1000;
                }
            }
            foreach (ScanLocation tick in Ticks)
            {
                
                second = (int)(tick.point. t/ (double)Stopwatch.Frequency);
                if(scanDict.ContainsKey(second))
                    currentList = scanDict[second];
                else break;
                tickMilliseconds = (tick.point.t / Stopwatch.Frequency - second) * 1000;

                closestTimeIndex = 0;
                closestTimeInterval = Math.Abs(tickMilliseconds - currentList[0].milliseconds);
                foreach (LmsScan2 scan in currentList)
                {
                    if (closestTimeInterval > Math.Abs(tickMilliseconds - scan.milliseconds))
                    {
                        closestTimeInterval = Math.Abs(tickMilliseconds - scan.milliseconds);
                        closestTimeIndex = currentList.IndexOf(scan);
                    }
                }

                GeoreferencedScan geoscan = new GeoreferencedScan();
                geoscan.Point = tick.point;
                geoscan.scanResults = currentList[closestTimeIndex].scanResults;
                //geoscan.scanResults = LaserScanUtilities.GetScanInfo(currentList[closestTimeIndex].buffer, leftScan);
                geoscan.timestamp = currentList[closestTimeIndex].timestamp;
                string scanID = leftScan ? "L" : "R";
                scanID += second.ToString() + "," + closestTimeIndex.ToString();
                geoscan.scanID = scanID;
                GeoScans.Add(geoscan);

            }
        }
    }
}
