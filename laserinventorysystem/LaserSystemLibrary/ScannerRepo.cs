using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
namespace LaserSystemLibrary
{
    public static class ScannerRepo
    {
        public static ACS430 TopLeftACS;
        public static ACS430 BottomLeftACS;
        public static ACS430 TopRightACS;
        public static ACS430 BottomRightACS;
        public static LMS291 LeftLMS;
        public static LMS291 RightLMS;
        public static NmeaReader GPS;

        public static List<SerialPort> serialPorts;
    }
}
