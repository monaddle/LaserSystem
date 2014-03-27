using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;

namespace SICK_LMS_211
{
    public class NmeaResponse
    {
        public byte[] buffer;
        public DateTime timestamp;

        public NmeaResponse()
        {
            buffer = new Byte[100];
        }
    }

    public class LMSScan
    {
        public DateTime timestamp;
        public byte[] buffer;

        public LMSScan()
        {
            buffer = new Byte[512];
        }

    }

    class Program
    {
        static ConcurrentQueue<NmeaResponse> results;


        static void Main(string[] args)
        {
            // setup threading
            results = new ConcurrentQueue<NmeaResponse>();
            
            int BufferSize = 512;
            byte[] Buffer = new byte[BufferSize];
            ushort[] gSamples = new ushort[1024];
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\daniel\laseroutput.txt");

            //System.IO.StreamWriter Console = new System.IO.StreamWriter(@"C:\Users\daniel\sickoutput.txt");
            Stopwatch stopWatch = new Stopwatch();
            SerialPort port = new SerialPort();
            TimeSpan elapsed = stopWatch.Elapsed;
            stopWatch.Start();
            double milliseconds = 0;
            
            Console.WriteLine(stopWatch.Elapsed.Milliseconds);
            
            int second = stopWatch.Elapsed.Seconds;
            int noScansPerSecond = 0;
            int Need = 0;
            int Have = 0;
            int numberOfScans = 0;
            int numberOfSeconds = 0;
            int BytesToRead;
            double currentScanTime = 0;
            double timeBetweenScans = 0;
            double lastScanTime = 0;

            port.BaudRate = 500000;
            port.PortName = "COM5";
            port.Open();
            byte[] SingleScan = new byte[] { 0x02, 0x00, 0x02, 0x00, 0x30, 0x01, 0x31, 0x18 };
            byte[] Start = new byte[] { 0x02, 0x00, 0x02, 0x00, 0x20, 0x24, 0x34, 0x08 };

            port.Write(ByteArrayToString(Start));

            for (; ; )
            {
                if (Console.KeyAvailable)
                {
                    byte[] Status = new byte[] { 0x02, 0x00, 0x01, 0x00, 0x31, 0x15, 0x12 };
                    byte[] Stop = new byte[]   { 0x02, 0x00, 0x02, 0x00, 0x20, 0x25, 0x35, 0x08 };
                    byte[] InstallationMode = new byte[] { 0x02, 0x00, 0x0A, 0x00, 0x20, 0x00, 
                        0x53, 0x49, 0x43, 0x4B, 0x5F, 0x4C, 0x4D, 0x53, 0x5F, 0xB2 };
                    byte[] SwitchOperatingModeSuccess = new byte[] { 0x02, 0x80, 0x03, 0x00, 0xA0, 0x00, 0x10, 0x16, 0x0A };
                    byte[] RequestConfiguration = { 0x02, 0x00, 0x01, 0x00, 0x74, 0x50, 0x12 };



                    char Key = Console.ReadKey().KeyChar;
                    if (Key == 'G' || Key == 'g')
                        port.Write(ByteArrayToString(Start));
                    if (Key == 'S' || Key == 's')
                        port.Write(ByteArrayToString(Stop));
                    if (Key == 'T' || Key == 't')
                        port.Write(ByteArrayToString(Status));
                    if (Key == 'r' || Key == 'R')
                        port.Write(ByteArrayToString(SingleScan));
                    if (Key == 'C' || Key == 'c')
                        port.Write(ByteArrayToString(RequestConfiguration));

                    // installation mode doesn't work for some reason
                    if (Key == 'I' || Key == 'i')
                        port.Write(ByteArrayToString(InstallationMode));
                    if (Key == 'Q' || Key == 'q') break;
                    if (Key == 'X' || Key == 'x') break;
                    //Show = 400;
                }

                BytesToRead = port.BytesToRead;

                if (BytesToRead > 0)
                {
                    //Console.Write("{0}, ", BytesRead);

                    if (Need > 0 && (Need - Have) < BytesToRead)
                    {
                        BytesToRead = Need - Have;
                    }
                    if (BytesToRead > (BufferSize - Have))
                    {
                        BytesToRead = BufferSize - Have;
                    }
                    port.Read(Buffer, Have, BytesToRead);
                    Have += BytesToRead;

                    for (int i = 0; i < Have; i++)
                    {
                        if ((Need == 0)	/* Looking for header? */
                        && Have >= i + 6)	/* Have enough to check it? */
                        {
                            byte STX = Buffer[i];
                            byte ADDR = Buffer[i + 1];
                            byte CMD = Buffer[i + 4];

                            if (STX == 0x02)
                            {
                                if ((ADDR & 0x80) == 0x80)
                                {
                                    if (CMD == 0xB0 || CMD == 0xF4 || CMD == 0xB1)
                                    {
                                        Need = ((Buffer[i + 2]) | (Buffer[i + 3] << 8)) + 6;	// STX(1)+ADDR(1)+TotalLen(2)+body+CRC(2)
                                        if (i != 0)	/* Compress out bogus data */
                                        {
                                            Console.Write("Trashing {0} bytes before header\n", (long)i);
                                            memmove(ref Buffer, i, Have - i);
                                            Have -= i;
                                        }
                                        if (CMD == 0xB1)
                                        {
                                            string ver = "";

                                            for (int j = 5; j < 12; j++)
                                            {
                                                ver += Convert.ToChar(Buffer[j]);
                                            }

                                            Console.WriteLine("\nVersion: {0}", ver);
                                            Console.WriteLine("Operating Mode: {0}h", Buffer[12].ToString("X"));
                                            Console.WriteLine("Status: {0}h", Buffer[13].ToString("X"));
                                            Console.WriteLine("E. Variant Type: {0}h", Buffer[16].ToString("X"));
                                            Console.WriteLine("F. Variant Type: {0}h", Buffer[16].ToString("X"));
                                            
                                            Byte[] bytes = { Buffer[104], Buffer[105], 0, 0};
                                            //Array.Reverse(bytes);
                                            Int32 scanningAngle = System.BitConverter.ToInt32(bytes, 0);

                                            bytes[0] = Buffer[106];
                                            bytes[1] = Buffer[107];
                                            Int32 angularResolution = System.BitConverter.ToInt32(bytes, 0);

                                            Console.WriteLine("Scanning Angle: {0}", scanningAngle);
                                            Console.WriteLine("Angular Resolution: {0}", angularResolution);
                                            
                                        }
                                    }
                                    else
                                        Console.WriteLine("\n\nCommand {0}\n\n", CMD);
                                }
                            }
                        }
                        if ((Need > 0) && Have >= Need)
                        {
                            // Get checksum
                            milliseconds += 10 + 10d / 3;
                            currentScanTime = stopWatch.ElapsedMilliseconds;
                            timeBetweenScans += currentScanTime - lastScanTime;
                            lastScanTime = currentScanTime;
                            
                            ushort givenCheckSum = (ushort)(Buffer[Need - 2] | (Buffer[Need - 1] << 8));


                            ushort Samples = (ushort)(Buffer[5] | (Buffer[6] << 8));
                            

                            Byte[] bytes = {Buffer[2], Buffer[3], 0, 0};
                            Int32 dataLength = System.BitConverter.ToInt32(bytes, 0);
                            for (int j = 0; j < Buffer.Length; j++)
                            {
                            }

                            if (second == stopWatch.Elapsed.Seconds)
                            {
                                noScansPerSecond += 1;
                            }
                            else
                            {
                                noScansPerSecond += 1;
                                second = stopWatch.Elapsed.Seconds;
                                
                                numberOfScans += noScansPerSecond;
                                numberOfSeconds += 1;
                                if (numberOfSeconds == 5)
                                {
                                    milliseconds = stopWatch.ElapsedMilliseconds;
                                }
                                Console.WriteLine("Second {0} delta: {1}",second,  milliseconds - stopWatch.ElapsedMilliseconds);
                                Console.WriteLine("Second {1}: {0} scans. Average: {2}", noScansPerSecond, second, Convert.ToDouble(numberOfScans) / numberOfSeconds);
                                Console.WriteLine("Average time between scans:{0}", timeBetweenScans / numberOfScans);
                                Console.WriteLine("Expected scans: {0}, delta: {1}", numberOfSeconds * 75, numberOfScans - numberOfSeconds * 75);
                                noScansPerSecond = 0;
                            }
                            for (int j = 5; j < dataLength + 4; j+= 2)
                            {
                                
                                Byte[] bytes1 = { Buffer[j], Buffer[j + 1], 0, 0 };
                            }

                            file.WriteLine();

                            memmove(ref Buffer, Need, Have - Need);
                            Have -= Need;
                            //Console.WriteLine("Moved {0} extra bytes", Have);
                            Need = 0;	// Looking for new header
                            
                        }
                    }
                    // printf("%.*s", (int) BytesRead, Buffer);
                }
            }
            port.Close();

            Console.WriteLine("End.");
            //Console.Read();


        }

        public static void memmove(ref byte[] input, int start, int size)
        {
            //byte[] output = new byte[size];
            for (int i = 0; i < size; i++)
            {
                input[i] = input[start + i];
            }
            //return output;
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


        static ushort computeCRC16(byte[] data, int offset, int length)
        {
            ushort CRC16_GEN_POL = 0x8005;
            ushort uCrc16 = 0;
            byte AB0 = 0;
            byte AB1;
            length += offset;
            for (int i = offset; i < length; i++)
            {
                AB1 = AB0;
                AB0 = data[i];
                if ((uCrc16 & 0x8000) == 0)
                {
                    uCrc16 = (ushort)((uCrc16 & 0x7fff) << 1);
                    uCrc16 ^= CRC16_GEN_POL;
                }
                else
                {
                    uCrc16 <<= 1;
                }
                uCrc16 ^= (ushort)(AB0 | ((int)(AB1) << 8));
            }
            return (uCrc16);
        }

        public class Crc16
        {
            const ushort polynomial = 0x8005;
            ushort[] table = new ushort[256];

            public ushort ComputeChecksum(byte[] bytes)
            {
                ushort crc = 0;
                for (int i = 0; i < bytes.Length; ++i)
                {
                    byte index = (byte)(crc ^ bytes[i]);
                    crc = (ushort)((crc >> 8) ^ table[index]);
                }
                return crc;
            }

            public byte[] ComputeChecksumBytes(byte[] bytes)
            {
                ushort crc = ComputeChecksum(bytes);
                return BitConverter.GetBytes(crc);
            }

            public Crc16()
            {
                ushort value;
                ushort temp;
                for (ushort i = 0; i < table.Length; ++i)
                {
                    value = 0;
                    temp = i;
                    for (byte j = 0; j < 8; ++j)
                    {
                        if (((value ^ temp) & 0x0001) != 0)
                        {
                            value = (ushort)((value >> 1) ^ polynomial);
                        }
                        else
                        {
                            value >>= 1;
                        }
                        temp >>= 1;
                    }
                    table[i] = value;
                }
            }
        }

    }
}