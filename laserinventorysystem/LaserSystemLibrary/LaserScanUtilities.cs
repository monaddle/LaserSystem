using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserSystemLibrary
{

    public static class LaserScanUtilities
    {
        static public double METER_TO_FEET = 3.28084;
        static public bool FakeScans = false;
        static public double laserHeight = 8;
        static public double max_distance = 8;
        static public double min_height = 1;
        static public double max_scan_length = 26;
        static public int lowest_angle = -60;
        static public double max_height = 16.5;
        static public double height(double hypotenuse, double degree)
        {
            double heightUnadjusted = Math.Sin(degreeToRadian(degree)) * hypotenuse;
            double height = heightUnadjusted + laserHeight;
            return height;
        }
        static public void calculateLowestAngle (double rowWidth, double tractorHeight)
        {
            double angle = Math.Atan(tractorHeight/rowWidth);
            angle = angle / Math.PI * 180.0;
            lowest_angle = (int)Math.Ceiling(90 + angle);
            
        }
        static public double distance(double hypotenuse, double degree)
        {
            double distance =  Math.Cos(degreeToRadian(degree)) * hypotenuse;
            return distance;
        }

        public static double degreeToRadian(double degree)
        {
            return Math.PI * degree / 180.0;
        }

        public static double millimetersToFeet(double millimeters)
        {
            return millimeters * 0.00328084;
        }

        public static ScanResults GetScanInfo(byte[] scan, bool LeftScan)
        {
            double[] scans = new double[181];
            int j = 0;
            //Console.WriteLine();
            ScanResults scanInfo = new ScanResults();

            for (int i = 8; i < 181 * 2 + 8; i += 2)
            {
                byte[] arry = { scan[i + 1], scan[i] };
                int myScanValue;
                myScanValue = BitConverter.ToInt16(arry, 0);
                scans[j] = myScanValue / 1000.0 * METER_TO_FEET;
                j++;
            }
            //Console.WriteLine();
            if (!LeftScan)
            {

                Array.Reverse(scans);

            }
            scanInfo.distances = scans;
            
            //Console.WriteLine(String.Join(", ", scanInfo.distances));

            int skirtIndex = -1;
            int treeEndIndex = -1;
            double density;
            // find the skirt
            skirtIndex = CalculateSkirtHeight(scans);
            if (skirtIndex == -1)
            {
                scanInfo.Density = 0;
                scanInfo.SkirtHeight = 0;
                scanInfo.TreeHeight = 0;

                return scanInfo;
            }
            treeEndIndex = CalculateTreeTop(scans);

            density = CalculateDensity(scans, lowest_angle, treeEndIndex);

            scanInfo.Density = density;
            scanInfo.SkirtHeight = height(scans[skirtIndex], 90 - skirtIndex);
            scanInfo.TreeHeight = height(scans[treeEndIndex], 90 - treeEndIndex);

            //Console.WriteLine("Density: {0}, skirtheight: {1}, treeheight: {2}", scanInfo.Density, scanInfo.SkirtHeight, scanInfo.TreeHeight);
            if (FakeScans)
            {
                scanInfo.Density = .7;
                scanInfo.SkirtHeight = 4;
                scanInfo.TreeHeight = 8;
            }
            return scanInfo;
        }

        public static double CalculateDensity(double[] scans, int skirtIndex, int treeEndIndex)
        {
            if (skirtIndex - treeEndIndex > 100)
            {
                treeEndIndex = skirtIndex - 100;
            }
            if (skirtIndex == -1 | treeEndIndex == -1)
                return double.NaN;
            double density;
            int hitCount = 0;
            for (int i = skirtIndex; i > treeEndIndex; i--)
            {
                if (distance(scans[i], 90 - i) < max_distance && height(scans[i], 90 - i) > min_height & scans[i] < max_scan_length)
                {
                    hitCount++;
                }
            }

            density = Convert.ToDouble(hitCount) / Convert.ToDouble(skirtIndex - treeEndIndex);
            return density;
        }

        public static int CalculateTreeTop(double[] scans)
        {
            int treeEndIndex = -1;
            for (int i = 20; i < scans.Length - 20; i++)
            {
                if (distance(scans[i], 90 - i) < max_distance && height(scans[i], 90 - i) > min_height & scans[i] < max_scan_length & height(scans[i], 90 - i) < max_height)
                {
                    treeEndIndex = i;
                    break;
                }
            }
            return treeEndIndex;
        }

        public static int CalculateSkirtHeight(double[] scans)
        {

            int skirtIndex = -1;
            for (int i = scans.Length - 21; i > 20; i--)
            {
                double calcheight = height(scans[i], 90 - i);
                double calcdistance = distance(scans[i], 90 - i);

                if (height(scans[i], 90 - i) > min_height && distance(scans[i], 90 - i) < max_distance & scans[i] < max_scan_length & height(scans[i], 90 - i) < max_height)
                {
                    skirtIndex = i;
                    break;
                }
            }
            return skirtIndex;
        }

        public class TickOffset
        {
            public double X;
            public double Y;
        }

        public static TickOffset CalculateTickOffset(double m1, double magnitude)
        {
            double m2 = (-1 / m1);
            double radians = Math.Atan(m2);
            TickOffset offset = new TickOffset();
            offset.X = Math.Cos(radians) * magnitude;
            offset.Y = Math.Sin(radians) * magnitude;
            return offset;
        }

        internal static TickOffset CalculateTickOffset(double m1, double magnitude, pointXYZ pointXYZ1, pointXYZ pointXYZ2)
        {
            double m2 = (-1 / m1);
            double radians = Math.Atan(m2);


            TickOffset offset = new TickOffset();
            offset.X = Math.Cos(radians) * magnitude;
            offset.Y = Math.Sin(radians) * magnitude;

            if (pointXYZ1.x - pointXYZ2.x > 0)
            {
                if (offset.Y > 0)
                {
                    offset.Y = -offset.Y;
                }
            }
            else
            {
                if (offset.Y < 0)
                {
                    offset.Y = -offset.Y;
                }
            }

            if (pointXYZ1.y - pointXYZ2.y > 0)
            {
                if (offset.X < 0)
                {
                    offset.X = -offset.X;
                }
            }
            else
            {
                if (offset.X > 0)
                {
                    offset.X = -offset.X;
                }
            }
            return offset;
        }

        internal static double GetClosestPoint(LmsScan2 scan, string left)
        {
            double[] scans = new double[181];
            int j = 0;
            //Console.WriteLine();
            ScanResults scanInfo = new ScanResults();
            var buffer = scan.buffer;
            for (int i = 8; i < 181 * 2 + 8; i += 2)
            {
                byte[] arry = { buffer[i + 1], buffer[i] };
                int myScanValue;
                myScanValue = BitConverter.ToInt16(arry, 0);
                scans[j] = myScanValue / 1000.0 * METER_TO_FEET;
                //Console.Write("{1}, {0},\n ", myScanValue / 1000.0 * METER_TO_FEET, -90 +  (i-8)/2);
                j++;
            }
            //Console.WriteLine();
            if (left != "left")
            {
                Array.Reverse(scans);
            }
            List<double> distances = new List<double>();
            for (int i = 21; i < scans.Length - 20; i++)
            {
                if (distance(scans[i], 90 - i) < max_distance && height(scans[i], 90 - i) > min_height & scans[i] < max_scan_length & height(scans[i], 90 - i) < max_height)
                {
                    distances.Add(max_distance - distance(scans[i], 90 - i));
                }
            }
            distances.Sort();
            int len = distances.Count < 3 ? distances.Count : 3;
            
            return len == 0 ? 0 : distances.GetRange(0, len).Average();
        }
    }
}
