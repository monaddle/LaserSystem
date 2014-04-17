using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using LaserSystemLibrary;
using System.Threading;
using System.Diagnostics;

namespace COMServer
{
    class NMEAServer
    {
        NMEAWriter Writer;
        String SentenceFile;
        Thread th;
        int Hertz;
        Stopwatch SW;
        int NumSent = 0;
        public NMEAServer(string port, int baudRate) : this(port, baudRate, "sentences.bin", 5)
        {
            
        }

        public NMEAServer(string port, int baudRate, string sentenceFile, int hertz)
        {
            this.Hertz = hertz;
            this.SentenceFile = sentenceFile;
            SW = new Stopwatch();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(SentenceFile, FileMode.Open, FileAccess.Read, FileShare.None);
            List<pointXYZ> sentences = (List<pointXYZ>)formatter.Deserialize(stream);

            Writer = new NMEAWriter(sentences, port, baudRate);
        }

        public void RunServer()
        {
            SW.Start();
            th = new Thread(Run);
            th.Start();
        }

        private void Run()
        {
            while (true)
            {
                if (CheckTime())
                {
                    Console.Write("Writing an LMS buffer!");
                    Writer.write();
                }
            }
        }

        private bool CheckTime()
        {
            double interval = 1.0 / Hertz;
            double elapsed = SW.Elapsed.TotalMilliseconds / 1000.0;

            if (Convert.ToInt32(elapsed / interval) > NumSent)
            {
                NumSent++;
                return true;
            }
            return false;
        }
        public void StopServer()
        {
            th.Abort();
        }
    }
}
