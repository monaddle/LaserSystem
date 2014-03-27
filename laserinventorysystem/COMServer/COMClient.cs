using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
namespace COMServer
{
    class COMClient
    {
        SerialPort port = new SerialPort();
        byte[] buffer = new byte[1024];
        public COMClient(string portName, int baudRate)
        {
            port.PortName = portName;
            port.BaudRate = baudRate;
            port.Open();
        }
        public string ReadPort()
        {
            string str = "";
            while(port.BytesToRead > 0)
            {
                int bytestoread = port.BytesToRead;
                if (bytestoread > 1024)
                {
                    bytestoread = 1024;
                }
                port.Read(buffer, 0, bytestoread);
                str += System.Text.Encoding.ASCII.GetString(buffer, 0, bytestoread);
            }
            return str;
        }
            
    }
}
