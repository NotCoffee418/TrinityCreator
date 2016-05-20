using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.CreatureTemplates
{
    class ClassTrainerTemplate : TrainerTemplate
    {
        public ClassTrainerTemplate() : base() { }

        public override void LoadTemplate()
        {
            base.LoadTemplate();
            Name = "Class Trainer";
            SetBmspValues(NpcFlags, new int[] { 32 });      // Class Trainer
            SetIKVPValue(Page.trainerCb, 3);                // Any class
        }
    }
}
