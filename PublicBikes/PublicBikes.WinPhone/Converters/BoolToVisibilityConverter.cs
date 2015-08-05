using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PublicBikes.WinPhone.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            string culture)
        {
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException("The target must be a Visibility");

            if ((bool)value == true)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            string culture)
        {
            throw new NotSupportedException();
        }
    }
}

