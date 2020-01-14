using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.Tools.CreatureCreator.Templates
{
    class ExampleTemplate : TemplateBase
    {
        public ExampleTemplate() : base() { }

        public override void LoadTemplate()
        {
            MinLevel = 80;
            SetBmspValues(NpcFlags, new int[] { 1, 2, 4 });
            SetComboboxValue(Page.rankCb, 2); // Elite
            SetIKVPValue(Page.movementCb, 1);
        }
    }
}
