using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserSystemLibrary;
namespace COMServer
{
    class NMEAWriter
    {
        COMServer CS;
        List<pointXYZ> Sentences;
        int SentSentences = 0;

        public NMEAWriter(List<pointXYZ> sentences, string PortName, int BaudRate)
        {
            if (sentences.Count < 0)
            {
                throw new Exception();
            }
            CS = new COMServer(PortName, BaudRate);
            Sentences = sentences;
        }

        public void write()
        {
            pointXYZ point = Sentences[SentSentences % Sentences.Count];
            CS.Write(String.Format("$GPGPA{0},{1},{2}", point.x,point.y,point.z));
            SentSentences++;
        }
    }
}
