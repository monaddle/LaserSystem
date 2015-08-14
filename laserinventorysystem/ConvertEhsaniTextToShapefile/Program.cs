using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConvertEhsaniTextToShapefile
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(args[0]);
            foreach (String f in Directory.EnumerateFiles(args[0]))
            {
                Console.WriteLine(f);
            }
            Console.ReadKey();
            
        }
    }
}
