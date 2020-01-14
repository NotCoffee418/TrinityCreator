using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.Tools.CreatureCreator.Templates
{
    class ProfessionTrainerTemplate : TrainerTemplate
    {
        public ProfessionTrainerTemplate() : base() { }

        public override void LoadTemplate()
        {
            base.LoadTemplate();
            Name = "Profession Trainer";
            SetBmspValues(NpcFlags, new int[] { 64 });      // Class Trainer
            SetIKVPValue(Page.trainerCb, 1);                // Any profession
        }
    }
}
