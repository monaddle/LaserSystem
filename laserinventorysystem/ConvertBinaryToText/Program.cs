using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ConvertBinaryToText
{
    class Program
    {
        static void Main(string[] args)
        {
            string dir = args.Length > 0 ? args[0] : "";
            Console.WriteLine("Please enter a directory with laser binary files you would like converted to string.");
            if (!Directory.Exists(dir))
            {
                Console.WriteLine("The directory specified does not exist. Enter a valid directory or enter q to exit");
                
            }
            Console.ReadKey();
        }
    }

    class ErrorReport
    {
        void Report(string text)
        {
            Console.WriteLine(text);
        }
    }
}
