using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MainClient.Utilities
{
    // This converter converts a string to Visibility based on whether it is null or empty.
    public class StringNullOrEmptyToVisibilityConverter : IValueConverter
    {
        // Convert method to convert a string value to Visibility.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert the value to a string.
            var strValue = value as string;

            // If the string is null or empty, return Collapsed (invisible), else return Visible.
            return string.IsNullOrEmpty(strValue) ? Visibility.Collapsed : Visibility.Visible;
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
