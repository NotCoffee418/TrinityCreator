using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.Tools.CreatureCreator.Templates
{
    class TrainerTemplate : TemplateBase
    {
        public TrainerTemplate() : base() { }

        public override void LoadTemplate()
        {
            MinLevel = 80;
            MaxLevel = 80;
            Faction = 35;
            SetBmspValues(NpcFlags, new int[] { 1, 2, 16 });      // Gossip, questgiver, Trainer
            SetBmspValues(FlagsExtra, new int[] { 2 });        // Civilian
            SetBmspValues(TypeFlags, new int[] { 134217728 }); // Force gossip
            SetIKVPValue(Page.creatureTypeCb, 7);              // Humanoid
        }
    }
}
