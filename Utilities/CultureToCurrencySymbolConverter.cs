using System;
using System.Globalization;
using System.Windows.Data;

namespace MainClient.Utilities
{
    // This converter formats currency values by adding a currency symbol.
    public class CultureToCurrencySymbolConverter  : IValueConverter
    {
        // Convert method that takes in an array of values and returns a formatted currency string.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CultureInfo info)
            {
                return info.NumberFormat.CurrencySymbol;
            }
            return null;
        }

        // ConvertBack method required by the IMultiValueConverter interface, but not implemented in this converter.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ConvertBack method is not implemented, so throw a NotImplementedException if called.
            throw new NotImplementedException();
        }
    }
}
