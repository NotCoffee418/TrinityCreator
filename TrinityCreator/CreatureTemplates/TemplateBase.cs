using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrinityCreator;

namespace TrinityCreator.CreatureTemplates
{
    class TemplateBase : TrinityCreature
    {
        public TemplateBase() : base() { }
        
        /// <summary>
        /// Set flags for the given BMSP
        /// </summary>
        /// <param name="bmsp"></param>
        /// <param name="flags"></param>
        public void SetBmspValues(BitmaskStackPanel bmsp, int[] flags)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds index of value & sets it in TrinityCreature
        /// </summary>
        /// <param name="selectedValue">Value of selected item</param>
        /// <param name="creatureFamily"></param>
        public void SetComboboxValue(int selectedValue, IKeyValue[] creatureFamily)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set flags for the given DDC
        /// </summary>
        /// <param name="ddc"></param>
        /// <param name="flags"></param>
        public void SetDdcValues(DynamicDataControl ddc, string[] values, IKeyValue[] ikvKeys = null, string[] stringKeys = null)
        {
            throw new NotImplementedException();
        }
    }
}
