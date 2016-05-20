using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.CreatureTemplates
{
    class MountTrainerTemplate : TrainerTemplate
    {
        public MountTrainerTemplate() : base() { }

        public override void LoadTemplate()
        {
            base.LoadTemplate();
            Name = "Mount Trainer";
            SetIKVPValue(Page.trainerCb, 16);   // Any race
        }
    }
}
