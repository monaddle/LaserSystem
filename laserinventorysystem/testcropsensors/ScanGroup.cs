using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserSystemLibrary;
namespace testcropsensors
{
    class ScanGroup
    {
        public LmsScan2 LMSScan;
        public ACS430Reading TopReading;
        public ACS430Reading BottomReading;

        public ScanGroup(LmsScan2 lmsScan, ACS430Reading topReading, ACS430Reading bottomReading)
        {
            LMSScan = lmsScan;
            TopReading = topReading;
            BottomReading = bottomReading;
        }

        public ScanGroup() { }


    }
}
