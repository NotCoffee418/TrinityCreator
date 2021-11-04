using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TrinityCreator.Shared.Profiles;
using TrinityCreator.Shared.UI.UIElements;

namespace TrinityCreator.Shared.Helpers
{
    public static class UiHelper
    {
        /// <summary>
        /// Display CustomDisplayFields to a groupbox
        /// </summary>
        public static void PrepareCustomDisplayFields(GroupBox cdfGb, Export.C type)
        {
            // Grab all custom display fields for defined type
            var customDisplayFields = Profile.Active.CustomDisplayFields
                .Where(x => x.ExportType == type)
                .ToList();

            // Clear groupbox, hide if CustomDisplayFields is not applicable
            cdfGb.Content = null;
            if (!customDisplayFields.Any())
            {
                cdfGb.Visibility = Visibility.Collapsed;
                return;
            }

            // Display all relevant customdisplayfields
            cdfGb.Visibility = Visibility.Visible;
            StackPanel content = new StackPanel();
            foreach (var field in customDisplayFields)
                content.Children.Add(new CustomDisplayFieldControl(field));
            cdfGb.Content = content;
        }
    }
}
