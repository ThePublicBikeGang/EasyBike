
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace PublicBikes.Models
{
    public class ContractGroup : ObservableObject
    {
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
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
        public int ItemsCounter { get; set; }
        public string ItemsCounterStr
        {
            get
            {
                return $"{ItemsCounter} {(ItemsCounter > 1 ? "cities" : "city")}";
            }
        }
        public ObservableCollection<Contract> Items { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}