
using EasyBike.Models;
using Newtonsoft.Json;

namespace EasyBike.Services.Settings
{
    public class SettingsModel
    {
        public bool IsLocalisationOn { get; set; }

        /// <summary>
        /// determine if the user wants to show the availability of bikes or slots.
        /// </summary>
        [JsonIgnore]
        public bool IsBikeMode { get; set; } = true;

        [JsonIgnore]
        public bool IsCompassMode { get; set; } = false;

        public Location LastLocation { get; set; }

        public SettingsModel() {
            LastLocation = new Location(){ Latitude = 48.879918, Longitude = 2.354810, ZoomLevel = 14.5 };
        }
    }
}