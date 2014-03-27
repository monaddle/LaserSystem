using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserSystemLibrary
{
    [Serializable]
    public class NmeaSentence
    {
        public DateTime timestamp;
        public String[] buffer;
        public long ticks;

        public NmeaSentence()
        {
        }
    }
}
