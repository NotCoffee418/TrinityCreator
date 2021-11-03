using System;
using System.Globalization;

namespace DBCViewer
{
    public class BinaryFormatter : IFormatProvider, ICustomFormatter
    {
        // IFormatProvider.GetFormat implementation.
        public object GetFormat(Type formatType)
        {
            // Determine whether custom formatting object is requested.
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        // Format number in binary (B), octal (O), or hexadecimal (H).
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            // Handle format string.
            int baseNumber;
            // Handle null or empty format string, string with precision specifier.
            string thisFmt = String.Empty;
            // Extract first character of format string (precision specifiers
            // are not supported).
            if (!String.IsNullOrEmpty(format))
                thisFmt = format.Length > 1 ? format.Substring(0, 1) : format;

            // Get a byte array representing the numeric value.
            byte[] bytes;
            if (arg is sbyte)
            {
                string byteString = ((sbyte)arg).ToString("X2", CultureInfo.InvariantCulture);
                bytes = new byte[1] { Byte.Parse(byteString, NumberStyles.HexNumber, CultureInfo.InvariantCulture) };
            }
            else if (arg is byte)
            {
                bytes = new byte[1] { (byte)arg };
            }
            else if (arg is short)
            {
                bytes = BitConverter.GetBytes((short)arg);
            }
            else if (arg is int)
            {
                bytes = BitConverter.GetBytes((int)arg);
            }
            else if (arg is long)
            {
                bytes = BitConverter.GetBytes((long)arg);
            }
            else if (arg is ushort)
            {
                bytes = BitConverter.GetBytes((ushort)arg);
            }
            else if (arg is uint)
            {
                bytes = BitConverter.GetBytes((uint)arg);
            }
            else if (arg is ulong)
            {
                bytes = BitConverter.GetBytes((ulong)arg);
            }
            else if (arg is float)
            {
                bytes = BitConverter.GetBytes((float)arg);
            }
            else
            {
                try
                {
                    return HandleOtherFormats(format, arg);
                }
                catch (FormatException e)
                {
                    throw new FormatException(String.Format(CultureInfo.InvariantCulture, "The format of '{0}' is invalid.", format), e);
                }
            }

            switch (thisFmt.ToUpper(CultureInfo.InvariantCulture))
            {
                // Binary formatting.
                case "B":
                    baseNumber = 2;
                    break;
                case "O":
                    baseNumber = 8;
                    break;
                case "H":
                    baseNumber = 16;
                    break;
                // Handle unsupported format strings.
                default:
                    try
                    {
                        return HandleOtherFormats(format, arg);
                    }
                    catch (FormatException e)
                    {
                        throw new FormatException(String.Format(CultureInfo.InvariantCulture, "The format of '{0}' is invalid.", format), e);
                    }
            }

            // Return a formatted string.
            string numericString = String.Empty;
            for (int ctr = bytes.GetUpperBound(0); ctr >= bytes.GetLowerBound(0); ctr--)
            {
                string byteString = Convert.ToString(bytes[ctr], baseNumber);
                if (baseNumber == 2)
                    byteString = new String('0', 8 - byteString.Length) + byteString;
                else if (baseNumber == 8)
                    byteString = new String('0', 4 - byteString.Length) + byteString;
                // Base is 16.
                else
                    byteString = new String('0', 2 - byteString.Length) + byteString;

                numericString += byteString + " ";
            }
            return numericString.Trim();
        }

        private static string HandleOtherFormats(string format, object arg)
        {
            var iFmt = arg as IFormattable;
            if (iFmt != null)
                return iFmt.ToString(format, CultureInfo.CurrentCulture);
            else if (arg != null)
                return arg.ToString();
            else
                return String.Empty;
        }
    }
}
