using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace COMServer
{
    class Program
    {
        static void Main(string[] args)
        {
            COMServer cs = new COMServer("COM3", 9800);
            COMClient cl = new COMClient("COM4", 9800);
            Thread.Sleep(1000);
            cs.Write("omfg a string!");
            Thread.Sleep(100);
            string output;
            while ((output = cl.ReadPort()) == "")
            {

            }
            Console.WriteLine(output);
            string line = Console.ReadLine();
            cs.Write(line);
            Thread.Sleep(1);
            Console.WriteLine(cl.ReadPort());
            Console.ReadKey();
        }
    }
}
