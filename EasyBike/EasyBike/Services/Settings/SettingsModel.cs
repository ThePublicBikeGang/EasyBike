
namespace EasyBike.Services.Settings
{
    public class SettingsModel
    {
        public bool IsLocalisationOn { get; set; }
        /// <summary>
        /// determine if the user wants to show the availability of bikes or slots.
        /// </summary>
        public bool IsBikeMode { get; set; } = true;

        public bool IsCompassMode { get; set; } = true;
    }
}