using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserSystemLibrary
{
    [Serializable]
    public class ScanResults
    {
        public int ticks;
        public double Density;
        public double TreeHeight;
        public double SkirtHeight;
    }
}
