using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace COMServer
{
    class COMServer
    {
        SerialPort port = new SerialPort();
        public COMServer(string portName, int baudRate)
        {
            port.PortName = portName;
            port.BaudRate = baudRate;
            port.Open();
        }

        public void Write(string message)
        {
            port.Write(message);
        }
    }
}
