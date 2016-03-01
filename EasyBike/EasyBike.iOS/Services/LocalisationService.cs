using EasyBike.Models;
using EasyBike.Services;

namespace EasyBike.iOS.Services
{
    public class LocalisationService : ILocalisationService
    {
        public Location GetCurrentMapCenter()
        {
            //if (MainActivity._map != null)
            //{
            //    CameraPosition camPosition = MainActivity._map.CameraPosition;
            //    return new Location() { Latitude = camPosition.Target.Latitude, Longitude = camPosition.Target.Longitude, ZoomLevel = camPosition.Zoom };
            //}
            //else {
            //    return new Location() { Latitude = 48.879918, Longitude = 2.354810, ZoomLevel = 14.5 };
            //}
            return null;
        }
    }
}
