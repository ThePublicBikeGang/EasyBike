using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace EasyBike.Models.Contracts.FR
{
    public class VeloPlusModel : StationModelBase
    {
        [JsonProperty(PropertyName = "nb_spot")]
        public override int? AvailableBikeStands { get; set; }

        [JsonProperty(PropertyName = "nb_bike")]
        public override int AvailableBikes { get; set; }

        public override double Latitude { get; set; }

        public override double Longitude { get; set; }

        public override bool Banking { get; set; }

        public override bool Status { get; set; } = true;

        [JsonProperty(PropertyName = "libelle")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "gps")]
        public Location Location { get; set; }


        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context)
        {
            Longitude = Math.Round(Location.Longitude, 5);
            Latitude = Math.Round(Location.Latitude, 5);
        }
    }

    public class Location
    {
        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }
        [JsonProperty(PropertyName = "lng")]
        public double Longitude { get; set; }
    }
}


