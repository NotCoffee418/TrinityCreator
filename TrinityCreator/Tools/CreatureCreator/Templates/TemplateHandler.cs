using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TrinityCreator.Tools.CreatureCreator.Templates
{
    class TemplateHandler
    {
        private static Dictionary<string, string> _d;

        // Register templates here <Class, Description>
        public static Dictionary<string, string> AllTemplates
        {
            get
            {
                if (_d == null)
                {
                    _d = new Dictionary<string, string>();
                    _d.Add("BlankTemplate", "Default");
                    _d.Add("HumanoidEnemyTemplate", "Humanoid Enemy");
                    _d.Add("BeastEnemyTemplate", "Beast Enemy");
                    _d.Add("BossTemplate", "Raid Boss");
                    _d.Add("QuestgiverTemplate", "Questgiver");
                    _d.Add("DeadQuestgiverTemplate", "Dead Questgiver");
                    _d.Add("EscorteeTemplate", "Quest Escort NPC");
                    _d.Add("VendorTemplate", "Vendor");
                    _d.Add("RepairVendorTemplate", "Repair Vendor");
                    //_d.Add("ClassTrainerTemplate", "Class Trainer");
                    //_d.Add("ProfessionTrainerTemplate", "Profession Trainer");
                    //_d.Add("MountTrainerTemplate", "Mount Trainer");
                }
                return _d;
            }
        }

        public static IEnumerable<string> ListTemplateDescriptions()
        {
            return from tData in AllTemplates select tData.Value;
        }

        public static void LoadTemplateByDescription(string description)
        {
            var kvp = from tData in AllTemplates where tData.Value == description select tData.Key;
            LoadTemplate(kvp.FirstOrDefault());
        }

        public static void LoadTemplate(string className)
        {
            CreatureCreatorPage page = (CreatureCreatorPage)App._MainWindow.CreatureCreatorTab.Content;
            if (page.IsCreatureModified)
            {
                var r = MessageBox.Show("Do you want to discard your unsaved creature changes to load " + className + "?",
                    "Overwrite creature", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.No);
                if (r != MessageBoxResult.Yes)
                    return; // Don't load template
            }
            
            Type type = Assembly.GetExecutingAssembly().GetTypes().First(t => t.Name == className);
            TrinityCreature creature = (TemplateBase)Activator.CreateInstance(type);
            page.Creature = creature;
            page.DataContext = null;
            page.DataContext = page.Creature;
            ((TemplateBase)page.Creature).LoadTemplate();
            page.PrepareCreaturePage();
        }


    }
}
