using Android.Gms.Maps.Model;
using Com.Google.Maps.Android.Clustering;

namespace EasyBike.Droid.Models
{
    public class ClusterItem : Java.Lang.Object, IClusterItem
    {
        public LatLng Position { get; set; }

        public ClusterItem(double lat, double lng)
        {
            Position = new LatLng(lat, lng);
        }
    }
}