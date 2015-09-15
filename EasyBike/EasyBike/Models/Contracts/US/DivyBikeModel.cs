using Newtonsoft.Json;

namespace EasyBike.Models.Contracts.US
{
    public class DivyBikeModel
    {
        [JsonProperty(PropertyName = "stationBeanList")] 
        public Station[] Stations { get; set; }
    }

    public class Station : StationModelBase
    {
        [JsonProperty(PropertyName = "availableBikes")]
        public override int AvailableBikes { get; set; }

        [JsonProperty(PropertyName = "availableDocks")]
        public override int? AvailableBikeStands { get; set; }

        public override bool Banking { get; set; }

        public override string Id { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int InnerId { get; set; }

        [JsonProperty(PropertyName = "statusKey")]
        public string InnerStatus { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public override double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public override double Longitude { get; set; }

        public override bool Status { get; set; }
    }
}
