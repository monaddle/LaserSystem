using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Laser
{
    class Program
    {
        static void Main(string[] args)
        {
            int BufferSize = 100000;
            byte[] buffer = new byte[BufferSize];
            ushort[] gSamples = new ushort[1024];

            SerialPort thePort = new SerialPort();

            int Need = 0;
            int BytesRead = 0;

            int bytesToRead = 0;
            int needToRead = 0;
            //Crc16 CRCer = new Crc16();

            thePort.BaudRate = 500000;
            thePort.PortName = "COM5";
            thePort.Open();
            string[] portNames = SerialPort.GetPortNames();
            foreach (string port in portNames)
            {
                Console.WriteLine(port);
            }

            byte[] Status = new byte[] { 0x02, 0x00, 0x01, 0x00, 0x31, 0x15, 0x12 };
            byte[] Start = new byte[] { 0x02, 0x00, 0x02, 0x00, 0x20, 0x24, 0x34, 0x08 };
            byte[] Stop = new byte[] { 0x02, 0x00, 0x02, 0x00, 0x20, 0x25, 0x35, 0x08 };

            for (; ; )
            {
                if (Console.KeyAvailable)
                {

                    char Key = Console.ReadKey().KeyChar;
                    if (Key == 'G' || Key == 'g')
                        thePort.Write(ByteArrayToString(Start));
                    if (Key == 'S' || Key == 's')
                        thePort.Write(ByteArrayToString(Stop));
                    if (Key == 'T' || Key == 't')
                        thePort.Write(ByteArrayToString(Status));
                    if (Key == 'Q' || Key == 'q') break;
                    if (Key == 'X' || Key == 'x') break;
                    //Show = 400;
                }

                bytesToRead = thePort.BytesToRead;

                if (bytesToRead > 0)
                {

                    thePort.Read(buffer, BytesRead, bytesToRead);
                    BytesRead = bytesToRead;
                    for (int i = 0; i < bytesToRead; i++)
                    {
                        Console.Write(buffer[i]);
                    }
                    Console.WriteLine();
                    
                    
                }
            }
            


        }
        public static string ByteArrayToString(byte[] bytes)
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
