using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LaserSystemLibrary
{
    class GPSPath
    {
        List<pointXYZ> points = new List<pointXYZ>();

        public GPSPath()
        {

        }
        
        public void AddPoint(pointXYZ point)
        {
            points.Add(point);
            
        }

        public GPSVector GetPointAtTime(double time)
        {
            if (points.Count < 2)
                return null;
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].t > time)
                {
                    pointXYZ p1 = points[i - 1];
                    pointXYZ p2 = points[i];
                    pointXYZ delta = new pointXYZ(p2.x - p1.x, 
                                                  p2.y - p1.y, 
                                                  p2.z - p1.z, 
                                                  p2.t - p1.t);
                    double offset = time - p1.t;
                    double offsetPct = offset / delta.t;

                    pointXYZ pfinal =  new pointXYZ(p1.x + delta.x * offsetPct,
                                                    p1.y + delta.y * offsetPct,
                                                    p1.z + delta.z * offsetPct,
                                                    time);
                    double speed = TwoDimensionalDistance(p1, p2) / delta.t; // meters per ... millisecond?
                    double angle = Math.Atan2(delta.y, delta.x) * 180 / Math.PI;

                    return new GPSVector(pfinal, speed, angle);
                }
            }
            //LaserScanUtilities.TickOffset tickOffset = LaserScanUtilities.CalculateTickOffset(tick.xySlope, scanningOptions.rowDistance * FEET_TO_METERS, pointXYZ1, pointXYZ2);
            return null;
        }

        double TwoDimensionalDistance(pointXYZ p1, pointXYZ p2)
        {
            return Math.Sqrt(Math.Pow((p2.x - p1.x), 2)+ Math.Pow((p2.y - p1.y), 2));
        }

        

        
    }
}
