using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.Tools.CreatureCreator.Templates
{
    class BeastEnemyTemplate : TemplateBase
    {
        public BeastEnemyTemplate() : base() { }

        public override void LoadTemplate()
        {
            Name = "Beast Enemy";
            MinLevel = 80;
            MaxLevel = 80;
            Faction = 168;
            SetIKVPValue(Page.movementCb, 1); // Random movement
            SetIKVPValue(Page.creatureTypeCb, 1); // beast type
            SetBmspValues(DynamicFlags, new int[] { 1 }); // lootable
            SetBmspValues(TypeFlags, new int[] { 1 }); // tamable
            SetBmspValues(UnitFlags, new int[] { 67108864 }); // skinnable
        }
    }
}
