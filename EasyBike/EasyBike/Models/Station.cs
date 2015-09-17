using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace EasyBike.Models
{
    public class Station : ObservableObject
    {
        [JsonIgnore]
        public bool IsUiRefreshNeeded;

        [JsonIgnore]
        public bool OnlyColorReload;

        public Contract Contract { get; set; }

        [JsonIgnore]
        public bool DownloadingAvailability { get; set; }


        [JsonIgnore]
        public bool IsInRefreshPool { get; set; }

        [JsonIgnore]
        public object Control;

        [JsonIgnore]
        public bool Loaded { get; set; }

        public string Id { get; set; }

        public int? AvailableBikeStands { get; set; } = -1;

        public int AvailableBikes { get; set; } = -1;

        public string ContractStorageName { get; set; }
        
        private string availableStr;

        [JsonIgnore]
        public string AvailableStr
        {
            get { return availableStr; }
            set
            {
                Set(() => AvailableStr, ref availableStr, value);
            }
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool Status { get; set; }

        public bool Banking { get; set; }

        public bool Locked { get; set; }


        public override int GetHashCode()
        {
            return (Longitude + Latitude).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Longitude == ((obj as Station).Longitude) && Latitude == ((obj as Station).Latitude);
        }


        public object Location { get; set; }

        /// <summary>
        /// specific for China
        /// </summary>
        private object imageNumber;
        [JsonIgnore]
        public object ImageNumber
        {
            get { return imageNumber; }
            set
            {
                Set(() => ImageNumber, ref imageNumber, value);
            }
        }
        [JsonIgnore]
        public object ImageDocks { get; set; }
        [JsonIgnore]
        public object ImageAvailable { get; set; }


        // store the offsetLocation in order to reuse it for each draw cycle
        public object OffsetLocation;
    }
}
