using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.Tools.CreatureCreator.Templates
{
    class RepairVendorTemplate : VendorTemplate
    {
        public RepairVendorTemplate() : base() { }

        public override void LoadTemplate()
        {
            base.LoadTemplate(); // Vendor
            Name = "Repair Vendor";
            SetBmspValues(NpcFlags, new int[] { 4096 });     // Gossip, Vendor
        }
    }
}
