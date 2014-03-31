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
        byte[] bytes;
        public Stopwatch stopwatch;
        public NmeaReader(string portName, int baudRate, Stopwatch SW)
        {
            port.BaudRate = baudRate;
            port.PortName = portName;
            port.Open();
            bytes = new byte[20000];
            stopwatch = SW;
        }
        public void close()
        {
            port.Close();
        }

        public void read()
        {
            BytesToRead = port.BytesToRead;
            if (BytesToRead > 0)
            {

                port.Read(bytes, BytesRead, BytesToRead);
                BytesRead += BytesToRead;
                //Console.WriteLine(System.Text.Encoding.ASCII.GetString(bytes, 0, BytesRead));

                String str = System.Text.Encoding.ASCII.GetString(bytes, 0, BytesRead);
                int index = str.IndexOf("$GPGGA");
                if ((BytesRead - index) > 67 & (index != -1))
                {
                    NmeaSentence sentence = new NmeaSentence();

                    try
                    {
                        String[] gpgga = str.Substring(index, 67).Split(',');
                        sentence.buffer = gpgga;
                        sentence.ticks = stopwatch.ElapsedTicks;

                        //Console.WriteLine("Latittude:{0}{1}, Longitude:{2}{3}, altitude:{4}", gpgga[2], gpgga[3], gpgga[4], gpgga[5], gpgga[9]);
                        readings.Enqueue(sentence);
                        memmove(ref bytes, 66, 2000);
                        BytesRead = 0;
                    }
                    catch
                    {
                        return;
                    }

                }
            }
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
    }
}
