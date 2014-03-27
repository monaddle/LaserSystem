using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSGeo.OGR;
//using OSGeo.GDAL;
namespace LaserSystem
{
    class FileGeodatabase
    {
        public FileGeodatabase()
        {
            Ogr.RegisterAll();
            OSGeo.OGR.Driver drv = Ogr.GetDriverByName("ESRI Shapefile");


        }
    }
}
