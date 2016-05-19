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
            SetIKVPValue(Page.trainerCb, 15);   // Any race
            base.LoadTemplate();
        }
    }
}
