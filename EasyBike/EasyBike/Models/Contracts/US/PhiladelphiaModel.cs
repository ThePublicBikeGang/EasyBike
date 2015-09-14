using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EasyBike.Models.Contracts.US
{
    public class PhiladelphiaModel
    {
        [JsonProperty(PropertyName = "features")]
        public List<Feature> Features { get; set; }
    }

    public class Properties
    {
        [JsonProperty(PropertyName = "location_name")]
        public string LocationName { get; set; }

        [JsonProperty(PropertyName = "bikesAvailable")]
        public int AvailableBikes { get; set; }

        [JsonProperty(PropertyName = "docksAvailable")]
        public int AvailableBikeStands { get; set; }
    }
 
    public class Geometry
    {
        [JsonProperty(PropertyName = "coordinates")]
        public double[] Location { get; set; }
    }

    public class Feature : StationModelBase
    {
        [JsonProperty(PropertyName = "geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int internalId { get; set; }

        [JsonProperty(PropertyName = "properties")]
        public Properties Properties { get; set; }

        public override int AvailableBikes { get; set; }

        public override int? AvailableBikeStands { get; set; }

        public override bool Banking { get; set; }

        public override double Latitude { get; set; }

        public override double Longitude { get; set; }

        public override bool Status { get; set; }

        public override string Id { get; set; }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context)
        {
            Longitude = Math.Round(Geometry.Location[0], 5);
            Latitude = Math.Round(Geometry.Location[1], 5);
            AvailableBikes = Properties.AvailableBikes;
            AvailableBikeStands = Properties.AvailableBikeStands;
           // Status = status == "OPEN" ? true : false;
        }
    }
}
