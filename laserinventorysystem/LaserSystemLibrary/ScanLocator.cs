using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserSystemLibrary
{
    class ScanLocator
    {
        List<NmeaSentence> sentences;
        double FEET_TO_METERS = 0.3048;
        List<pointXYZ> points = new List<pointXYZ>();
        ScanningOptions options;
        public ScanLocator(List<NmeaSentence> sentences, ScanningOptions options)
        {
            this.options = options;
            this.sentences = sentences;
            foreach (NmeaSentence sentence in sentences)
            {
                this.points.Add(TwoCropCircles.parseGPSReading(sentence));
            }
        }

        public ScanLocation LocateScan(LmsScan2 scan, bool left)
        {
            double smallestDifference = scan.calculatedMilliseconds - sentences[0].milliseconds;
            if (smallestDifference < 0)
            {
                return null;
            }
            int indexOfSmallestDifference = 1;
            double difference = smallestDifference;
            for (int i = 2; i < sentences.Count - 1; i++)
            {
                difference = scan.calculatedMilliseconds - sentences[i].milliseconds;
                if (Math.Abs(difference) < Math.Abs(smallestDifference))
                {
                    smallestDifference = difference;
                    indexOfSmallestDifference = i;
                }
            }
            Console.WriteLine("index of smallest difference: " + indexOfSmallestDifference.ToString());
            pointXYZ p1;
            pointXYZ p2;
            if(smallestDifference >= 0) {
                p1 = this.points[indexOfSmallestDifference];
                p2 = this.points[indexOfSmallestDifference + 1];
            }
            else 
            {
                p1 = this.points[indexOfSmallestDifference - 1];
                p2 = this.points[indexOfSmallestDifference];
            }
            
            LineSegment ls = new LineSegment(p1, p2);
            double xySlope = (p2.y - p1.y) / (p2.x - p1.x);

            ScanLocation scanloc = ls.GetTickAtTime(scan.calculatedMilliseconds);
            LaserScanUtilities.TickOffset offset = LaserScanUtilities.CalculateTickOffset(xySlope, options.rowDistance * FEET_TO_METERS, p1, p2);
            if (!left)
            {
                offset.X = -offset.X;
                offset.Y = -offset.Y;
            }
            scanloc.point.x = scanloc.point.x + offset.X;
            scanloc.point.y = scanloc.point.y + offset.Y;
            return scanloc;
        }
    }
}
