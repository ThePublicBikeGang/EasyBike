
using System.Collections.ObjectModel;

namespace PublicBikes.Models
{
    public class ContractGroup
    {
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
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