using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PublicBikes.WinPhone.Converters
{
    public class NullOrEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            string culture)
        {
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException("The target must be a Visibility");

            if (value == null || string.IsNullOrWhiteSpace(value as string))
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            string culture)
        {
            throw new NotSupportedException();
        }
    }
}


