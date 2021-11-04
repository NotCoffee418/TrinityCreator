using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using TrinityCreator.Shared.Profiles;

namespace TrinityCreator.Shared.Data
{
    /// <summary>
    /// See documentation for more info on how to use this
    /// </summary>
    public class CustomDisplayField : INotifyPropertyChanged
    {
        // field
        private string _inputValue = string.Empty;

        /// <summary>
        /// Table Name
        /// </summary>
        public string Table { get; private set; }

        /// <summary>
        /// Column Name
        /// </summary>
        public string Column { get; private set; }

        /// <summary>
        /// Display Name
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Full custom field name, used by Exporter
        /// </summary>
        public string FullCustomFieldName { get; private set; }

        /// <summary>
        /// Export type
        /// </summary>
        public Export.C ExportType { get; private set; }

        /// <summary>
        /// Value input by user for export
        /// </summary>
        public string InputValue
        {
            get 
            { 
                return _inputValue;
            }
            set 
            {
                _inputValue = value;
                RaisePropertyChanged("InputValue");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a CustomDisplayField using the information defined in profile
        /// </summary>
        /// <param name="databaseTable">database real table name</param>
        /// <param name="databaseColumn">database real table column</param>
        /// <param name="fullCustomFieldName">eg. Creature.CustomInt.MyCustomDisplayName</param>
        /// <returns></returns>
        public static CustomDisplayField Create(
            string databaseTable,
            string databaseColumn,
            string fullCustomFieldName)
        {
            Regex regex = new Regex(
                @"(Item|Quest|Creature)\.Custom(Int|Float|Text)\.(.+)");

            // return null if not valid or not custom DISPLAY
            if (!regex.IsMatch(fullCustomFieldName))
                return null;
            var result = new CustomDisplayField()
            {
                Table = databaseTable,
                Column = databaseColumn,
                FullCustomFieldName = fullCustomFieldName
            };

            var match = regex.Match(fullCustomFieldName);

            // Deternine the creator / export type
            switch (match.Groups[1].Value)
            {
                case "Item":
                    result.ExportType = Export.C.Item;
                    break;
                case "Quest":
                    result.ExportType = Export.C.Quest;
                    break;
                case "Creature":
                    result.ExportType = Export.C.Creature;
                    break;
                default:
                    throw new ArgumentException(
                        match.Groups[1].Value + " is not a supported creator for custom display fields.");
            }

            // Grab DisplayName
            result.DisplayName = match.Groups[3].Value;

            // Set default value to 0 if type is numeric
            if (match.Groups[2].Value == "Int" || match.Groups[2].Value == "Float")
                result.InputValue = "0";

            // Create object and return
            return result;
        }

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
