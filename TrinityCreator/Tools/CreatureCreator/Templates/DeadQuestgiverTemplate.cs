using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.Tools.CreatureCreator.Templates
{
    class DeadQuestgiverTemplate : QuestgiverTemplate
    {
        public DeadQuestgiverTemplate() : base() { }

        public override void LoadTemplate()
        {
            base.LoadTemplate();
            Name = "Dead Questgiver";
            SetBmspValues(TypeFlags, new int[] { 128 });        // Dead interact
            SetBmspValues(DynamicFlags, new int[] { 4, 32 });   // Appear dead
            Emote = 65;                                         // state_dead
        }
    }
}
