using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserSystemLibrary;
namespace testcropsensors
{
    class ScanRepo
    {
        public List<LmsScan2> LMSScans = new List<LmsScan2>();
        public List<ACS430Reading> ACSTopReadings = new List<ACS430Reading>();
        public List<ACS430Reading> ACSBottomReadings = new List<ACS430Reading>();

        public ScanRepo() { }


        public ScanGroup GetNextScanGroup()
        {
            ScanGroup sg = new ScanGroup();
            while (ACSTopReadings[0].Milliseconds - ACSBottomReadings[0].Milliseconds > 100)
            {
                ACSBottomReadings.RemoveAt(0);
            }
            while (ACSBottomReadings[0].Milliseconds - ACSTopReadings[0].Milliseconds > 100)
            {
                ACSTopReadings.RemoveAt(0);
            }
            if (ACSTopReadings[0].Milliseconds > ACSBottomReadings[0].Milliseconds)
            {
                if (ACSTopReadings[0].Milliseconds - ACSBottomReadings[0].Milliseconds
                    < ACSTopReadings[0].Milliseconds - ACSBottomReadings[1].Milliseconds)
                {
                    sg.TopReading = ACSTopReadings[0];
                    sg.BottomReading = ACSBottomReadings[0];
                }
                else
                {
                    sg.TopReading = ACSTopReadings[0];
                    sg.BottomReading = ACSBottomReadings[1];
                }
            }
            else
            {
                if (ACSBottomReadings[0].Milliseconds - ACSTopReadings[0].Milliseconds
                    < ACSBottomReadings[0].Milliseconds - ACSTopReadings[1].Milliseconds)
                {
                    sg.TopReading = ACSTopReadings[0];
                    sg.BottomReading = ACSBottomReadings[0];
                }
                else
                {
                    sg.TopReading = ACSTopReadings[1];
                    sg.BottomReading = ACSBottomReadings[0];
                }
            }

            double millisecondsAverage = (sg.TopReading.Milliseconds + sg.BottomReading.Milliseconds) / 2;
            LmsScan2 closestScan = LMSScans.Aggregate((x, y) => Math.Abs(x.calculatedMilliseconds - millisecondsAverage) < Math.Abs(y.calculatedMilliseconds - millisecondsAverage) ? x : y);
            sg.LMSScan = closestScan;
            return sg;
        }

    }
}
