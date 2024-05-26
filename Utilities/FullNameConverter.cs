using System;
using System.Globalization;
using System.Windows.Data;

namespace MainClient.Utilities
{
    // This converter combines first name, middle name, last name, and suffix into a full name string.
    public class FullNameConverter : IMultiValueConverter
    {
        // Convert method to concatenate the individual name parts into a full name.
        public object Convert(
            object[] values,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            // Extract individual name parts from the input array.
            string firstName = values[0] as string;
            string middleName = values[1] as string;
            string lastName = values[2] as string;
            string suffix = values[3] as string;

            // Concatenate the name parts into a full name string.
            string fullName =
                $"{firstName} {(string.IsNullOrEmpty(middleName) ? "" : middleName + " ")}{lastName} {(string.IsNullOrEmpty(suffix) ? "" : suffix)}";

            // Trim any leading or trailing whitespaces from the full name.
            return fullName.Trim();
        }

        // ConvertBack method is not implemented.
        public object[] ConvertBack(
            object value,
            Type[] targetTypes,
            object parameter,
            CultureInfo culture
        )
        {
            // ConvertBack method is not used and hence not implemented.
            throw new NotSupportedException();
        }
    }
}
