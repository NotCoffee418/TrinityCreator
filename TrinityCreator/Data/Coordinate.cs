
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Database;

namespace TrinityCreator.Data
{
    public class Coordinate
    {
        private int _x;
        private int _y;
        private int _z;
        private int _mapId;

        public Coordinate()
            : this(0, 0, 0, 0) { }
        public Coordinate(int x, int y, int z)
            : this(x, y, z, 0) { }
        public Coordinate(int x, int y)
            : this(x, y, 0, 0) { }
        public Coordinate(int x = 0, int y = 0, int z = 0, int mapid = 0)
        {
            X = x;
            Y = y;
            Z = z;
            MapId = mapid;
        }
        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = DataType.LimitLength(value, "int(10)");
            }
        }
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = DataType.LimitLength(value, "int(10)");
            }
        }
        public int Z
        {
            get
            {
                return _z;
            }
            set
            {
                _z = DataType.LimitLength(value, "int(10)");
            }
        }
        public int MapId
        {
            get
            {
                return _mapId;
            }
            set
            {
                _mapId = DataType.LimitLength(value, "smallint(5)");
            }
        }
    }
}
