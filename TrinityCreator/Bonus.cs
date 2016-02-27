using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator
{
    public abstract class Bonus
    {
        /// <summary>
        /// Database ID of bonus
        /// </summary>
        int Id
        {
            get;
            set;
        }


        /// <summary>
        /// bonus description
        /// </summary>
        string Description
        {
            get;
            set;
        }
    }
}
