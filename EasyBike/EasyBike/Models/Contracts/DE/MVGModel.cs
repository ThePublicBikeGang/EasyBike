using Newtonsoft.Json;

namespace EasyBike.Models.Contracts.DE
{
    public class MVGModel : StationModelBase
    {
        [JsonProperty(PropertyName = "id")]
        public int InnerId { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string InnerStatus { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public override double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public override double Longitude { get; set; }

        [JsonProperty(PropertyName = "blocked")]
        public bool Locked { get; set; }

        [JsonProperty(PropertyName = "bikes_available")]
        public override int AvailableBikes { get; set; }

        [JsonProperty(PropertyName = "docks_available")]
        public override int? AvailableBikeStands { get; set; }
    }
}

