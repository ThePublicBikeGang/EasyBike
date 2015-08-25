using System;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using System.Diagnostics;

namespace EasyBike.WinPhone.Converters
{
    public class MemoryStreamToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            string culture)
        {
            if (targetType != typeof(ImageSource))
                throw new InvalidOperationException("The target must be a MemoryStream");

            if (value == null)
                return null;

            var bitmap = new BitmapImage();
            Debug.WriteLine("tetst" + (value as byte[]).Length);
            using (var ms = new MemoryStream((value as byte[])))
            {
                bitmap.SetSource(WindowsRuntimeStreamExtensions.AsRandomAccessStream(ms));
            }
            Debug.WriteLine("tetst"+bitmap.PixelHeight);

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            string culture)
        {
            throw new NotSupportedException();
        }
    }
}
