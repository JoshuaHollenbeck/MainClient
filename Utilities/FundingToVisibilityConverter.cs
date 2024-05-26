using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MainClient.Utilities
{
    public class FundingToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is null first
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            // If value is not null, cast it to int? since we know it's a nullable int
            int? fundingValue = value as int?;

            // Make sure the parameter is a string and can be parsed to an integer
            if (parameter is string fundingIndexString && int.TryParse(fundingIndexString, out int fundingIndex))
            {
                // If the funding value from the database matches the parameter, return Visible
                return fundingValue.GetValueOrDefault() == fundingIndex ? Visibility.Visible : Visibility.Collapsed;
            }

            // If the parsing fails, return Collapsed
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}