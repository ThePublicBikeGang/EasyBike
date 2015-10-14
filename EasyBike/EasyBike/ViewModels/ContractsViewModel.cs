using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using EasyBike.Config;
using EasyBike.Design;
using EasyBike.Models;
using EasyBike.Notification;
using System;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Connectivity.Plugin;

namespace EasyBike.ViewModels
{
    public class ContractsViewModel : ViewModelBase
    {
        private readonly IContractService _contractsService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly INotificationService _notificationService;
        private readonly IConfigService _configService;

        private RelayCommand<Contract> _contractTappedCommand;
        private bool _stopLoadingContracts = false;

        private int cityCounter;
        public int CityCounter
        {
            get { return cityCounter; }
            set
            {
                Set(() => CityCounter, ref cityCounter, value);
            }
        }

        [PreferredConstructor]
        public ContractsViewModel(IConfigService configService, INotificationService notificationService, IDialogService dialogService, IContractService contractsService, INavigationService navigationService)
        {
            _notificationService = notificationService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _contractsService = contractsService;
            _configService = configService;

        }

#if DEBUG
        // This constructor is used in the Windows Phone app at design time,
        // for the Blend visual designer.

        public ContractsViewModel()
        {
            if (IsInDesignMode)
            {
                
                _contractsService = new DesignContractsService();
                GetCountries();
            }
        }
#endif

        public async Task<List<Country>> GetCountries()
        {
            var storedContracts = await _contractsService.GetContractsAsync();
            var countries = _contractsService.GetCountries();
            var assembly = typeof(ContractsViewModel).GetTypeInfo().Assembly;
            foreach(var country in countries)
            {
                
                try
                {
                    using (var stream = assembly.GetManifestResourceStream($"EasyBike.Assets.Flags.{country.ISO31661}.png"))
                    {
                        using(var imageMemoryStream = new MemoryStream())
                        {
                            stream.CopyTo(imageMemoryStream);
                            country.ImageByteArray = imageMemoryStream.ToArray();
                        }
                    }
                }
                catch { }

                country.Contracts = country.Contracts.OrderBy(c => c.Name).ToList();
                for (int i = 0; i< country.Contracts.Count; i++)
                {
                    foreach(var storedContract in storedContracts)
                    {
                        if(storedContract.Name == country.Contracts[i].Name)
                        {
                            country.Contracts[i] = storedContract;
                            break;
                        }
                    }
                    cityCounter++;
                    if (_stopLoadingContracts)
                    {
                        return countries;
                    }
                }
            }
            return countries;
        }

        private RelayCommand _stopLoadingContractsCommand;
        public RelayCommand StopLoadingContractsCommand
        {
            get
            {
                return _stopLoadingContractsCommand
                       ?? (_stopLoadingContractsCommand = new RelayCommand(() =>
                       {
                           _stopLoadingContracts = true;
                       }
                    ));
            }
        }

        private RelayCommand _clearAllDownloadedContractsCommand;
        public RelayCommand ClearAllDownloadedContractsCommand
        {
            get
            {
                return _clearAllDownloadedContractsCommand
                       ?? (_clearAllDownloadedContractsCommand = new RelayCommand(async () =>
                       {
                           var storedContracts = await _contractsService.GetContractsAsync();
                           foreach (Contract c in storedContracts)
                           {
                               c.Downloaded = false;
                           }

                           await _contractsService.RemoveAllContractsAsync();
                       }
                    ));
            }
        }

        

        public RelayCommand<Contract> ContractTappedCommand
        {
            get
            {
                return _contractTappedCommand
                       ?? (_contractTappedCommand = new RelayCommand<Contract>(
                           async (contract) =>
                           {
                               if (contract.Downloading)
                               {
                                   return;
                               }
                               try
                               {
                                   if (contract.Downloaded)
                                   {
                                       await _contractsService.RemoveContractAsync(contract);
                                       contract.Downloaded = false;
                                   }
                                   else
                                   {
                                       //if (CrossConnectivity.Current.IsConnected)
                                       //{
                                           contract.Downloading = true;
                                           var stations = await contract.GetStationsAsync();
                                           contract.Stations = stations;
                                           contract.StationCounter = stations.Count();
                                           contract.Downloaded = true;
                                           contract.Downloading = false;
                                           await _contractsService.AddContractAsync(contract);
                                       //}
                                       //else
                                       //{
                                       //    try
                                       //    {
                                       //        await _dialogService.ShowMessage("Apparently you network connection is off. I'm afraid you'll not be able to download a city if you don't have any active network connection.", "Oops !");
                                       //    }
                                       //    catch { }
                                       //}
                                     
                                   }
                               }
                               catch (Exception e)
                               {
                                   var message = $"You seems to struggle downloading {contract.Name}. It may be either that your network connection isn't healthy, the service provider is down or it has changed. If you think it could be the last option, then contact us to and we will investigate. Thumbs up !";
                                   try
                                   {
                                       // this can throw a application called an interface that was marshalled for a different thread. (Exception from HRESULT: 0x8001010E (RPC_E_WRONG_THREAD))
                                       // better to catch this as I have found a way to force it to the UI thread from the cross platform library
                                       await _dialogService.ShowMessage(message,
                                         "Oops !",
                                         buttonConfirmText: "Ok", buttonCancelText: "Contact us",
                                         afterHideCallback: (confirmed) =>
                                         {
                                             if (!confirmed)
                                             {
                                                 _notificationService.Notify(new SendEmailNotification()
                                                 {
                                                     Subject = $"Download city: unable to download {contract.Name}",
                                                     Body = $"Hey folks ! I'm unable to download {contract.Name} with the following error: " +
                                                      $"Message: { e.Message } Inner: {e.InnerException} Stack: {e.StackTrace}",
                                                 });
                                             }
                                         });
                                   }
                                   catch { }
                               }
                               finally
                               {
                                   contract.Downloading = false;
                               }
                           }));
            }
        }
    }
}
