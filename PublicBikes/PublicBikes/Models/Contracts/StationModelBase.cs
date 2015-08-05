using System;
using System.Runtime.Serialization;

namespace PublicBikes.Models.Contracts
{
    public abstract class StationModelBase
    {
        public abstract  double Latitude { get; set; }

        public abstract  double Longitude { get; set; }

        public abstract  int AvailableBikes { get; set; }

        public abstract int? AvailableBikeStands { get; set; }

        public abstract bool Status { get; set; }

        public abstract bool Banking { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Longitude = Math.Round(Longitude, 5);
            Latitude = Math.Round(Latitude, 5);
        }
    }
}
