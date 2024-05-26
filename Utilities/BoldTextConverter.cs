using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MainClient.Utilities
{
    // This converter applies bold font weight to text based on a condition.
    public class BoldTextConverter : IValueConverter
    {
        // Convert method to apply bold font weight based on a condition.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is an integer and the parameter is a string that can be parsed to an integer.
            if (
                value is int intValue
                && parameter is string parameterString
                && int.TryParse(parameterString, out int paramValue)
            )
            {
                // If the value equals the parameter value, return bold font weight, otherwise return normal font weight.
                return intValue == paramValue ? FontWeights.Bold : FontWeights.Normal;
            }
            // If the value or parameter cannot be converted, return normal font weight.
            return FontWeights.Normal;
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
