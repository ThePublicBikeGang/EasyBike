using GalaSoft.MvvmLight;
using EasyBike.Models.Storage;
using EasyBike.Services.Settings;
using System;
using System.ComponentModel;
using System.Reactive.Linq;

namespace EasyBike.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private SettingsModel _settings;

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _settings = _settingsService.Settings;

            Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
           .Throttle(TimeSpan.FromMilliseconds(500))
           .Where(e => e.EventArgs.PropertyName == "IsLocalisationOn")
           .Subscribe(e =>
           {
               UpdateSettingsAsync();
           });

            IsLocalisationOn = _settings.IsLocalisationOn;
        }

        private bool _isLocalisationOn;
        public bool IsLocalisationOn
        {
            get { return _isLocalisationOn; }
            set
            {
                if (_isLocalisationOn != value)
                {
                    Set(() => IsLocalisationOn, ref _isLocalisationOn, value);
                    _settings.IsLocalisationOn = value;
                }
            }
        }

        private async void UpdateSettingsAsync()
        {
            await _settingsService.SetSettingsAsync(_settings);
        }
    }
}
