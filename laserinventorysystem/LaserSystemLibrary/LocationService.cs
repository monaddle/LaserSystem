using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Data;
using DotSpatial.Topology;

namespace LaserSystemLibrary
{
    public class LocationService
    {
        public string PathToFile;
        public DotSpatial.Data.IFeatureSet fs;
        public IFeatureList fl;
        public Feature lastIntersection = null;
        List<IEnvelope> envelopes;
        public LocationService(string path)
        {
            PathToFile = path;
            fs = FeatureSet.Open(PathToFile);
            fl = fs.Features;
            
            envelopes = fl.AsEnumerable().Select(x => (IEnvelope)x.Envelope).ToList();
            
        }

        public Feature GetLocation(Feature point, double x, double y)
        {
            if (lastIntersection != null)
            {
                if (lastIntersection.Contains(point))
                {
                    return lastIntersection;
                }
                else
                {
                    lastIntersection = null;
                    return null;
                }
            }
            for (int i = 0; i < envelopes.Count; i++)
            {
                if (envelopes[i].Contains(x, y))
                {
                    if (fl[i].Contains(point))
                    {
                        lastIntersection = (Feature)fl[i];
                        return lastIntersection;
                    }
                }
            }
            lastIntersection = null;

            return null;
        }
    }
}
