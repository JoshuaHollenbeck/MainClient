using System;
using System.Globalization;
using System.Windows.Data;

namespace MainClient.Utilities
{
    // This converter formats currency values by adding a dollar sign.
    public class CurrencyFormatConverter : IValueConverter
    {
        // Convert method to add a dollar sign to the value.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is null.
            if (value == null)
            {
                // If null, return an empty string.
                return string.Empty;
            }
            else
            {
                // If not null, return a dollar sign.
                return "$";
            }
        }

        // ConvertBack method is not implemented.
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            // ConvertBack method is not used and hence not implemented.
            throw new NotImplementedException();
        }
    }
}
