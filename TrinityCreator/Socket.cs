using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public class Socket : IKeyValue
    {
        private string _description;
        private int _id;

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public override string ToString()
        {
            return Description;
        }

        public static SocketBonus[] GetSocketList()
        {
            return new SocketBonus[]
            {
                new SocketBonus(1, "Meta"),
                new SocketBonus(2, "Red"),
                new SocketBonus(4, "Yellow"),
                new SocketBonus(8, "Blue"),
            };
        }
    }
}
