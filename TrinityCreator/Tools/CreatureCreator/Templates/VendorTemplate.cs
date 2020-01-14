using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.Tools.CreatureCreator.Templates
{
    class VendorTemplate : TemplateBase
    {
        public VendorTemplate() : base() { }

        public override void LoadTemplate()
        {
            Name = "Vendor";
            MinLevel = 75; // He's seen some bad days but isn't exactly a legendary adventurer
            MaxLevel = 75;
            Faction = 35;
            SetBmspValues(NpcFlags, new int[] { 1, 128 });     // Gossip, Vendor
            SetBmspValues(TypeFlags, new int[] { 134217728 }); // Force gossip
            SetBmspValues(FlagsExtra, new int[] { 2 });        // Civilian
            SetIKVPValue(Page.creatureTypeCb, 7);              // Humanoid
        }
    }
}
