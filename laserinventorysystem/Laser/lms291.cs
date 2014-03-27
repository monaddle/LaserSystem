using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Laser
{
    class lms291
    {
        SerialPort _serialPort;
        byte[] StatusCode = new byte[] { 0x02, 0x00, 0x01, 0x00, 0x31, 0x15, 0x12 };

        public lms291(string comPort, int baudRate)
        {
            _serialPort = new SerialPort();
            _serialPort.BaudRate = baudRate;
            _serialPort.PortName = comPort;

        }

        public bool status()
        {
            bool status = false;
            _serialPort.Write(ByteArrayToString(StatusCode));

            return status;
        }
        private string ByteArrayToString(byte[] bytes)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            string a = encoding.GetString(bytes);
            byte[] b = new byte[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                b[i] = (byte)(a[i]);
            }
            return a;
        }
    }
}
