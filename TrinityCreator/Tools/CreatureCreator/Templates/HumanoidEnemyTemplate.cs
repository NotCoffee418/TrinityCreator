using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.Tools.CreatureCreator.Templates
{
    class HumanoidEnemyTemplate : TemplateBase
    {
        public HumanoidEnemyTemplate() : base() { }

        public override void LoadTemplate()
        {
            Name = "Humanoid Enemy";
            MinLevel = 80;
            MaxLevel = 80;
            Faction = 168;
            SetIKVPValue(Page.movementCb, 1); // Random movement
            SetIKVPValue(Page.creatureTypeCb, 7); // humanoid type
            SetBmspValues(DynamicFlags, new int[] { 1 }); // lootable
        }
    }
}
