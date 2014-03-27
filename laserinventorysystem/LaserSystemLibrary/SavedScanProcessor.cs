using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserSystemLibrary
{
    public class SavedScanProcessor
    {
        public string GPSPath { get; set; }
        public string LeftScansPath { get; set; }
        public string RightScansPath { get; set; }

        public SavedScanProcessor(string gpsPath, string leftScansPath, string rightScansPath)
        {
            GPSPath = gpsPath;
            LeftScansPath = leftScansPath;
            RightScansPath = rightScansPath;
        }
    }
    
}
