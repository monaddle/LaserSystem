using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using LaserSystemLibrary;
namespace timingtest
{
    class Program
    {
        static void Main(string[] args)
        {
            ScanningOptions options = new ScanningOptions();
            options.LoadSettings();
            options.laserHeight = 8.0;
            options.samplingDistance = 1.0;
            string rightloc = @"C:\scans from jack\rightJanuary-16-2014124940.bin";
            string leftloc = @"C:\scans from jack\leftJanuary-16-2014124940.bin";
            string gpsloc = @"C:\scans from jack\gpsJanuary-16-2014124940.bin";

            ScanRunner4 scanrunner = new ScanRunner4(options, false, false, null, null, null);
            scanrunner.LoadSavedScans(gpsloc, leftloc, rightloc);
            scanrunner.ProcessSavedScans();
            
        }
    }
}
