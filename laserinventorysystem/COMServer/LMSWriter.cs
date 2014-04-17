using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LaserSystemLibrary;
namespace COMServer
{
    class LMSWriter
    {
        COMServer CS;
        List<LmsScan2> Scans;
        int SentScans = 0;
        public LMSWriter(List<LmsScan2> scans, string port, int baudRate)
        {
            if (scans.Count < 0)
            {
                throw new Exception();
            }
            CS = new COMServer(port, baudRate);
            Scans = scans;
        }

        public void write()
        {
            LmsScan2 scan = Scans[SentScans % Scans.Count];
            CS.WriteBytes(scan.buffer, 0, scan.buffer.Length);
            SentScans++;
        }
    }
}
