using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AwesomeTodo.Shared.Converters
{
    public class AlwaysVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
