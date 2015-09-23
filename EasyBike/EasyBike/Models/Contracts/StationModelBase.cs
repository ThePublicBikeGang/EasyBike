using System;
using System.Runtime.Serialization;

namespace EasyBike.Models.Contracts
{
    public abstract class StationModelBase
    {
        public abstract int? AvailableBikes { get; set; }
        public abstract int? AvailableBikeStands { get; set; }
        public abstract double Latitude { get; set; }
        public abstract double Longitude { get; set; }
        public virtual bool Status { get; set; } = true;
        public virtual bool Banking { get; set; }
        public virtual string Id { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Longitude = Math.Round(Longitude, 5);
            Latitude = Math.Round(Latitude, 5);
        }
    }
}
