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

    public class BoolToChipSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool isSelected)
            {
                return isSelected ? Colors.GreenYellow : Colors.Wheat; // Change the colors as you like
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
