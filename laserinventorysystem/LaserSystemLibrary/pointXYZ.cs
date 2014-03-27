using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserSystemLibrary
{
    [Serializable]
    public class pointXYZ
    {
        public double x;
        public double y;
        public double z;
        public double t;
        public double latitude;
        public double longitude;
        public pointXYZ(double x, double y, double z, double t)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.t = t;
        }
        public pointXYZ()
        {
        }

        public pointXYZ(pointXYZ pointXYZ1)
        {
            // TODO: Complete member initialization
            this.t = pointXYZ1.t;
            this.x = pointXYZ1.x;
            this.y = pointXYZ1.y;
            this.z = pointXYZ1.z;
        }
    }

    
}
