using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    interface IKeyValue
    {
        int Id
        {
            get; set;
        }

        string Description
        {
            get; set;
        }

        string ToString();
    }
}
