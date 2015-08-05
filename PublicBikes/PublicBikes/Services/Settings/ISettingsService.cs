using PublicBikes.Services.Settings;
using System.Threading.Tasks;

namespace PublicBikes.Models.Storage
{
    public interface ISettingsService
    {
        Task<SettingsModel> GetSettingsAsync();
        Task SetSettingsAsync(SettingsModel settings);
        SettingsModel Settings { get; set; }
    }
}
