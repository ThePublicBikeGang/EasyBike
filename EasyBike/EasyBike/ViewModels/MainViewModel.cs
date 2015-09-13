using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Command;
using EasyBike.Config;
using EasyBike.Models.Storage;
using EasyBike.Models;
using EasyBike.Notification;
using System;
using EasyBike.Models.Favorites;

namespace EasyBike.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private RelayCommand _showContractsCommand;
        private RelayCommand _clearCacheCommand;
        private readonly INavigationService _navigationService;
        private readonly IConfigService _configService;
        private readonly IStorageService _storageService;
        private readonly ISettingsService _settingsService;
        private readonly INotificationService _notificationService;
        private readonly IDialogService _dialogService;
        public event EventHandler FireGoToFavorite;

        public MainViewModel(IDialogService dialogService, INotificationService notificationService, ISettingsService settingsService, INavigationService navigationService, IConfigService configService, IStorageService storageService)
        {
            _navigationService = navigationService;
            _configService = configService;
            _storageService = storageService;
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

        public void GoToFavorite(Favorite favorite)
        {
            FireGoToFavorite?.Invoke(favorite, EventArgs.Empty);
        }


        private bool firstLoad;
        private RelayCommand mainPageLoadedCommand;
        public RelayCommand MainPageLoadedCommand
        {
            get
            {
                return mainPageLoadedCommand
                       ?? (mainPageLoadedCommand = new RelayCommand(
                           async () =>
                           {
                               if (!firstLoad)
                               {
                                   var contracts = await SimpleIoc.Default.GetInstance<IContractService>().GetContractsAsync();
                                   if (contracts.Count == 0)
                                   {
                                       try
                                       {
                                           // this can throw a application called an interface that was marshalled for a different thread. (Exception from HRESULT: 0x8001010E (RPC_E_WRONG_THREAD))
                                           // better to catch this as I have found a way to force it to the UI thread from the cross platform library
                                           await _dialogService.ShowMessage("To get started, you will have to download at least one city. Let's check if your city is in the current list.",
                                              "Welcome to EasyBike !");
                                           GoToDownloadCitiesCommand.Execute(null);
                                       }
                                       catch { }
                                   }
                                   firstLoad = true;
                               }
                           }));
            }
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
                               _navigationService.NavigateTo(ViewModelLocator.HowToPageKey);
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
                               _navigationService.NavigateTo(ViewModelLocator.AboutPageKey, SimpleIoc.Default.GetInstanceWithoutCaching<AboutViewModel>());
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
                               _navigationService.NavigateTo(ViewModelLocator.FavoritesPageKey, SimpleIoc.Default.GetInstanceWithoutCaching<FavoritesViewModel>());
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
