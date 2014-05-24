using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;
using System.Collections.Concurrent;
using LaserSystemLibrary;
namespace testcropsensors
{
    class Program
    {
        static void Main(string[] args)
        {

            TwoCropCircles tcc = new TwoCropCircles();
            tcc.Run();
            return;
        }
    }
}
