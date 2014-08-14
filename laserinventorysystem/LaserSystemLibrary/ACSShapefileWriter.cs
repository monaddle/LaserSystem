using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Data;
using DotSpatial.Topology;
using DotSpatial.Projections;
using System.IO;
namespace LaserSystemLibrary
{
    public class ACSShapefileWriter
    {
        FeatureSet fs;
        int countSinceSave = 0;
        string path;
        ProjectionInfo wgs84 = KnownCoordinateSystems.Geographic.World.WGS1984;
        ProjectionInfo utm17 = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone17N;

        double[] xy = new double[2];
        double[] zproj = new double[1];
        public ACSShapefileWriter(string path)
        {
            fs = new FeatureSet(FeatureType.Point);
            fs.DataTable.Columns.Add("DENSITY", typeof(double));
            fs.DataTable.Columns.Add("HEIGHT", typeof(double));
            fs.DataTable.Columns.Add("SKIRTHT", typeof(double));
            fs.DataTable.Columns.Add("TREDEDGE", typeof(double));
            fs.DataTable.Columns.Add("BREDEDGE", typeof(double));
            fs.DataTable.Columns.Add("TNIR", typeof(double));
            fs.DataTable.Columns.Add("BNIR", typeof(double));
            fs.DataTable.Columns.Add("TRED", typeof(double));
            fs.DataTable.Columns.Add("BRED", typeof(double));
            fs.DataTable.Columns.Add("TNDRE", typeof(double));
            fs.DataTable.Columns.Add("BNDRE", typeof(double));
            fs.DataTable.Columns.Add("TNDVI", typeof(double));
            fs.DataTable.Columns.Add("BNDVI", typeof(double));
            fs.DataTable.Columns.Add("TIME", typeof(string));
            fs.DataTable.Columns.Add("LATITUDE", typeof(string));
            fs.DataTable.Columns.Add("LONGITUDE", typeof(string));
            fs.DataTable.Columns.Add("ALTITUDE", typeof(double));
            fs.DataTable.Columns.Add("Left", typeof(string));
            fs.DataTable.Columns.Add("DISTANCE", typeof(double));
            fs.DataTable.Columns.Add("HARBLKID", typeof(string));
            fs.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
            fs.Projection = utm17;
            this.path = path;
        }


        public void write(ScanGroup scans, string left)
        {
            double ClosestPoint = LaserScanUtilities.GetClosestPoint(scans.LMSScan, left);
            xy[0] = scans.ScanLoc.point.x;
            xy[1] = scans.ScanLoc.point.y;
            Point point = new Point(xy[0], xy[1]);
            zproj[0] = scans.ScanLoc.point.z;
            Reproject.ReprojectPoints(xy, zproj, utm17, wgs84, 0, 1);
            IFeature feature = fs.AddFeature(point);

            feature.DataRow.BeginEdit();
            feature.DataRow["DENSITY"] = scans.ScanResults.Density;
            feature.DataRow["HEIGHT"] = scans.ScanResults.TreeHeight;
            feature.DataRow["SKIRTHT"] = scans.ScanResults.SkirtHeight;

            feature.DataRow["TREDEDGE"] = scans.TopReading.RedEdge;
            feature.DataRow["TNIR"] = scans.TopReading.NIR;
            feature.DataRow["TRED"] = scans.TopReading.Red;
            feature.DataRow["TNDRE"] = scans.TopReading.NDRE;
            feature.DataRow["TNDVI"] = scans.TopReading.NDVI;

            feature.DataRow["BREDEDGE"] = scans.BottomReading.RedEdge;
            feature.DataRow["BNIR"] = scans.BottomReading.NIR;
            feature.DataRow["BRED"] = scans.BottomReading.Red;
            feature.DataRow["BNDRE"] = scans.BottomReading.NDRE;
            feature.DataRow["BNDVI"] = scans.BottomReading.NDVI;

            feature.DataRow["TIME"] = scans.LMSScan.timestamp.ToString();
            feature.DataRow["LATITUDE"] = xy[1];
            feature.DataRow["LONGITUDE"] = xy[0];
            feature.DataRow["left"] = left;
            feature.DataRow["ALTITUDE"] = scans.ScanLoc.point.z;
            feature.DataRow["DISTANCE"] = ClosestPoint;
            feature.DataRow["HARBLKID"] = scans.ScanLoc.HarBlkID;
            feature.DataRow.EndEdit();
        }



        public void Close()
        {
            fs.UpdateExtent();
            fs.SaveAs(path, true);
            string[] prjpatharray = path.Split('.');
            prjpatharray[prjpatharray.Length - 1] = "prj";
            string prjpath = String.Join(".", prjpatharray);
            string[] lines = new string[1];
            lines[0] = "PROJCS[\"WGS_1984_UTM_Zone_17N\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",500000.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",-81.0],PARAMETER[\"Scale_Factor\",0.9996],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]";
            System.IO.File.WriteAllLines(prjpath, lines);
        }


    }
}
