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
                    _selectedEmulator = new Trinity335a();
                    break;
                case 1:
                    _selectedEmulator = new cmangosZero();
                    break;
            }
        }

        public static string GenerateQuery(TrinityItem item)
        {
            return SelectedEmulator.GenerateQuery(item);
        }

        public static string GenerateQuestQuery(TrinityQuest quest)
        {
            return SelectedEmulator.GenerateQuery(quest);
        }

        public static string GenerateCreatureQuery(TrinityCreature creature)
        {
            return SelectedEmulator.GenerateQuery(creature);
        }
    }
}
