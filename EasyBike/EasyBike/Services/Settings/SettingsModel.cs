
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
    }
}