using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMServer
{
    class NMEAWriter
    {
        COMServer cs;
        int hertz = 5;
        public NMEAWriter(string PortName, int BaudRate)
        {
            cs = new COMServer(PortName, BaudRate);
        }
    }
}
