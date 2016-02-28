using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace EasyBike.Models.Contracts
{
    public class OpenSourceBikeShareModel : StationModelBase
    {
        // unused
        //public string standDescription { get; set; }
        //public string standName { get; set; }
        //public string standPhoto { get; set; }

        [JsonProperty(PropertyName = "standId")]
        public override string Id { get; set; }

        [JsonProperty(PropertyName = "bikecount")]
        public override int? AvailableBikes { get; set; }

        public override int? AvailableBikeStands { get; set; }

        public  string lat { get; set; }
        public string lon { get; set; }


        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context)
        {
            Latitude = Math.Round(double.Parse(lat, new CultureInfo("en-US")), 5);
            Longitude = Math.Round(double.Parse(lon, new CultureInfo("en-US")), 5);
        }
    }
}
   

