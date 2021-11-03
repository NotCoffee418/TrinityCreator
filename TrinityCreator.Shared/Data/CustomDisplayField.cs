using System;
using System.Text.RegularExpressions;
using TrinityCreator.Shared.Profiles;

namespace TrinityCreator.Shared.Data
{
    /// <summary>
    /// See documentation for more info on how to use this
    /// </summary>
    public class CustomDisplayField
    {

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
        /// Add quotes around the value when exporting?
        /// </summary>
        public bool ShouldAddQuotes { get; private set; }

        /// <summary>
        /// Export type
        /// </summary>
        public Export.C ExportType { get; private set; }

        /// <summary>
        /// Creates a CustomDisplayField using the information defined in profile
        /// </summary>
        /// <param name="databaseTable">database real table name</param>
        /// <param name="databaseColumn">database real table column</param>
        /// <param name="fullCustomFieldName">eg. Creature.CustomNumber.MyCustomDisplayName</param>
        /// <returns></returns>
        public static CustomDisplayField Create(
            string databaseTable,
            string databaseColumn,
            string fullCustomFieldName)
        {
            Regex regex = new Regex(
                @"(Item|Quest|Creature)\.Custom(Number|Text)\.(.+)");

            // return null if not valid or not custom DISPLAY
            if (!regex.IsMatch(fullCustomFieldName))
                return null;
            var result = new CustomDisplayField()
            {
                Table = databaseTable,
                Column = databaseColumn
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

            // Deternine ShouldAddQuotes
            result.ShouldAddQuotes = match.Groups[2].Value == "Text";

            // Grab DisplayName
            result.DisplayName = match.Groups[3].Value;

            // Create object and return
            return result;
        }
    }
}
