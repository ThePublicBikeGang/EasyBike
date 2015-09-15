
using System.Collections.Generic;

namespace EasyBike.Models
{
    public class ContractGroup
    {
        public string Title { get; set; }
        public object ImageByteArray { get; set; }
        private object _imageSource;
        public object ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
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
        public List<Contract> Items { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}