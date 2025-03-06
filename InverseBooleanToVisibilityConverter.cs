using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MyShop.Converters
{
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        // Convert Boolean to Visibility
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && parameter.ToString() == "Inverse")
            {
                return (value is bool && !(bool)value) ? Visibility.Visible : Visibility.Collapsed;
            }

            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        // Convert Back Visibility to Boolean
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }
}
