using System;
using Android.Gms.Maps.Model;

namespace EasyBike.Droid.Helpers
{
    public class MapHelper
    {
        public static LatLngBounds extendLimits(LatLngBounds bounds, int ratio)
        {
            double extendedLongitude = Math.Abs(bounds.Northeast.Longitude
                                 - bounds.Southwest.Longitude) / ratio;

            double extendedLatitude = Math.Abs(bounds.Northeast.Latitude
                                 - bounds.Southwest.Latitude) / ratio;

            LatLng topRight = null;
            LatLng bottomLeft = null;

            // Longitudes
            double topRightLongitude = bounds.Northeast.Longitude + extendedLongitude;
            double bottomLeftLongitude = bounds.Southwest.Longitude - extendedLongitude;

            // latitudes
            double topRightLatitude = bounds.Northeast.Latitude + extendedLatitude;
            double bottomLeftLatitude = bounds.Southwest.Latitude - extendedLatitude;

            topRight = new LatLng(topRightLatitude, topRightLongitude);
            bottomLeft = new LatLng(bottomLeftLatitude, bottomLeftLongitude);

            return new LatLngBounds(bottomLeft, topRight);
        }
    }
}