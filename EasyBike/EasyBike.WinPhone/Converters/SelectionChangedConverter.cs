using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace EasyBike.WinPhone.Converters
{
    public class SelectionChangedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = value as SelectionChangedEventArgs;

            if (args.AddedItems.Count != 0)
                return args.AddedItems.FirstOrDefault();

            if (args.RemovedItems.Count != 0)
                return args.RemovedItems.FirstOrDefault();

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            string language)
        {
            throw new NotImplementedException();
        }
    }
}