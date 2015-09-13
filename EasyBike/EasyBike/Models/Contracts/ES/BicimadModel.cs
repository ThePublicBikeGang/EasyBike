using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EasyBike.Models.Contracts.ES
{
    public class BicimadModel
    {
        [JsonProperty(PropertyName = "estaciones")]
        public List<BicimadStation> Stations
        {
            get; set;
        }
    }

    public class BicimadStation : StationModelBase
    {
        public override string Id { get; set; }

        [JsonProperty(PropertyName = "bicis_enganchadas")]
        public override int AvailableBikes { get; set; }

        [JsonProperty(PropertyName = "bases_libres")]
        public override int? AvailableBikeStands { get; set; }

        public override bool Banking { get; set; }

        [JsonProperty(PropertyName = "latitud")]
        public override double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitud")]
        public override double Longitude { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "activo")]
        public string innerStatus { get; set; }

        [JsonIgnore]
        public override bool Status { get; set; }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context)
        {
            Status = innerStatus == "1" ? true : false;
        }
    }
}
