using EasyBike.Services;
using EasyBike.Models;

namespace EasyBike.Droid.Services
{
    public class LocalisationService : ILocalisationService
    {
        public Location GetCurrentMapCenter()
        {
            //if (MainPage.Map != null && MainPage.Map.Center != null)
            //{
            //    return new Location() { Latitude = MainPage.Map.Center.Position.Latitude, Longitude = MainPage.Map.Center.Position.Longitude, ZoomLevel = MainPage.Map.ZoomLevel };
            //}
            //else
            //{
                return new Location();
            //}
        }
    }
}
