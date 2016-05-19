using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.CreatureTemplates
{
    class ProfessionTrainerTemplate : TrainerTemplate
    {
        public ProfessionTrainerTemplate() : base() { }

        public override void LoadTemplate()
        {
            SetBmspValues(NpcFlags, new int[] { 64 });      // Class Trainer
            SetIKVPValue(Page.trainerCb, 1);                // Any profession
            base.LoadTemplate();
        }
    }
}
