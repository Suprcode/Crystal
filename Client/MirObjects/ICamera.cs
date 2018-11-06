using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MirObjects
{
    public interface ICamera
    {
        Point CurrentLocation { get; set; }
        Point MapLocation { get; set; }

        Point DrawLocation { get; set; }
        Point Movement { get; set; }
        Point FinalDrawLocation { get; set; }
        Point OffSetMove { get; set; }

        string Name { get; set; }

        void Process();
    }
}
