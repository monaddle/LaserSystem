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
    public class ShapefileWriter
    {
        string ShapefilePath;
        FeatureSet fs;
        int countSinceSave = 0;
        string path;
        ProjectionInfo wgs84 = KnownCoordinateSystems.Geographic.World.WGS1984;
        ProjectionInfo utm17 = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone17N;
        
        double[] xy = new double[2];
        double[] zproj = new double[1];
        public ShapefileWriter(string path)
        {
            fs = new FeatureSet(FeatureType.Point);
            fs.DataTable.Columns.Add("DENSITY", typeof(double));
            fs.DataTable.Columns.Add("HEIGHT", typeof(double));
            fs.DataTable.Columns.Add("SKIRTHT", typeof(double));
            fs.DataTable.Columns.Add("TIME", typeof(string));
            fs.DataTable.Columns.Add("LATITUDE", typeof(string));
            fs.DataTable.Columns.Add("LONGITUDE", typeof(string));
            fs.DataTable.Columns.Add("ALTITUDE", typeof(string));
            fs.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
            fs.Projection = utm17;
            this.path = path;
        }

        public static void CreateShapefile(string path)
        {

        }
        public void write(double x, double y, double z, double density, double height, string timestamp, double skirtHeight)
        {
            xy[0] = x;
            xy[1] = y;

            zproj[0] = z;
            Reproject.ReprojectPoints(xy, zproj, utm17, wgs84, 0, 1);

            Point point = new Point(xy[0], xy[1], z);

            
            IFeature feature = fs.AddFeature(point);
            
            feature.DataRow.BeginEdit();
            feature.DataRow["DENSITY"] = density;
            feature.DataRow["HEIGHT"] = height;
            feature.DataRow["TIME"] = timestamp;
            feature.DataRow["LATITUDE"] = xy[1];
            feature.DataRow["LONGITUDE"] = xy[0];
            feature.DataRow["SKIRTHT"] = skirtHeight;
            feature.DataRow.EndEdit();
            fs.SaveAs(path, true);
            countSinceSave++;
        }

        public void writeWithoutSave(double x, double y, double z, double density, double height, string timestamp, double skirtHeight, string scanID)
        {
            xy[0] = x;
            xy[1] = y;

            zproj[0] = z;
            Reproject.ReprojectPoints(xy, zproj, utm17, wgs84, 0, 1);

            Point point = new Point(x, y, z);


            IFeature feature = fs.AddFeature(point);

            feature.DataRow.BeginEdit();
            feature.DataRow["DENSITY"] = density;
            feature.DataRow["HEIGHT"] = height;
            feature.DataRow["TIME"] = timestamp;
            feature.DataRow["LATITUDE"] = xy[1];
            feature.DataRow["LONGITUDE"] = xy[0];
            feature.DataRow["SKIRTHT"] = skirtHeight;
            feature.DataRow["ALTITUDE"] = z;
            feature.DataRow.EndEdit();
            countSinceSave++;
        }

        public void writePoint(double x, double y, double z)
        {
            xy[0] = x;
            xy[1] = y;

            zproj[0] = z;
            //Reproject.ReprojectPoints(xy, zproj, wgs84, utm17, 0, 1);

            Point point = new Point(xy[0], xy[1], z);
            IFeature feature = fs.AddFeature(point);
            feature.DataRow.BeginEdit();
            feature.DataRow.EndEdit();
            
        }

        internal void writeBatch(List<GeoreferencedScan> georeferencedScans)
        {

            for (int i = 0; i < georeferencedScans.Count; i++)
            {
                write(georeferencedScans[i].Point.x, georeferencedScans[i].Point.y,
                    georeferencedScans[i].Point.z, georeferencedScans[i].scanResults.Density,
                    georeferencedScans[i].scanResults.TreeHeight, DateTime.Now.ToString(),
                    georeferencedScans[i].scanResults.SkirtHeight);
            }
        }

        public void WriteBatchWithoutSave(List<GeoreferencedScan> georeferencedScans)
        {
            for (int i = 0; i < georeferencedScans.Count; i++)
            {
                writeWithoutSave(georeferencedScans[i].Point.x, georeferencedScans[i].Point.y,
                    georeferencedScans[i].Point.z, georeferencedScans[i].scanResults.Density,
                    georeferencedScans[i].scanResults.TreeHeight, georeferencedScans[i].timestamp.ToString(),
                    georeferencedScans[i].scanResults.SkirtHeight, georeferencedScans[i].scanID);
            }
        }
        public void close()
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


        internal void writePoints(List<ScanLocation> LeftTicks)
        {
            foreach (ScanLocation tick in LeftTicks)
            {
                writePoint(tick.point.x, tick.point.y, tick.point.z);
            }
        }
    }
}
