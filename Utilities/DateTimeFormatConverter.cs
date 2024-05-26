using System;
using System.Globalization;
using System.Windows.Data;

namespace MainClient.Utilities
{
    // This converter formats DateTime values into a specific date format.
    public class DateTimeFormatConverter : IValueConverter
    {
        // Convert method to format DateTime value into a specific date format.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is a DateTime.
            if (value is DateTime dateTime)
            {
                // Format the DateTime value into "MM/dd/yyyy" format and return it as a string.
                return dateTime.ToString("MM/dd/yyyy");
            }
            // Return null if the value is not a DateTime.
            return null;
        }

        // ConvertBack method to parse a string into a DateTime object.
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            // Check if the value is a string and can be parsed into a DateTime using the specified format.
            if (
                value is string dateString
                && DateTime.TryParseExact(
                    dateString,
                    "MM/dd/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime dateTime
                )
            )
            {
                // If successful, return the parsed DateTime.
                return dateTime;
            }
            // Return the original value if it cannot be converted back.
            return value;
        }
    }
}
