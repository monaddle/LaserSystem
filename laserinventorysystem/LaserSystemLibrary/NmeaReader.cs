using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;
using System.Collections.Concurrent;
namespace LaserSystemLibrary
{
    
    public class NmeaReader
    {
        public SerialPort port = new SerialPort();
        public ConcurrentQueue<NmeaSentence> readings = new ConcurrentQueue<NmeaSentence>();

        int BytesRead = 0;
        int BytesToRead;
        char[] chars;
        public Stopwatch stopwatch;
        public NmeaReader(string portName, int baudRate, Stopwatch SW)
        {
            port.BaudRate = baudRate;
            port.PortName = portName;
            port.Open();
            chars = new char[200];
            stopwatch = SW;
        }
        public void close()
        {
            port.Close();
        }

        public void read()
        {
            BytesRead += port.Read(chars, BytesRead, 69);
            if (BytesRead > 0)
            {
                while (BytesRead < 69)
                    BytesRead += port.Read(chars, BytesRead, 69);
                
                String str = new String(chars);
                int index = str.IndexOf("$GPGGA");
                if (index > 0)
                {
                    memmove(ref chars, index, 100);
                    BytesRead -= index;
                    return;
                }
                if (index == -1 & BytesRead > 100)
                {
                    BytesRead = 0;
                }


                NmeaSentence sentence = new NmeaSentence();

                try
                {
                    String[] gpgga = str.Substring(index, 67).Split(',');
                    sentence.buffer = gpgga;
                    sentence.ticks = stopwatch.ElapsedTicks;

                    Console.WriteLine("Latittude:{0}{1}, Longitude:{2}{3}, altitude:{4}", gpgga[2], gpgga[3], gpgga[4], gpgga[5], gpgga[9]);
                    readings.Enqueue(sentence);
                    BytesRead = 0;
                    chars = new char[200];
                    
                }
                catch
                {
                    BytesRead = 0;
                    return;
                }

                
            }
        }
        public static void memmove(ref char[] input, int start, int size)
        {
            for (int i = 0; i < size; i++)
            {
                input[i] = input[start + i];
            }
        }
    }
}
