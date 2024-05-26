using System.Windows;
using System.Windows.Controls;

namespace MainClient.Utilities
{
    // This class represents a custom RadioButton control.
    public class Btn : RadioButton
    {
        // Static constructor to override the default style key property.
        static Btn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(Btn),
                new FrameworkPropertyMetadata(typeof(Btn))
            );
        }
    }
}
