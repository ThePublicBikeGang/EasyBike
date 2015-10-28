using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Com.Google.Maps.Android.Clustering;
using Com.Google.Maps.Android.Clustering.View;
using Java.Lang;

namespace EasyBike.Droid.Helpers
{
    public class StationRenderer : DefaultClusterRenderer
    {
        public StationRenderer(Context context, GoogleMap map,
                             ClusterManager clusterManager) : base(context, map, clusterManager)
        {

        }
        protected override void OnBeforeClusterItemRendered(Object p0, MarkerOptions markerOptions)
        {
            BitmapDescriptor markerDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueMagenta);
            markerOptions.SetIcon(markerDescriptor);

            base.OnBeforeClusterItemRendered(p0, markerOptions);
        }
    }
}