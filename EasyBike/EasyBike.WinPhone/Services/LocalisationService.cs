using EasyBike.Models;
using EasyBike.Services;

namespace EasyBike.WinPhone.Services
{
    public class LocalisationService : ILocalisationService
    {
        public Location GetUserLocation()
        {
            return new Location() { Latitude = MainPage.Map.Center.Position.Latitude, Longitude = MainPage.Map.Center.Position.Longitude, ZoomLevel = MainPage.Map.ZoomLevel };
        }
    }
}
