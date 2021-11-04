using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinityCreator.Shared.Database
{
    class DataType
    {
        /// <summary>
        /// Limits value to max supported by mysql column
        /// </summary>
        /// <param name="value">input value</param>
        /// <param name="mySqlDt">dataType eg: tinyint(3)</param>
        /// <returns></returns>
        public static int LimitLength(int value, string mySqlDt, int overrideMaxValue = 0)
        {
            // handle negative numbers
            bool wasNegative = false;
            if (value < 0)
            {
                wasNegative = true;
                value = Math.Abs(value);
            }

            // calculate maxValue
            int maxValue = 0;
            if (overrideMaxValue != 0)
                maxValue = overrideMaxValue;
            else
            {
                string[] parts = mySqlDt.Split('(');
                string type = parts[0];
                int maxLength = int.Parse(parts[1].Substring(0, parts[1].Length - 1));

                switch (type) // Assumes unsigned, limit to int.
                {
                    case "tinyint":
                        maxValue = 255;
                        break;
                    case "smallint":
                        maxValue = 65535;
                        break;
                    case "mediumint":
                        maxValue = 16777215;
                        break;
                    case "int":
                        maxValue = 2147483647;
                        break;
                    case "bigint":
                        maxValue = 2147483647;
                        break;
                }

                // check length limit
                if (maxValue.ToString().Length > maxLength)
                {
                    string newMaxValue = string.Empty;
                    for (int i = 0; i < maxLength; i++)
                        newMaxValue += '9';
                    maxValue = int.Parse(newMaxValue);
                }
            }

            // Check if value is allowed
            if (maxValue != 0 && value > maxValue) // limit by max value when set
                value = maxValue;

            // back to negative
            if (wasNegative)
                value = 0 - value;

            return value;
        }

        /// <summary>
        /// Chop off part of the string when too long
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string LimitLength(string value, int length)
        {
            if (value.Length > length)
                value = value.Substring(0, length); // Chop off ending when too long
            return value;
        }
    }
}
