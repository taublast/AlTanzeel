using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace AlTanzeel.Converters // Make sure this matches your project's structure
{
    public class BoolToSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected)
            {
                return isSelected ? "✔" : "☐"; // Check Mark or Unchecked Box
            }
            return "☐"; // Default to unchecked
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
