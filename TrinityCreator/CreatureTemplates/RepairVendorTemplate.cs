using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.CreatureTemplates
{
    class RepairVendorTemplate : VendorTemplate
    {
        public RepairVendorTemplate() : base() { }

        public override void LoadTemplate()
        {
            SetBmspValues(NpcFlags, new int[] { 4096 });     // Gossip, Vendor
            base.LoadTemplate(); // Vendor
        }
    }
}
