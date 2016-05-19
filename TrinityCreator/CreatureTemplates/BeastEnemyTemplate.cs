using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.CreatureTemplates
{
    class BeastEnemyTemplate : TemplateBase
    {
        public BeastEnemyTemplate() : base() { }

        public override void LoadTemplate()
        {
            MinLevel = 80;
            MaxLevel = 80;
            Faction = 168;
            SetIKVPValue(Page.creatureTypeCb, 1); // beast type
            SetBmspValues(DynamicFlags, new int[] { 1 }); // lootable
            SetBmspValues(TypeFlags, new int[] { 1 }); // tamable
            SetBmspValues(UnitFlags, new int[] { 67108864 }); // skinnable
        }
    }
}
