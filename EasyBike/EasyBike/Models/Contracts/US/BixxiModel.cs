using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace EasyBike.Models.Contracts.US
{
    public class BixxiModel : StationModelBase
    {
        [JsonProperty(PropertyName = "nbBikes")]
        public override int? AvailableBikes { get; set; }

        [JsonProperty(PropertyName = "nbEmptyDocks")]
        public override int? AvailableBikeStands { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public override double Latitude { get; set; }

        [JsonProperty(PropertyName = "long")]
        public override double Longitude { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "locked")]
        public string Locked { get; set; }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context)
        {
            base.OnDeserializedMethod(context);
            Status = Locked == "false" ? true : false;
        }
    }
}
