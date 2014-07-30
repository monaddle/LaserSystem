using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Data;
namespace LaserSystemLibrary
{
    public class LocationService
    {
        public string PathToFile;
        public DotSpatial.Data.IFeatureSet fs;
        public IFeatureList fl;
        public Feature lastIntersection = null;
        public LocationService(string path)
        {
            PathToFile = path;
            fs = FeatureSet.Open(PathToFile);
            fl = fs.Features;
            
        }

        public Feature GetLocation(Feature point)
        {
            if (lastIntersection != null)
            {
                if (lastIntersection.Contains(point))
                {
                    return lastIntersection;
                }
            }
            foreach (Feature polygon in fl)
            {
                if (polygon.Contains(point))
                {
                    lastIntersection = polygon;
                    return polygon;
                }
            }
            return null;
        }
    }
}
