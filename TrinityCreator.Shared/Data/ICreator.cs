using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator.Shared.Profiles;

namespace TrinityCreator.Shared.Data
{
    public interface ICreator
    {
        /// <summary>
        /// Creator export type
        /// </summary>
        Export.C ExportType { get; }

        /// <summary>
        /// Defines data for custom display fields
        /// </summary>
        List<CustomDisplayField> CustomDisplayFields { get; }
    }
}
