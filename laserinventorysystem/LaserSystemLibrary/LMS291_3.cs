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
    public class LMS291_3
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

        bool leftScan;
        bool startingOffsetDetermined = false;
        public bool saveScans { get; set; }
        int milliseconds = 0;
        int currentSecond = 0;
        public int numberOfScansThisSecond = 0;
        double startingTick = -1;

        public LMS291_3(string portName, int baudRate, Stopwatch stopwatch, bool LeftScan)
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
        int bytesRead;
        
        public LmsScan2 read()
        {
            bytesRead += port.Read(Buffer, bytesRead, 1000 - bytesRead);
            if (bytesRead < 370)
                return null;

            for (int i = 0; i < bytesRead - 6; i++)
            {
                byte STX = Buffer[i];
                byte ADDR = Buffer[i + 1];
                byte CMD = Buffer[i + 4];
                int Need = ((Buffer[i + 2]) | (Buffer[i + 3] << 8)) + 6;	// STX(1)+ADDR(1)+TotalLen(2)+body+CRC(2)

                if (STX == 0x02 && ADDR == 0x80)
                {

                    if (i != 0)	/* Compress out bogus data */
                    {
                        Console.Write("Trashing {0} bytes before header\n", (long)i);
                        memmove(ref Buffer, i, bytesRead - i);
                        bytesRead -= i;
                    }

                    if (Need <= bytesRead)
                    {
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
                            numberOfScansThisSecond = 1;
                            currentSecond = stopWatch.Elapsed.Seconds;
                            numberOfSeconds++;
                            //Console.WriteLine("Second {0} scan average: {1}", currentSecond, Convert.ToDouble(numberOfScansFound) / numberOfSeconds);
                            //Console.WriteLine("Expected scans: {0}, delta: {1}", numberOfSeconds * 75, numberOfScansFound - numberOfSeconds * 75);
                        }

                        ushort givenCheckSum = (ushort)(Buffer[Need - 2] | (Buffer[Need - 1] << 8));
                        LmsScan2 scan = new LmsScan2();

                        scan.buffer = new byte[512];
                        Array.Copy(Buffer, scan.buffer, 512);
                        scan.scanResults = LaserScanUtilities.GetScanInfo(Buffer, leftScan);
                        //Console.WriteLine("starting tick: {0} ", startingTick);
                        scan.milliseconds = startingTick + numberOfScansFound * (10.0 + 10.0 / 3.0);
                        scan.calculatedMilliseconds = Convert.ToDouble(stopWatch.ElapsedTicks) / Stopwatch.Frequency * 1000;
                        scan.second = (int)(scan.calculatedMilliseconds / 1000.0);
                        scan.timestamp = DateTime.Now;
                        ushort Samples = (ushort)(Buffer[5] | (Buffer[6] << 8));

                        memmove(ref Buffer, Need, bytesRead - Need);
                        bytesRead -= Need;

                        Need = 0;	// Looking for new header
                        return scan;
                    }
                }
            }
            return null;
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
