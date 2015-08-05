
using PublicBikes.Models.Storage;
using PublicBikes.Services.Settings;
using System;
using System.Threading.Tasks;

namespace PublicBikes.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IStorageService _storageService;
        private SettingsModel _settings;
        public SettingsModel Settings { get { return _settings; } set { } }

        public SettingsService(IStorageService storageService)
        {
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
    }
}
