using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace EasyBike.Models.Contracts.CH
{
    public class PubliBikeModel
    {
        [JsonProperty(PropertyName = "terminals")]
        public List<Station> Stations { get; set; }
    }

    public class Bike
    {
        [JsonProperty(PropertyName = "available")]
        public int Available { get; set; }
    }

    public class BikeHolder
    {
        [JsonProperty(PropertyName = "holdersfree")]
        public int HoldersFree { get; set; }
    }

    public class Station : StationModelBase
    {
        public override string Id { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "terminalid")]
        public int InnerId { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string InnerStatus { get; set; }

        [JsonProperty(PropertyName = "bikes")]
        public List<Bike> Bikes { get; set; }

        [JsonProperty(PropertyName = "bikeholders")]
        public List<BikeHolder> AvailableDocks { get; set; }

        public override int AvailableBikes { get; set; }

        public override int? AvailableBikeStands { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public override double Latitude { get; set; }

        [JsonProperty(PropertyName = "lng")]
        public override double Longitude { get; set; }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context)
        {
            Longitude = Math.Round(Longitude, 5);
            Latitude = Math.Round(Latitude, 5);
            AvailableBikes = Bikes.Sum(t => t.Available);
            AvailableBikeStands = AvailableDocks.Sum(t => t.HoldersFree);
            Status = InnerStatus == "1" ? true : false;
        }
    }
}


