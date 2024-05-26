using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MainClient.Utilities
{
    // This generic class converts a boolean value to a specified type of value.
    public class BoolToValueConverter<T> : IValueConverter
    {
        // Property representing the value when the boolean value is false.
        public T FalseValue { get; set; }

        // Property representing the value when the boolean value is true.
        public T TrueValue { get; set; }

        // Convert method to convert a boolean value to the specified type of value.
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture
        )
        {
            // Check if the value is null.
            if (value == null)
                return FalseValue;
            else
                // Return TrueValue if the value is true, otherwise return FalseValue.
                return (bool)value ? TrueValue : FalseValue;
        }

        // ConvertBack method to convert the value back to a boolean based on TrueValue.
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture
        )
        {
            // Check if the value is not null and equals to TrueValue.
            return value != null ? value.Equals(TrueValue) : false;
        }
    }

    // Specialized converters for specific types.
    public class BoolToStringConverter : BoolToValueConverter<String> { }

    public class BoolToBrushConverter : BoolToValueConverter<Brush> { }

    public class BoolToFontWeightConverter : BoolToValueConverter<FontWeight> { }
}
