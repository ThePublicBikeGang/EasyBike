using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace EasyBike.Models.Contracts.CN
{
    public class PublicBicycleModel : StationModelBase
    {
        [JsonProperty(PropertyName = "lat")]
        public override double Latitude { get; set; }

        [JsonProperty(PropertyName = "lng")]
        public override double Longitude { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int InnerId { get; set; }

        [JsonProperty(PropertyName = "capacity")]
        public int Capacity { get; set; }

        [JsonProperty(PropertyName = "availBike")]
        public override int? AvailableBikes { get; set; }

        public override int? AvailableBikeStands { get; set; }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context)
        {
            Longitude = Math.Round(Longitude, 5);
            Latitude = Math.Round(Latitude, 5);

            // reverse some lat and long as for exemple Daxing have bad values ...
            if (Latitude > 90 || Latitude < -90)
            {
                var tempLat = Latitude;
                Latitude = Longitude;
                Longitude = tempLat;
            }

            Id = InnerId.ToString();
        }
    }
}
