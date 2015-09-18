using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace EasyBike.Models.Contracts.DE
{
    public class CallABikeModel
    {
        [JsonProperty(PropertyName = "marker")]
        public Station[] stations { get; set; }
    }

    public class Station : StationModelBase
    {
        [JsonProperty(PropertyName = "lat")]
        public override double Latitude { get; set; }

        [JsonProperty(PropertyName = "lng")]
        public override double Longitude { get; set; }

        [JsonProperty(PropertyName = "hal2option")]
        public Details Details { get; set; }

        public override int AvailableBikes { get; set; }

        public override int? AvailableBikeStands { get; set; }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context)
        {
            Longitude = Math.Round(Longitude, 5);
            Latitude = Math.Round(Latitude, 5);
            AvailableBikes = Details.Bikelist.Length;
            AvailableBikeStands = null;
        }
    }

    public class Details
    {
        [JsonProperty(PropertyName = "bikelist")]
        public object[] Bikelist { get; set; }
    }
}
