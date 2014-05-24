using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LaserSystemLibrary;
namespace RerunScans
{
    class Program
    {
        static void Main(string[] args)
        {
            BinaryFormatter deserializer = new BinaryFormatter();
            string dir = @"C:\Users\daniel\Documents\05-23-14 15.28.17\";
            List<string> files = new List<string>(Directory.EnumerateFiles(dir));
            Stream topleft = File.OpenRead(files.Where(x => x.Contains("TopLeftACS")).ToList()[0]);
            Stream topRight = File.OpenRead(files.Where(x => x.Contains("TopRightACS")).ToList()[0]);
            Stream bottomLeft = File.OpenRead(files.Where(x => x.Contains("BottomLeftACS")).ToList()[0]);
            Stream bottomRight = File.OpenRead(files.Where(x => x.Contains("BottomRightACS")).ToList()[0]);
            Stream LeftLMS = File.OpenRead(files.Where(x => x.Contains("LeftLMS")).ToList()[0]);
            Stream rightLMS = File.OpenRead(files.Where(x => x.Contains("RightLMS")).ToList()[0]);
            Stream GPS = File.OpenRead(files.Where(x => x.Contains("GPS")).ToList()[0]);

            List<ACS430Reading> TopLeftReadings = new List<ACS430Reading>();
            List<ACS430Reading> TopRightReadings = new List<ACS430Reading>();
            List<ACS430Reading> BottomLeftReadings = new List<ACS430Reading>();
            List<ACS430Reading> BottomRightReadings = new List<ACS430Reading>();
            while (topleft.Position <= topleft.Length - 1)
            {
                TopLeftReadings.Add((ACS430Reading)deserializer.Deserialize(topleft));
            }
            while (topRight.Position <= topRight.Length - 1)
            {
                TopRightReadings.Add((ACS430Reading)deserializer.Deserialize(topRight));
            }
            while (bottomLeft.Position <= bottomLeft.Length - 1)
            {
                BottomLeftReadings.Add((ACS430Reading)deserializer.Deserialize(bottomLeft));
            } 
            while (bottomRight.Position <= bottomRight.Length - 1)
            {
                BottomRightReadings.Add((ACS430Reading)deserializer.Deserialize(bottomRight));
            }
            
            double lastMS = 0;
            foreach (ACS430Reading reading in BottomLeftReadings)
            {
                
                Console.WriteLine(reading.Milliseconds - lastMS);
                lastMS = reading.Milliseconds;
                
            }
            TopRightReadings.Average(x => x.Milliseconds);
            List<double> values = TopRightReadings.ConvertAll(x => x.Milliseconds);
            Console.WriteLine("STD DEVIATION: {0}", ACSStdDev(BottomLeftReadings));
            Console.ReadKey();
        }

        public static double ACSStdDev(List<ACS430Reading> readings)
        {
            List<double> differences = new List<double>();
            double lastMS = 0;
            foreach(ACS430Reading reading in readings)
            {
                differences.Add(reading.Milliseconds - lastMS);
                lastMS = reading.Milliseconds;
            }
            return StandardDeviation(differences);

        }
        public static double StandardDeviation(List<double> valueList)
        {
            double M = 0.0;
            double S = 0.0;
            int k = 1;
            foreach (double value in valueList)
            {
                double tmpM = M;
                M += (value - tmpM) / k;
                S += (value - tmpM) * (value - M);
                k++;
            }
            return Math.Sqrt(S / (k - 1));
        }
    }
}
