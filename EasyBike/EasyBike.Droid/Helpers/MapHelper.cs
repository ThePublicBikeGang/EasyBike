using System;
using Android.Gms.Maps.Model;
using System.Collections.Generic;

namespace EasyBike.Droid.Helpers
{
    public class MapHelper
    {
        public static IList<LatLng> DecodePolyline(string encoded)
        {
            List<LatLng> poly = new List<LatLng>();
            int index = 0, len = encoded.Length;
            int lat = 0, lng = 0;

            while (index < len)
            {
                int b, shift = 0, result = 0;
                do
                {
                    b = encoded[index++] - 63;
                    result |= (b & 0x1f) << shift;
                    shift += 5;
                } while (b >= 0x20);
                int dlat = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
                lat += dlat;

                shift = 0;
                result = 0;
                do
                {
                    b = encoded[index++] - 63;
                    result |= (b & 0x1f) << shift;
                    shift += 5;
                } while (b >= 0x20);
                int dlng = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
                lng += dlng;

                LatLng p = new LatLng((((double)lat / 1E5)), (((double)lng / 1E5)));
                poly.Add(p);
            }
            return poly;
        }

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