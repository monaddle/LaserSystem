using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testcropsensors
{
    [Serializable]
    class ACS430Reading
    {
        public double RedEdge;
        public double NIR;
        public double Red;
        public double NDRE;
        public double NDVI;
        public double Milliseconds;
        public ACS430Reading(double milliseconds, double redEdge, double nir, double red, double ndre, double ndvi)
        {
            Milliseconds = milliseconds;
            RedEdge = redEdge;
            NIR = nir;
            Red = red;
            NDRE = ndre;
            NDVI = ndvi;
        }
    }
}
