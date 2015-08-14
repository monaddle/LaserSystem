using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using DotSpatial.Data;
using DotSpatial.Topology;
using DotSpatial.Projections;
namespace LaserSystemLibrary
{
    public class ConvertReadingsToText
    {
        List<NmeaSentence> readings = new List<NmeaSentence>();
        List<LmsScan2> leftLMSScans = new List<LmsScan2>();
        List<LmsScan2> rightLMSScans = new List<LmsScan2>();
        ScanningOptions options;
        string filePath;
        string fileTag;
        Stream LeftLMSStream;
        Stream RightLMSStream;
        double[] xy = new double[2];
        double[] zproj = new double[1];
        ProjectionInfo wgs84 = KnownCoordinateSystems.Geographic.World.WGS1984;
        ProjectionInfo utm17 = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone17N;


        public ConvertReadingsToText(string filePath, string fileTag, ScanningOptions options)
        {
            this.options = options;
            this.filePath = filePath;
            this.fileTag = fileTag;
            string gpsLoc = "";
            string leftLMSloc = "";
            string rightLMSloc = "";

            foreach (String f in Directory.EnumerateFiles(filePath))
            {
                if (f.Contains("RightLMS.bin"))
                {
                    rightLMSloc = f;
                }
                if (f.Contains("LeftLMS.bin"))
                {
                    leftLMSloc = f;
                }
                if (f.Contains("GPS.bin"))
                {
                    gpsLoc = f;
                }
            }
            LoadSavedScans(gpsLoc, leftLMSloc, rightLMSloc);
        }

        public void ConvertAndSave() 
        {
            List<ScanLocation> leftPositions = new List<ScanLocation>();
            List<ScanLocation> rightPositions = new List<ScanLocation>();
            ScanLocator locator = new ScanLocator(readings, options);
            List<LmsScan2> scansToRemove = new List<LmsScan2>();
            foreach(LmsScan2 scan in leftLMSScans)
            {
                leftPositions.Add(locator.LocateScan(scan, true));
                if (leftPositions.Last() == null)
                {
                    scansToRemove.Add(scan);
                }
            }
            foreach (LmsScan2 scan in scansToRemove)
            {
                leftLMSScans.Remove(scan);
            }
            scansToRemove = new List<LmsScan2>();

            foreach(LmsScan2 scan in rightLMSScans)
            {
                rightPositions.Add(locator.LocateScan(scan, false));
                if (rightPositions.Last() == null)
                {
                    scansToRemove.Add(scan);
                }
            }
            foreach (LmsScan2 scan in scansToRemove)
            {
                rightLMSScans.Remove(scan);
            }

            LeftLMSStream = File.Create(filePath + fileTag + "LeftLMS.txt");
            EhsaniShapefileWriter sf = new EhsaniShapefileWriter(filePath + fileTag + "ehsanishapefile.shp");
            foreach(ScanLocation scanloc in rightPositions.Where(x=> x != null))
            {
                sf.write(scanloc, "Right");
            }
            sf.Close();
            List<string> rightPositionStrings = rightPositions.Where(x=> x != null).Select(scanloc => scanloc.point.x.ToString() + "," + scanloc.point.y).ToList();
            List<string> rightScanStrings = rightLMSScans.Select(scan => String.Join(",", scan.scanResults.distances.Select(distance => distance.ToString()))).ToList();

            List<string> strings = rightPositionStrings.Zip(rightScanStrings, (position, scan) => position + "|" + scan).ToList();
            System.IO.File.WriteAllLines(filePath + fileTag + "RightLMS.txt", strings);

            List<string> settings = new List<string>();
            settings.Add("Row distance: " + options.rowDistance.ToString());
            settings.Add("Laser offset: " + options.laserOffset.ToString());
            settings.Add("Laser height: " + options.laserHeight.ToString());
            System.IO.File.WriteAllLines(filePath + fileTag + "settings.txt", settings);
        }
        List<double> convertUTMtoNAD83(double x, double y, double z)
        {
            xy[0] = x;
            xy[1] = y;
            Point point = new Point(xy[0], xy[1]);
            zproj[0] = z;
            Reproject.ReprojectPoints(xy, zproj, utm17, wgs84, 0, 1);
            return xy.ToList();
        }
        public void LoadSavedScans(string gpsloc, string leftloc, string rightloc)
        {
            if (leftloc != "")
                options.useLeftLaser = true;
            else
                options.useLeftLaser = false;
            if (rightloc != "")
                options.useRightLaser = true;
            else
                options.useRightLaser = false;

            IFormatter formatter = new BinaryFormatter();

            Stream stream = new FileStream(leftloc, FileMode.Open, FileAccess.Read, FileShare.None);
            try
            {
                while (stream.Length != stream.Position)
                {
                    LmsScan2 scan = (LmsScan2)formatter.Deserialize(stream);
                    leftLMSScans.Add(scan);
                }
            }
            catch (SerializationException e) {
                leftLMSScans = null;
            }
            try
            {
                stream = new FileStream(rightloc, FileMode.Open, FileAccess.Read, FileShare.None);
                while (stream.Length != stream.Position)
                {
                    LmsScan2 scan = (LmsScan2)formatter.Deserialize(stream);
                    rightLMSScans.Add(scan);
                }
            }
            catch (SerializationException e)
            {
                rightLMSScans = null;
            }
            stream = new FileStream(gpsloc, FileMode.Open, FileAccess.Read, FileShare.None);
            while (stream.Length != stream.Position)
            {
                NmeaSentence sentence = (NmeaSentence)formatter.Deserialize(stream);
                readings.Add(sentence);
            }
            List<NmeaSentence> readings2 = new List<NmeaSentence>();
            for (int i = 0; i < readings.Count; i++)
            {
                if (i % 5 == 0 | i == readings.Count - 1)
                {
                    readings2.Add(readings[i]);
                }
            }
            readings = readings2;
        }
    }

    class Position
    {

    }


}


