using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.CreatureTemplates
{
    class ExampleTemplate : TemplateBase
    {
        public ExampleTemplate() : base() { }

        public override void LoadTemplate()
        {
            MinLevel = 80;
            SetBmspValues(NpcFlags, new int[] { 1, 2, 4 });
            SetComboboxValue(Page.rankCb, 2); // Elite
            SetDdcValues(Resistances, new string[] { "22", "123", "456" }); // Create key array when not unique as well
            SetIKVPValue(Page.movementCb, 1);
        }
    }
}
