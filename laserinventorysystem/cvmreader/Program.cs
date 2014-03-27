using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserSystemLibrary;

namespace cvmreader
{
    class Program
    {
        static void Main(string[] args)
        {
            LaserScanUtilities.calculateLowestAngle(16, 8);
            byte[] bytes = System.IO.File.ReadAllBytes(@"C:\Users\daniel\Documents\bas 3-2\0627123450LS.cvm");
            string gpsfile = System.IO.File.ReadAllText(@"C:\Users\daniel\Documents\bas 3-2\0627123450GS.cvm");
            string splitter = "$GPGGA";
            string[] readings = gpsfile.Split("$GPGGA".ToCharArray());
            Console.WriteLine();
            //Console.ReadKey();
            ScanResults sr;
            int count = 0;
            for (int i = 0; i < bytes.Length - 1000; i++)
            {
                byte STX = bytes[i];
                byte ADDR = bytes[i + 1];
                byte CMD = bytes[i + 4];
                if (STX == 0x02)
                {
                    if ((ADDR & 0x80) == 0x80)
                    {
                        if (CMD == 176)
                        {
                            byte[] tempBuffer = new byte[512];
                            Array.Copy(bytes, tempBuffer, 512);
                            Array.Copy(bytes, i, tempBuffer, 0, 512);
                            sr = LaserScanUtilities.GetScanInfo(tempBuffer, true);
                            count++;
                        }
                    }
                }
            }
            Console.WriteLine("count: {0}", count);
            Console.ReadKey();
        }
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }

}
