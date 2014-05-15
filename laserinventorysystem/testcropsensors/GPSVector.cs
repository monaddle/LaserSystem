using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserSystemLibrary;
namespace testcropsensors
{
    class GPSVector
    {
        public pointXYZ Location;
        public double Speed;
        public double Direction;
        public GPSVector(pointXYZ location, double speed, double direction)
        {
            Location = location;
            Speed = speed;
            Direction = direction;
        }
    }
}
