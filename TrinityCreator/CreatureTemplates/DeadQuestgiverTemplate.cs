using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.CreatureTemplates
{
    class DeadQuestgiverTemplate : QuestgiverTemplate
    {
        public DeadQuestgiverTemplate() : base() { }

        public override void LoadTemplate()
        {
            SetBmspValues(TypeFlags, new int[] { 128 });        // Dead interact
            SetBmspValues(DynamicFlags, new int[] { 4, 32 });   // Appear dead
            base.LoadTemplate();
        }
    }
}
