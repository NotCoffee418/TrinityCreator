using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.Tools.CreatureCreator.Templates
{
    class BossTemplate : TemplateBase
    {
        public BossTemplate() : base() { }

        public override void LoadTemplate()
        {
            Name = "Raid Boss";
            MinLevel = 83;
            MaxLevel = 83;
            Faction = 168;
            SetIKVPValue(Page.movementCb, 1);                           // Random movement
            Scale = 2.5f;                                               // They tend to get pretty big
            SetIKVPValue(Page.rankCb, 3);                               // boss rank
            SetBmspValues(DynamicFlags, new int[] { 1 });               // lootable
            SetBmspValues(TypeFlags, new int[] { 4, 8 });               // Level ??, no parry animation
            SetBmspValues(MechanicImmuneMask, new int[] {
                1, 2, 4, 8, 16, 32, 64, 128, 256,
                512, 1024, 2048, 4096, 8192, 65536, 131072, 524288,
                4194304, 8388608, 33554432, 67108864, 536870912});      // Relevant immunities
            SetBmspValues(FlagsExtra, new int[] { 1, 1073741824 });     // Bind to instance group & immune to knockback again?
            HealthModifier = 400;
            DamageModifier = 35;
        }
    }
}
