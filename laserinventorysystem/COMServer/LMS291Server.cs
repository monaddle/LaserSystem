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
    class LMS291Server
    {
        LMSWriter Writer;
        String SentenceFile;
        Thread th;
        int Hertz = 75;
        Stopwatch SW;
        string mode = "SEND";
        int NumSent = 0;


        public LMS291Server(string port, int baudRate, string sentenceFile)
        {
            this.SentenceFile = sentenceFile;
            SW = new Stopwatch();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(SentenceFile, FileMode.Open, FileAccess.Read, FileShare.None);
            List<LmsScan> sentences = (List<LmsScan>)formatter.Deserialize(stream);

            //Writer = new LMSWriter(sentences, port, baudRate);
        }

        public LMS291Server(string p1, int p2) :this(p1, p2, "scans.bin")
        {

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
