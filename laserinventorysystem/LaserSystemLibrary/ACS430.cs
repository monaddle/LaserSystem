using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
namespace LaserSystemLibrary
{
    public class ACS430
    {
        public SerialPort Port;
        char[] buffer = new char[100];
        int bytesread = 0;
        Stopwatch SW;
        double previousMs = 0;
        double currentMs = 0;
        double numScans = 0;
        
        public ACS430(string portName, int baudRate, Stopwatch sw)
        {
            Port = new SerialPort(portName, baudRate);
            Port.Open();
            SW = sw;
        }
        
        public ACS430Reading Read()
        {
            bytesread += Port.Read(buffer, bytesread, 100 - bytesread);
            if (bytesread > 0)
            {
                while (!buffer.Contains('\n'))
                {
                    bytesread += Port.Read(buffer, bytesread, 100 - bytesread);
                }

                int index = GetIndexOfChar('\n', buffer);
                try
                {
                    double[] values = new string(buffer.Take(index).ToArray()).Split(',')
                        .Select(n => Convert.ToDouble(n)).ToArray();
                    if (values.Length < 5)
                    {
                        throw new Exception();
                    }
                    bytesread = 0;
                    buffer = new char[100];
                    previousMs = currentMs;
                    currentMs = SW.Elapsed.TotalMilliseconds;
                    numScans++;
                    //Console.WriteLine("another scan! average scan time: {0}", currentMs / numScans);
                    //Console.WriteLine("average time between scans: {0}", currentMs / numScans);
                    return new ACS430Reading(SW.Elapsed.TotalMilliseconds, values[0], values[1], values[2], values[3], values[4]);
                }
                catch (ThreadAbortException err)
                {
                    throw err;
                }
                catch
                {
                    Console.WriteLine("oops, an error...");
                    buffer = new char[100];
                    bytesread = 0;
                }
            }
            return null;
        }

        private int GetIndexOfChar(char c, char[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == c)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Close()
        {
            
            Port.Close();
            Port.Dispose();
        }
    }
}
