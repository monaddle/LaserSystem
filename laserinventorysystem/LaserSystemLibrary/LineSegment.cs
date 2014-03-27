using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserSystemLibrary
{
    public class LineSegment
    {
        private pointXYZ p1;
        private pointXYZ p2;

        private double m1;
        private double b1;
        private double m2;
        private double b2;
        private double m3;
        private double b3;

        public LineSegment(pointXYZ P1, pointXYZ P2)
        {
            // TODO: Complete member initialization
            this.p1 = P1;
            this.p2 = P2;
            m1 = calculateM(p2.x, p1.x, p2.t, p1.t);
            b1 = calculateB(p1.x, p1.t, m1);
            m2 = calculateM(p2.y, p1.y, p2.t, p1.t);
            b2 = calculateB(p1.y, p1.t, m2);
            m3 = calculateM(p2.z, p1.z, p2.t, p1.t);
            b3 = calculateB(p1.z, p1.t, m3);

            
        }

        private double calculateB(double x, double t, double m)
        {
            return t - m * x;
        }

        private double calculateM(double x2, double x1, double t2, double t1)
        {
            return (t1 - t2) / (x1 - x2);
        }

        public pointXYZ GetPointFromTValue(double tick)
        {
            pointXYZ point = new pointXYZ(getX(tick), getY(tick), getZ(tick), tick);

            return point;


        }

        public double GetTFromPoint(pointXYZ point)
        {
            return m1 * point.x + b1;
        }

        private double getX(double tick)
        {
            if (m1 == double.NegativeInfinity)
            {
                return p1.x;
            }
            return (tick - b1) / m1;
        }

        private double getY(double tick)
        {
            if (m2 == double.NegativeInfinity)
            {
                return p1.y;
            }
            return (tick - b2) / m2;
        }

        private double getZ(double tick)
        {
            if (m3 == double.NegativeInfinity)
            {
                return p1.z;
            }
            return (tick - b3) / m3;
        }

        public double getLength()
        {
            double length = Math.Sqrt(
                  Math.Pow(p2.x - p1.x, 2) 
                + Math.Pow(p2.y - p1.y, 2) 
                + Math.Pow(p2.z - p1.z, 2));
            return length;
        }

        internal ScanLocation GetTickAtDistance(double distance)
        {
            ScanLocation a = new ScanLocation();
            double length = getLength();
            double pctDistance = distance / length;
            double tick = ((p2.t - p1.t) * pctDistance) + p1.t;
            
            a.tick = tick;
            a.point = GetPointFromTValue(tick);
            a.point.latitude = p1.latitude + (p2.latitude - p1.latitude) * pctDistance;
            a.point.longitude = p1.longitude + (p2.longitude - p1.longitude) * pctDistance;
            a.xySlope = (p2.y - p1.y) / (p2.x - p1.x);
            return a;

        }
    }
}
