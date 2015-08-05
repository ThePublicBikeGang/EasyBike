using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Command;
using PublicBikes.Config;
using PublicBikes.Models.Storage;
using PublicBikes.Models;
using System.Linq;
using PublicBikes.Notification;

namespace PublicBikes.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private RelayCommand _showContractsCommand;
        private RelayCommand _clearCacheCommand;
        private readonly INavigationService _navigationService;
        private readonly IConfigService _configService;
        private readonly IStorageService _storageService;
        private readonly IRefreshService _refreshService;
        private readonly ISettingsService _settingsService;
        private readonly INotificationService _notificationService;
        private readonly IDialogService _dialogService;

        public MainViewModel(IDialogService dialogService, INotificationService notificationService, ISettingsService settingsService, INavigationService navigationService, IConfigService configService, IStorageService storageService, IRefreshService refreshService)
        {
            _navigationService = navigationService;
            _configService = configService;
            _storageService = storageService;
            _refreshService = refreshService;
            _settingsService = settingsService;
            _notificationService = notificationService;
            _dialogService = dialogService;

            Init();
            //if (App.LocalSettings.Values["Localization"] != null && (bool)App.LocalSettings.Values["Localization"] == false)
            //{
            //    var dialog = new MessageDialog("Localization in disabled, go to the settings page to activate it ?");
            //    //Commands
            //    dialog.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler((f) => Frame.Navigate(typeof(SettingsPage)))));
            //    dialog.Commands.Add(new UICommand("Cancel"));


            //    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
            //    return;
            //}
        }
        private async void Init()
        {
            MapServiceToken = (await _configService.GetConfigAsync()).WindowsPhoneMapServiceToken;

        }
        public RelayCommand ShowContractsCommand
        {
            get
            {
                return _showContractsCommand
                       ?? (_showContractsCommand = new RelayCommand(
                           () =>
                           {
                               _navigationService.NavigateTo(ViewModelLocator.ContractsPageKey, SimpleIoc.Default.GetInstanceWithoutCaching<ContractsViewModel>());
                           }));
            }
        }

        public RelayCommand ClearCacheCommand
        {
            get
            {
                return _clearCacheCommand
                       ?? (_clearCacheCommand = new RelayCommand(
                           () =>
                           {
                               _storageService.ClearAsync();
                           }));
            }
        }

        private RelayCommand goToDownloadCitiesCommand;
        public RelayCommand GoToDownloadCitiesCommand
        {
            get
            {
                return goToDownloadCitiesCommand
                       ?? (goToDownloadCitiesCommand = new RelayCommand(
                           () =>
                           {
                               _navigationService.NavigateTo(ViewModelLocator.ContractsPageKey, SimpleIoc.Default.GetInstanceWithoutCaching<ContractsViewModel>());
                           }));
            }
        }
        private RelayCommand goToSettingsCommand;
        public RelayCommand GoToSettingsCommand
        {
            get
            {
                return goToSettingsCommand
                       ?? (goToSettingsCommand = new RelayCommand(
                            () =>
                            {
                                _navigationService.NavigateTo(ViewModelLocator.SettingsPageKey, SimpleIoc.Default.GetInstanceWithoutCaching<SettingsViewModel>());
                            }));
            }
        }
        private RelayCommand howToUseThisAppCommand;
        public RelayCommand HowToUseThisAppCommand
        {
            get
            {
                return howToUseThisAppCommand
                       ?? (howToUseThisAppCommand = new RelayCommand(
                           () =>
                           {

                           }));
            }
        }

        private RelayCommand aboutCommand;
        public RelayCommand AboutCommand
        {
            get
            {
                return aboutCommand
                       ?? (aboutCommand = new RelayCommand(
                           () =>
                           {

                           }));
            }
        }

        private RelayCommand goToFavoritsCommand;
        public RelayCommand GoToFavoritsCommand
        {
            get
            {
                return goToFavoritsCommand
                       ?? (goToFavoritsCommand = new RelayCommand(
                           () =>
                           {

                           }));
            }
        }

        private RelayCommand locationCommand;
        public RelayCommand LocationCommand
        {
            get
            {
                return locationCommand
                       ?? (locationCommand = new RelayCommand(
                          async () =>
                           {
                               if (!_settingsService.Settings.IsLocalisationOn)
                               {
                                   _notificationService.Notify(new GoToPageNotification() { PageKey = "SettingsPageKey" });
                                   // Message with custom buttons and callback action.
                                   await _dialogService.ShowMessage("Localization is disabled, go to settings to activate ?",
                                       "Allow localization",
                                       buttonConfirmText: "Yes", buttonCancelText: "No",
                                       afterHideCallback: (confirmed) =>
                                       {
                                           if (confirmed)
                                           {
                                               goToSettingsCommand.Execute(null);
                                           }
                                       });
                               }
                               else
                               {

                               }
                           }));
            }
        }

        private RelayCommand searchClickCommand;
        public RelayCommand SearchClickCommand
        {
            get
            {
                return searchClickCommand
                       ?? (searchClickCommand = new RelayCommand(
                           () =>
                           {

                           }));
            }
        }


        private RelayCommand northIndicatorCommand;
        public RelayCommand NorthIndicatorCommand
        {
            get
            {
                return northIndicatorCommand
                       ?? (northIndicatorCommand = new RelayCommand(
                           () =>
                           {

                           }));
            }
        }

        private RelayCommand flyoutButtonDriveToCommand;
        public RelayCommand FlyoutButtonDriveToCommand
        {
            get
            {
                return flyoutButtonDriveToCommand
                       ?? (flyoutButtonDriveToCommand = new RelayCommand(
                           () =>
                           {

                           }));
            }
        }

        private RelayCommand addToFavCommand;
        public RelayCommand AddToFavCommand
        {
            get
            {
                return addToFavCommand
                       ?? (addToFavCommand = new RelayCommand(
                           () =>
                           {

                           }));
            }
        }

        private RelayCommand favoriteNameButtonOk;
        public RelayCommand FavoriteNameButtonOk
        {
            get
            {
                return favoriteNameButtonOk
                       ?? (favoriteNameButtonOk = new RelayCommand(
                           () =>
                           {

                           }));
            }
        }


        private RelayCommand shareLocationByTextCommand;
        public RelayCommand ShareLocationByTextCommand
        {
            get
            {
                return shareLocationByTextCommand
                       ?? (shareLocationByTextCommand = new RelayCommand(
                           () =>
                           {

                           }));
            }
        }

        private RelayCommand shareLocationByMailCommand;
        public RelayCommand ShareLocationByMailCommand
        {
            get
            {
                return shareLocationByMailCommand
                       ?? (shareLocationByMailCommand = new RelayCommand(
                           () =>
                           {

                           }));
            }
        }
        private RelayCommand favoriteNameTextBox_KeyUpCommand;
        public RelayCommand FavoriteNameTextBox_KeyUpCommand
        {
            get
            {
                return favoriteNameTextBox_KeyUpCommand
                       ?? (favoriteNameTextBox_KeyUpCommand = new RelayCommand(
                           () =>
                           {

                           }));
            }
        }

        private RelayCommand searchLocationPoint_TappedCommand;
        public RelayCommand SearchLocationPoint_TappedCommand
        {
            get
            {
                return searchLocationPoint_TappedCommand
                       ?? (searchLocationPoint_TappedCommand = new RelayCommand(
                           () =>
                           {

                           }));
            }
        }

        private RelayCommand searchLocationPoint_ManipulationStartingCommand;
        public RelayCommand SearchLocationPoint_ManipulationStartingCommand
        {
            get
            {
                return searchLocationPoint_ManipulationStartingCommand
                       ?? (searchLocationPoint_ManipulationStartingCommand = new RelayCommand(
                           () =>
                           {

                           }));
            }
        }

        private RelayCommand addressTextBox_KeyUpCommand;
        public RelayCommand AddressTextBox_KeyUpCommand
        {
            get
            {
                return addressTextBox_KeyUpCommand
                       ?? (addressTextBox_KeyUpCommand = new RelayCommand(
                           () =>
                           {

                           }));
            }
        }


        public string MapServiceToken
        {
            get; set;
        }
    }
}
