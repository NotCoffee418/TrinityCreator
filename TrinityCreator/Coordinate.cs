
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class Coordinate
    {
        public Coordinate()
            : this(0, 0, 0, 0) { }
        public Coordinate(double x, double y, double z)
            : this(x, y, z, 0) { }
        public Coordinate(double x, double y)
            : this(x, y, 0, 0) { }
        public Coordinate(double x = 0, double y = 0, double z = 0, int mapid = 0)
        {
            X = x;
            Y = y;
            Z = z;
            MapId = mapid;
        }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public int MapId { get; set; }
    }
}
