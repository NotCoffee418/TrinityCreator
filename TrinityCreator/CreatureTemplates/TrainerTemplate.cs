using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.CreatureTemplates
{
    class TrainerTemplate : TemplateBase
    {
        public TrainerTemplate() : base() { }

        public override void LoadTemplate()
        {
            MinLevel = 80;
            MaxLevel = 80;
            Faction = 35;
            SetBmspValues(NpcFlags, new int[] { 1, 16 });      // Gossip, Trainer
            SetBmspValues(FlagsExtra, new int[] { 2 });        // Civilian
            SetIKVPValue(Page.creatureTypeCb, 7);              // Humanoid
        }
    }
}
