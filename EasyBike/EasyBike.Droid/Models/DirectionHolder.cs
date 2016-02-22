using Android.Gms.Maps.Model;

namespace EasyBike.Droid.Models
{
    public class DirectionHolder
    {
        public string Distance { get; set; }
        public string Duration{ get; set; }
        public PolylineOptions Polylines { get; set; }
        
    }
}