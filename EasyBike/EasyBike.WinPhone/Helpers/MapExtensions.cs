using EasyBike.Helpers;
using EasyBike.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;

namespace EasyBike.WinPhone.Helpers
{
    /// <summary>
    /// Helper methods to easily set and get view area in a more or less consistent way
    /// </summary>
    public static class MapExtensions
    {
        public static bool IsLocationVisible(this MapControl map, Point point)
        {
            bool isInView;
            Geopoint location;
            map.GetLocationFromOffset(point, out location);
            map.IsLocationInView(location, out isInView);
            return isInView;
        }

        public static bool Contains(List<Geopoint> polyPoints, double latitude, double longitude)
        {
            bool inside = false;
            Geopoint p1, p2;

            //iterate each side of the polygon
            Geopoint oldPoint = polyPoints[polyPoints.Count - 1];

            foreach (Geopoint newPoint in polyPoints)
            {
                //order points so p1.lat <= p2.lat;
                if (newPoint.Position.Latitude > oldPoint.Position.Latitude)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                //test if the line is crossed and if so invert the inside flag.
                if ((newPoint.Position.Latitude < latitude) == (latitude <= oldPoint.Position.Latitude)
                    && (longitude - p1.Position.Longitude) * (p2.Position.Latitude - p1.Position.Latitude)
                     < (p2.Position.Longitude - p1.Position.Longitude) * (latitude - p1.Position.Latitude))
                {
                    inside = !inside;
                }

                oldPoint = newPoint;
            }

            return inside;
        }


        public static bool Contains(List<Geopoint> polyPoints, Geopoint point)
        {
            bool inside = false;
            Geopoint p1, p2;

            //iterate each side of the polygon
            Geopoint oldPoint = polyPoints[polyPoints.Count - 1];

            foreach (Geopoint newPoint in polyPoints)
            {
                //order points so p1.lat <= p2.lat;
                if (newPoint.Position.Latitude > oldPoint.Position.Latitude)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                //test if the line is crossed and if so invert the inside flag.
                if ((newPoint.Position.Latitude < point.Position.Latitude) == (point.Position.Latitude <= oldPoint.Position.Latitude)
                    && (point.Position.Longitude - p1.Position.Longitude) * (p2.Position.Latitude - p1.Position.Latitude)
                     < (p2.Position.Longitude - p1.Position.Longitude) * (point.Position.Latitude - p1.Position.Latitude))
                {
                    inside = !inside;
                }

                oldPoint = newPoint;
            }

            return inside;
        }

        public static Point GetOffsetLocation(this Station station, MapControl map)
        {
            if (station.OffsetLocation == null)
            {
                Point p;
                map.GetOffsetFromLocation(new Geopoint(new BasicGeoposition() { Latitude = station.Latitude, Longitude = station.Longitude }), out p);
                station.OffsetLocation = p;
            }
            return (Point)station.OffsetLocation;
        }
        public static Point GetOffsetLocation2(this Station station, BasicGeoposition origin, double zoomLevel)
        {
            if (station.OffsetLocation == null)
            {
                station.OffsetLocation = origin.GetOffsetedLocation(new BasicGeoposition() { Latitude = station.Latitude, Longitude = station.Longitude }, zoomLevel);
            }
            return (Point)station.OffsetLocation;
        }


        public static double GetDistanceTo(this Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        public static GeoboundingBox GetViewArea(this MapControl map)
        {
            Geopoint p1, p2;
            map.GetLocationFromOffset(new Point(0, 0), out p1);
            map.GetLocationFromOffset(new Point(map.ActualWidth, map.ActualHeight), out p2);
            return new GeoboundingBox(p1.Position, p2.Position);
        }
        public static List<Geopoint> GetViewLocations(this MapControl map)
        {
            Geopoint p1, p2, p3, p4;
            try
            {
                map.GetLocationFromOffset(new Point(0, 0), out p1);
                map.GetLocationFromOffset(new Point(map.ActualWidth, 0), out p2);
                map.GetLocationFromOffset(new Point(map.ActualWidth, map.ActualHeight), out p3);
                map.GetLocationFromOffset(new Point(0, map.ActualHeight), out p4);
                var points = new List<Geopoint>(4) { p1, p2, p3, p4 };
                return points;
            }
            catch (Exception e)
            {
                return null;
            }


        }

        public static string ParseMapLocationFinderResultAddress(this MapLocationFinderResult result)
        {
            var address = result.Locations[0].Address.StreetNumber + " " + result.Locations[0].Address.Street
                    + " " + result.Locations[0].Address.PostCode + " " + result.Locations[0].Address.Town;
            if (!string.IsNullOrWhiteSpace(address))
                address += " " + result.Locations[0].Address.Country;
            return address;
        }

        public static GeoboundingBox GetAreaFromLocations(List<Geopoint> geopoints)
        {
            var northWestLong = geopoints.Min(g => g.Position.Longitude);
            var northWestLat = geopoints.Max(g => g.Position.Latitude);
            var southEstLong = geopoints.Max(g => g.Position.Longitude);
            var southEstLat = geopoints.Min(g => g.Position.Latitude);
            return new GeoboundingBox(new BasicGeoposition()
            {
                Latitude = northWestLat,
                Longitude = northWestLong
            }, new BasicGeoposition() { Latitude = southEstLat, Longitude = southEstLong });
        }



        public static void SetViewArea(this MapControl map, Geopoint p1, Geopoint p2)
        {
            var b = GeoboundingBox.TryCompute(new[] { p1.Position, p2.Position });
            map.TrySetViewBoundsAsync(b, new Thickness(1.0), MapAnimationKind.Bow);
        }

        public static bool Contains(Geopoint location, GeoboundingBox geoBox)
        {
            return (location.Position.Longitude >= geoBox.NorthwestCorner.Longitude &&
                     location.Position.Longitude <= geoBox.SoutheastCorner.Longitude &&
                     location.Position.Latitude <= geoBox.NorthwestCorner.Latitude &&
                     location.Position.Latitude >= geoBox.SoutheastCorner.Latitude);
        }



        //helper method to make reading the lambda a bit easier
        public static double ToRadian(double val) { return val * (Math.PI / 180); }
        //helper method for converting Radians, making the lamda easier to read
        public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }
        /// <summary> 
        /// Calculate the distance between two geocodes. Defaults to using Kilometers. 
        /// </summary> 
        public static double CalcDistance(this BasicGeoposition loc1, BasicGeoposition loc2)
        {
            return MapHelper.CalcDistance(loc1.Latitude, loc1.Longitude, loc2.Latitude, loc2.Longitude);
        }
      

    
        public static double GetDistanceKM(this BasicGeoposition point1, BasicGeoposition point2)
        {
            double dLat = ToRadian(point2.Latitude - point1.Latitude);
            double dLon = ToRadian(point2.Longitude - point1.Longitude);

            double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                       Math.Cos(ToRadian(point1.Latitude)) * Math.Cos(ToRadian(point2.Latitude)) *
                       Math.Pow(Math.Sin(dLon / 2), 2);

            return MapHelper.EARTH_RADIUS_KM * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }


        public static double GetDistancePixel(this BasicGeoposition point1, BasicGeoposition point2, double zoomLevel)
        {
            // 0.1 = pixel par metres à l'échelle de zoom 20
            // comme a chaque echelle de zoom, la carte est deux fois plus grande
            // on calcule la GroundResolution en prenant le nb de pixel par metre à l'échelle de zoom de 20 puis on l'élève à la puissance distante zoom courant
            // point1.GetDistanceKM(point2) * 1000 pour obtenir des metres
            return point1.GetDistanceKM(point2) * 1000 / (0.1 * Math.Pow(2, 20 - zoomLevel));
        }



        /// <summary>
        /// Clips a number to the specified minimum and maximum values.
        /// </summary>
        /// <param name="n">The number to clip.</param>
        /// <param name="minValue">Minimum allowable value.</param>
        /// <param name="maxValue">Maximum allowable value.</param>
        /// <returns>The clipped value.</returns>
        /// <remarks>
        ///     Most helper functions are from MSDN site:
        ///     http://msdn.microsoft.com/en-us/library/bb259689.aspx
        ///</remarks>
        private static double Clip(double n, double minValue, double maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        /// <summary>
        /// Determines the map width and height (in pixels) at a specified level
        /// of detail.
        /// </summary>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail)
        /// to 23 (highest detail).</param>
        /// <returns>The map width and height in pixels.</returns>
        public static uint MapSize(int levelOfDetail)
        {
            return (uint)256 << levelOfDetail;
        }

        /// <summary>
        /// Determines the ground resolution (in meters per pixel) at a specified
        /// latitude and level of detail.
        /// </summary>
        /// <param name="latitude">Latitude (in degrees) at which to measure the
        /// ground resolution.</param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail)
        /// to 23 (highest detail).</param>
        /// <returns>The ground resolution, in meters per pixel.</returns>
        public static double GroundResolution(double latitude, double levelOfDetail)
        {
            latitude = Clip(latitude, MapHelper.MinLatitude, MapHelper.MaxLatitude);
            return Math.Cos(latitude * Math.PI / 180) * 2 * Math.PI * MapHelper.EarthRadius / (256 * Math.Pow(2, levelOfDetail));
        }
        public static double CalculateAngle(this BasicGeoposition loc1, BasicGeoposition loc2)
        {
            double dy = loc2.Latitude - loc1.Latitude;
            double dx = (loc2.Longitude - loc1.Longitude);
            return Math.Atan2(dy, dx);
        }


        /// <summary>
        /// point1 must always be at the 0,0 offseted location 
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Point GetOffsetedLocation(this BasicGeoposition point1, BasicGeoposition point2, double zoomLevel)
        {
            double angle = CalculateAngle(point1, point2);

            double hypotenuse = point1.GetDistancePixel(point2, zoomLevel);
            double x = Math.Cos(angle) * hypotenuse;
            var y = Math.Sqrt(hypotenuse * hypotenuse - x * x);
            return (new Point(x, y));
        }
    }
}