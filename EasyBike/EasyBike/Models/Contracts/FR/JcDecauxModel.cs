using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace EasyBike.Models.Contracts
{
    public class Position
    {
        [JsonProperty(PropertyName = "lng")]
        public double Longitude { get; set; }
        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }
    }

    public class JcDecauxModel : StationModelBase
    {
        [JsonProperty(PropertyName = "number")]
        public int id { get; set; }

        public override string Id { get; set; }

        [JsonProperty(PropertyName = "available_bike_stands")]
        public override int? AvailableBikeStands { get; set; }

        [JsonProperty(PropertyName = "available_bikes")]
        public override int? AvailableBikes { get; set; }

        public override double Latitude { get; set; }

        public override double Longitude { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string status { get; set; }

        [JsonProperty(PropertyName = "banking")]
        public override bool Banking { get; set; }

        public override bool Status { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "position")]
        public Position Position { get; set; }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context)
        {
            Longitude = Math.Round(Position.Longitude, 5);
            Latitude = Math.Round(Position.Latitude, 5);
            Id = id.ToString();
            Status = status == "OPEN" ? true : false;
        }
    }
}
