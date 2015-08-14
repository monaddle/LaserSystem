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
    public class EhsaniShapefileWriter
    {
        FeatureSet fs;
        int countSinceSave = 0;
        string path;
        ProjectionInfo wgs84 = KnownCoordinateSystems.Geographic.World.WGS1984;
        ProjectionInfo utm17 = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone17N;

        double[] xy = new double[2];
        double[] zproj = new double[1];
        public EhsaniShapefileWriter(string path)
        {
            fs = new FeatureSet(FeatureType.Point);
            fs.DataTable.Columns.Add("LATITUDE", typeof(string));
            fs.DataTable.Columns.Add("LONGITUDE", typeof(string));
            fs.DataTable.Columns.Add("ALTITUDE", typeof(double));
            fs.DataTable.Columns.Add("Left", typeof(string));
            fs.DataTable.Columns.Add("HARBLKID", typeof(string));
            fs.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
            fs.Projection = utm17;
            this.path = path;
        }


        public void write(ScanLocation ScanLoc, string left)
        {
            
            xy[0] = ScanLoc.point.x;
            xy[1] = ScanLoc.point.y;
            Point point = new Point(xy[0], xy[1]);
            zproj[0] = ScanLoc.point.z;
            Reproject.ReprojectPoints(xy, zproj, utm17, wgs84, 0, 1);
            IFeature feature = fs.AddFeature(point);
            
            feature.DataRow.BeginEdit();
            feature.DataRow["LATITUDE"] = xy[1];
            feature.DataRow["LONGITUDE"] = xy[0];
            feature.DataRow["left"] = left;
            feature.DataRow["ALTITUDE"] = ScanLoc.point.z;
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
