using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator.Emulator
{
    class EmulatorHandler
    {

        private static IEmulator _selectedEmulator;

        public int ID { get; private set; }
        public static IEmulator SelectedEmulator
        {
            get
            {
                if (_selectedEmulator == null || _selectedEmulator.ID != Properties.Settings.Default.emulator)
                    SetSelectedEmulator();
                return _selectedEmulator;
            }
        }

        private static void SetSelectedEmulator()
        {
            switch (Properties.Settings.Default.emulator)
            {
                case 0:
                    _selectedEmulator = new Trinity335aTDB64();
                    break;
                case 1:
                    _selectedEmulator = new cmangosZero();
                    break;
                case 2:
                    _selectedEmulator = new Azeroth335a();
                    break;
                case 3:
                    _selectedEmulator = new cMangos335a();
                    break;
                case 4:
                    _selectedEmulator = new Trinity335a201901();
                    break;
                    
            }
        }

        public static string GenerateQuery(TrinityItem item)
        {
            if (item.EntryId == 0)
                throw new Exception("Please choose an entry ID.");
            return SelectedEmulator.GenerateQuery(item);
        }

        public static string GenerateQuery(TrinityQuest quest)
        {
            if (quest.EntryId == 0)
                throw new Exception("Please choose an entry ID.");
            return SelectedEmulator.GenerateQuery(quest);
        }

        public static string GenerateQuery(TrinityCreature creature)
        {
            if (creature.Entry == 0)
                throw new Exception("Please choose an entry ID.");
            if (creature.Faction == 0)
                throw new Exception("Please set a faction for your creature.");
            if (creature.ModelIds.GetFirstValue() == 0)
                throw new Exception("Please set a model ID for your creature.");
            if (creature.Name == "")
                throw new Exception("Please set a name for your creature.");

            return SelectedEmulator.GenerateQuery(creature);
        }

        public static string GenerateQuery(LootPage loot)
        {
            if (loot.entryTb.Text == "0")
                throw new Exception("Please choose an entry ID.");

            return SelectedEmulator.GenerateQuery(loot);
        }

        public static string GenerateQuery(VendorPage vendor)
        {
            if (vendor.npcTb.Text == "")
                throw new Exception("Please enter NPC ID.");
            else if (vendor.itemTb.Text == "")
                throw new Exception("Please enter item ID.");

            return SelectedEmulator.GenerateQuery(vendor);
        }
    }
}
