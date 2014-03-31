using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace LaserSystemLibrary
{
    public class LMS291_2
    {
        public SerialPort port = new SerialPort();
        public ConcurrentQueue<LmsScan2> scans;
        public Stopwatch stopWatch;
        int BufferSize = 512;
        public byte[] Buffer;
        int Need = 0;
        int Have = 0;
        int BytesToRead = 0;
        public int numberOfScansFound = 0;
        int numberOfSeconds = 0;
        byte[] Status = new byte[] { 0x02, 0x00, 0x01, 0x00, 0x31, 0x15, 0x12 };
        byte[] Start = new byte[] { 0x02, 0x00, 0x02, 0x00, 0x20, 0x24, 0x34, 0x08 };
        byte[] Stop = new byte[] { 0x02, 0x00, 0x02, 0x00, 0x20, 0x25, 0x35, 0x08 };
        byte[] InstallationMode = new byte[] { 0x02, 0x00, 0x0A, 0x00, 0x20, 0x00, 
                        0x53, 0x49, 0x43, 0x4B, 0x5F, 0x4C, 0x4D, 0x53, 0x5F, 0xB2 };
        byte[] SwitchOperatingModeSuccess = new byte[] { 0x02, 0x80, 0x03, 0x00, 0xA0, 0x00, 0x10, 0x16, 0x0A };
        byte[] RequestConfiguration = { 0x02, 0x00, 0x01, 0x00, 0x74, 0x50, 0x12 };
        byte[] SingleScan = new byte[] { 0x02, 0x00, 0x02, 0x00, 0x30, 0x01, 0x31, 0x18 };

        int lowestOffset = 0;
        int currentOffset = 0;
        int AgeOfLowestOffset = 0;
        int MaxAgeOfLowestOffset = 10;
        int startingOffset = 0;
        bool leftScan;
        bool startingOffsetDetermined = false;
        public bool saveScans { get; set; }

        private int HandleOffset(int offset)
        {
            int SuggestedOffset = 0;
            Console.WriteLine("Offset: {0}, LowestOffset: {1}, AgeOfLowestOffset: {2}, currentoffset: {3}", offset, lowestOffset, AgeOfLowestOffset, currentOffset);
            if (lowestOffset == 0)
            {
                lowestOffset = offset;
            }
            if (startingOffsetDetermined == false)
            {
                if (AgeOfLowestOffset < MaxAgeOfLowestOffset)
                {

                    if (lowestOffset > offset)
                    {
                        lowestOffset = offset;
                        AgeOfLowestOffset = 0;
                    }
                    AgeOfLowestOffset++;
                }
                else
                {
                    startingOffsetDetermined = true;
                    startingOffset = lowestOffset;
                    currentOffset = lowestOffset;
                    AgeOfLowestOffset = 0;
                }
            }
            else
            {
                if (offset < lowestOffset)
                {
                    lowestOffset = offset;
                    AgeOfLowestOffset = 0;
                }
                else
                {
                    AgeOfLowestOffset++;
                    if (AgeOfLowestOffset >= MaxAgeOfLowestOffset)
                    {
                        if (lowestOffset > currentOffset)
                        {
                            SuggestedOffset = lowestOffset - currentOffset;
                            currentOffset = lowestOffset;
                        }
                        else
                        {
                            AgeOfLowestOffset = 0;
                            lowestOffset = 0;
                        }
                    }
                }
            }
            return SuggestedOffset;
        }
        int milliseconds = 0;
        int currentSecond = 0;
        public int numberOfScansThisSecond = 0;
        double startingTick = -1;
        public LMS291_2(string portName, int baudRate, Stopwatch stopwatch, bool LeftScan)
        {
            scans = new ConcurrentQueue<LmsScan2>();
            port.PortName = portName;
            port.BaudRate = baudRate;
            this.stopWatch = stopwatch;
            Buffer = new Byte[BufferSize * 2];
            port.Open();
            BytesToRead = port.BytesToRead;
            leftScan = LeftScan;


        }

        public void close()
        {
            port.Close();
        }

        public void Reset()
        {
            numberOfScansFound = 0;
            numberOfSeconds = 0;
        }
        public LmsScan2 read()
        {

            BytesToRead = port.BytesToRead;
            //Console.WriteLine("Bytes to read: {0}", BytesToRead);
            if (BytesToRead > 0)
            {
                if (startingTick == 0)
                {
                    startingTick = stopWatch.ElapsedTicks / Stopwatch.Frequency / 1000;
                }
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
                bool foundCommand;

                for (int i = 0; i < Have; i++)
                {
                    byte STX = Buffer[i];
                    byte ADDR = Buffer[i + 1];
                    byte CMD = Buffer[i + 4];
                    if ((Need == 0)	/* Looking for header? */
                    && Have >= i + 6)	/* Have enough to check it? */
                    {
                        if (STX == 0x02)
                        {
                            if ((ADDR & 0x80) == 0x80)
                            {
                                foundCommand = true;
                                Need = ((Buffer[i + 2]) | (Buffer[i + 3] << 8)) + 6;	// STX(1)+ADDR(1)+TotalLen(2)+body+CRC(2)
                                Byte[] bytes = { Buffer[i + 2], Buffer[i + 3], 0, 0 };
                                Int32 dataLength = System.BitConverter.ToInt32(bytes, 0);

                                if (Need > BufferSize)
                                {
                                    memmove(ref Buffer, i + 3, (Have - (i + 3)));
                                    Need = 0;
                                    Have = Have - (i + 3);
                                    break;
                                }
                                //if(CMD == 0x3F)
                                if (CMD == 0xB0 || CMD == 0xF4 || CMD == 0xB1)
                                {
                                    if (i != 0)	/* Compress out bogus data */
                                    {
                                        Console.Write("Trashing {0} bytes before header\n", (long)i);
                                        memmove(ref Buffer, i, Have - i);
                                        Have -= i;
                                    }
                                    //Console.WriteLine("\n\nCommand {0}\n\n", CMD);
                                }
                                else
                                {
                                    Console.WriteLine("\n\nCommand {0}\n\n", CMD);
                                }
                            }
                        }
                    }
                    if ((Need > 0) && Have >= Need)
                    {
                        // Get checksum
                        numberOfScansFound++;
                        if (startingTick == -1)
                        {
                            startingTick = Convert.ToDouble(stopWatch.ElapsedTicks) / Stopwatch.Frequency;
                        }
                        if (currentSecond == stopWatch.Elapsed.Seconds)
                        {
                            numberOfScansThisSecond++;
                        }
                        if (currentSecond != stopWatch.Elapsed.Seconds)
                        {
                            Console.WriteLine("number of scans this second: {0}", numberOfScansThisSecond);
                            numberOfScansThisSecond = 0;
                            currentSecond = stopWatch.Elapsed.Seconds;
                            numberOfSeconds++;

                            Console.WriteLine("Second {0} scan average: {1}", currentSecond, Convert.ToDouble(numberOfScansFound) / numberOfSeconds);
                            Console.WriteLine("Expected scans: {0}, delta: {1}", numberOfSeconds * 75, numberOfScansFound - numberOfSeconds * 75);
                            Console.WriteLine("Suggested offset: {0}", HandleOffset(numberOfSeconds * 75 - numberOfScansFound));
                        }

                        ushort givenCheckSum = (ushort)(Buffer[Need - 2] | (Buffer[Need - 1] << 8));
                        LmsScan2 scan = new LmsScan2();
                        if (saveScans)
                        {
                            scan.buffer = new byte[512];
                            Array.Copy(Buffer, scan.buffer, 512);
                        }
                        scan.scanResults = LaserScanUtilities.GetScanInfo(Buffer, leftScan);
                        foundCommand = false;
                        //Console.WriteLine("starting tick: {0} ", startingTick);
                        scan.milliseconds = startingTick + numberOfScansFound * (10.0 + 10.0 / 3.0);
                        scan.calculatedMilliseconds = Convert.ToDouble(stopWatch.ElapsedTicks) / Stopwatch.Frequency * 1000;
                        scan.second = (int)(scan.calculatedMilliseconds / 1000.0);
                        scan.timestamp = DateTime.Now;
                        ushort Samples = (ushort)(Buffer[5] | (Buffer[6] << 8));

                        memmove(ref Buffer, Need, Have - Need);
                        Have -= Need;

                        Need = 0;	// Looking for new header
                        return scan;
                    }


                }
                // printf("%.*s", (int) BytesRead, Buffer);
            }

            return null;
            //Console.Read();

        }
        public void StartContinuousScan()
        {
            port.Write(ByteArrayToString(Start));
        }

        public void RequestSingleScan()
        {
            port.Write(ByteArrayToString(SingleScan));
        }

        public void StopContinuousScan()
        {
            port.Write(ByteArrayToString(Stop));
        }
        public void RequestStatus()
        {
            port.Write(ByteArrayToString(Status));
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
    }
}
