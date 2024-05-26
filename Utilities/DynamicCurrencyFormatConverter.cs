using System;
using System.Globalization;
using System.Windows.Data;

namespace MainClient.Utilities
{
    // This converter formats currency values by adding a currency symbol.
    public class DynamicCurrencyFormatConverter : IMultiValueConverter
    {
        // Convert method that takes in an array of values and returns a formatted currency string.
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the input values are valid and meet the expected criteria.
            if (values.Length > 1 && values[0] is decimal decimalValue)
            {
                // We expect the culture info to be the second value in the array
                if (values[1] is CultureInfo customCulture)
                {
                    // Use the custom culture to format the decimal part of the currency.
                    return decimalValue.ToString("N2", customCulture);
                }
            }
            return null;
        }

        // ConvertBack method required by the IMultiValueConverter interface, but not implemented in this converter.
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // ConvertBack method is not implemented, so throw a NotImplementedException if called.
            throw new NotImplementedException();
        }
    }
}
