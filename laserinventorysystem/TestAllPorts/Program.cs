using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserSystemLibrary;
using System.Diagnostics;
using System.Threading;
namespace TestAllPorts
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<ACSThread> ACSs = new List<ACSThread>();
            for (int i = 9; i <= 16; i++)
            {
                ACS430 acs = new ACS430(String.Format("COM{0}", i), 38400, sw);
                ACSThread thread = new ACSThread();
                thread.ComName = String.Format("COM{0}", i);
                thread.acs = acs;
                ACSs.Add(thread);
                
            }
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < 8; i++)
            {
                Thread th = new Thread(ACSs[i].RunThread);
                threads.Add(th);
                th.Start();
            }

            Console.ReadKey();
            foreach (ACSThread acs in ACSs)
            {
                acs.threadstop = true;
            }
        }
    }
}
