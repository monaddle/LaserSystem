using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserSystemLibrary
{   
    [Serializable]
    public class LmsScan
    {
        public DateTime timestamp { get; set; }
        public Byte[] buffer { get; set; }
        public double calculatedMilliseconds {get; set;}
        public double milliseconds {get; set;}
        public int second { get; set; }
        public LmsScan()
        {
            buffer = new Byte[512];
        }
    }

    [Serializable]
    public class LmsScan2
    {
        public DateTime timestamp { get; set; }
        public double calculatedMilliseconds { get; set; }
        public double milliseconds { get; set; }
        public int second { get; set; }
        public ScanResults scanResults;
        public Byte[] buffer;
        public LmsScan2()
        {
            //buffer = new byte[512];
        }
    }
}
