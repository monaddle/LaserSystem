using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserSystemLibrary;
namespace LaserSystemLibrary
{
    public class ScanRepo
    {
        public List<LmsScan2> LMSScans = new List<LmsScan2>();
        public List<ACS430Reading> ACSTopReadings = new List<ACS430Reading>();
        public List<ACS430Reading> ACSBottomReadings = new List<ACS430Reading>();

        public ScanRepo() { }
    }
}
