using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
namespace LaserSystemLibrary
{
    public class ACSThread 
    {
        public ACS430 acs;
        public ConcurrentQueue<ACS430Reading> readings;

        public bool threadstop;
        public void RunThread()
        {
            ACS430Reading reading;
            while (!threadstop)
            {
                reading = acs.Read();
                if (reading != null)
                {
                    readings.Enqueue(reading);
                    Console.WriteLine("{0}: A reading!", acs.Port.PortName);
                }
            }
        }
    }
}
