using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TrinityCreator.Shared.Data;
using TrinityCreator.Shared.Profiles;
using TrinityCreator.Shared.UI.UIElements;

namespace TrinityCreator.Shared.Helpers
{
    public static class UiHelper
    {
        /// <summary>
        /// Display CustomDisplayFields to a groupbox
        /// </summary>
        public static void PrepareCustomDisplayFields(GroupBox cdfGb, ICreator creator)
        {
            // Clear groupbox, hide if CustomDisplayFields is not applicable
            cdfGb.Content = null;
            if (!creator.CustomDisplayFields.Any())
            {
                cdfGb.Visibility = Visibility.Collapsed;
                return;
            }

            // Display all relevant customdisplayfields
            cdfGb.Visibility = Visibility.Visible;
            StackPanel content = new StackPanel();
            foreach (var field in creator.CustomDisplayFields)
                content.Children.Add(new CustomDisplayFieldControl(field));
            cdfGb.Content = content;
        }
    }
}
