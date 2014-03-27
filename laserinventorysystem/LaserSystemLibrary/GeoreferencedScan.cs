using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LaserSystemLibrary
{
    public class GeoreferencedScan
    {
        public double tick;
        public DateTime timestamp;
        public ScanResults scanResults;
        public pointXYZ Point;
        public string scanID;
        public GeoreferencedScan()
        {

        }

    }
}
