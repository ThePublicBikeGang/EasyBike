using EasyBike.Models;
using System;

namespace EasyBike.Helpers
{
    public static class MapHelper
    {
        /// <summary>
        /// found of lot of different radius... hard to tell which one is the most accurate
        /// </summary>
        public const double EarthRadius = 6378137;
        public const double MinLatitude = -85.05112878;
        public const double MaxLatitude = 85.05112878;
        public const double MinLongitude = -180;
        public const double MaxLongitude = 180;


        public const double EarthRadiusInMiles = 3956.0;
        public const double EarthRadiusInKilometers = 6367.0;
        public const double EARTH_RADIUS_KM = 6371;
        public const double radius = EarthRadiusInKilometers;

        //helper method to make reading the lambda a bit easier
        public static double ToRadian(double val) { return val * (Math.PI / 180); }
        //helper method for converting Radians, making the lamda easier to read
        public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }

        public static double Rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        /// <summary> 
        /// Calculate the distance between two geocodes.  (haversine)
        /// http://www.movable-type.co.uk/scripts/latlong.html
        /// </summary> 
        public static double CalcDistance(double lat1, double lng1, double lat2, double lng2)
        {
            return radius * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0) + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
        }
    }
}
