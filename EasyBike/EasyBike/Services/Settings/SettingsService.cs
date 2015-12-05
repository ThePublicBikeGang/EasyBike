using EasyBike.Models.Storage;
using EasyBike.Services.Settings;
using System.Threading.Tasks;

namespace EasyBike.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IStorageService _storageService;
        private readonly ILocalisationService _localisationService;
        private SettingsModel _settings;
        public SettingsModel Settings { get { return _settings; } set { } }

        public SettingsService(ILocalisationService localisationService, IStorageService storageService)
        {
            _localisationService = localisationService;
            _storageService = storageService;
            GetSettingsAsync().ConfigureAwait(false);
        }

        public async Task<SettingsModel> GetSettingsAsync()
        {
            if (_settings == null)
            {
                _settings = await _storageService.GetSettingsAsync().ConfigureAwait(false);
            }
            return _settings;
        }

        public async Task SetSettingsAsync(SettingsModel settings)
        {
            await _storageService.SetSettingsAsync(settings).ConfigureAwait(false);
        }

        public async Task SaveSettingAsync()
        {
            var settings = await GetSettingsAsync();
            settings.LastLocation = _localisationService.GetCurrentMapCenter();
            await _storageService.SetSettingsAsync(_settings).ConfigureAwait(false);
        }
    }
}
