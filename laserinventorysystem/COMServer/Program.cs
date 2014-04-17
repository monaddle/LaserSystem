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
            LMS291Server gps = new LMS291Server("COM3", 500000);
            gps.RunServer();
            COMClient cl = new COMClient("COM4", 500000);
            string output;
            while (true)
            {   
                output = cl.ReadPort();
                if (output != "")
                {
                    Console.Write(output);
                    output = "";
                }
            }
            
            Console.WriteLine(output);
            string line = Console.ReadLine();
            Thread.Sleep(1);
            Console.WriteLine(cl.ReadPort());
            Console.ReadKey();


        }
    }
}
