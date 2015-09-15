using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace EasyBike.Models
{
    public class Country : ObservableObject
    {
        public string Name{ get; set; }
        public string ISO31661 { get; set; }
        public List<Contract> Contracts { get; set; }
        public object ImageByteArray { get; set; }
        private object _imageSource;
        public object ImageSource
        {
            get { return _imageSource; }
            set
            {
                Set(() => ImageSource, ref _imageSource, value);
            }
        }
        public int ItemsCounter { get { return Contracts.Count; }}
        public string ItemsCounterStr
        {
            get
            {
                return $"{ItemsCounter} {(ItemsCounter > 1 ? "cities" : "city")}";
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
