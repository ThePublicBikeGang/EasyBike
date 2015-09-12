using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using EasyBike.Config;
using EasyBike.Design;
using EasyBike.Models;
using EasyBike.Notification;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using System.Reflection;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EasyBike.ViewModels
{
    public class ContractsViewModel : ViewModelBase
    {
        private readonly IContractService _contractsService;
        private readonly INavigationService _navigationService;
        private readonly IRefreshService _refreshService;
        private readonly IDialogService _dialogService;
        private readonly INotificationService _notificationService;
        private readonly IConfigService _configService;

        private RelayCommand<Contract> _contractTappedCommand;
        private bool _stopLoadingContracts = false;
        public ObservableCollection<ContractGroup> ContractGroups = new ObservableCollection<ContractGroup>();

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
        public ContractsViewModel(IConfigService configService, INotificationService notificationService, IDialogService dialogService, IContractService contractsService, IRefreshService refreshService, INavigationService navigationService)
        {
            _notificationService = notificationService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _contractsService = contractsService;
            _refreshService = refreshService;
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
                Init();
            }
        }
#endif

        public async void Init()
        {

            var storedContracts = await _contractsService.GetContractsAsync();
            var staticContracts = _contractsService.GetStaticContracts();
            staticContracts = storedContracts.Union(staticContracts).ToList();

            var assembly = typeof(ContractsViewModel).GetTypeInfo().Assembly;
            foreach (var contract in staticContracts.GroupBy(c => c.Country).Select(c => c.First()).OrderBy(c => c.Country).ToList())
            {
                if (_stopLoadingContracts)
                {
                    return;
                }
                var imageMemoryStream = new MemoryStream();
                byte[] imageByteArray = null;
                try
                {
                    using (var stream = assembly.GetManifestResourceStream($"EasyBike.Assets.Flags.{contract.ISO31661}.png"))
                    {
                        stream.CopyTo(imageMemoryStream);
                        imageByteArray = imageMemoryStream.ToArray();
                    }
                }
                catch (Exception e)
                {

                }

                var group = new ContractGroup() { Title = contract.Country, ImageByteArray = imageByteArray };
                group.Items = new ObservableCollection<Contract>();
                foreach (var c in staticContracts.Where(c => c.Country == contract.Country).OrderBy(c => c.Name).ToList())
                {
                    //if (storedContracts.Any(a => a.StorageName == c.StorageName))
                    //{
                    //    c.Downloaded = true;
                    //    c.StationCounter = 
                    //}
                    CityCounter++;
                    group.Items.Add(c);
                    group.ItemsCounter++;
                }
                await Task.Delay(1);
                try
                {
                    ContractGroups.Add(group);
                }
                catch 
                {

                }
          
            }
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
                                       contract.Downloading = true;
                                       var stations = await contract.GetStationsAsync();
                                       contract.Stations = stations;
                                       contract.StationCounter = stations.Count();
                                       contract.Downloaded = true;
                                       contract.Downloading = false;
                                       await _contractsService.AddContractAsync(contract);
                                   }
                               }
                               catch (Exception e)
                               {
                                   var message = $"Sorry, you are currently not able to download {contract.Name}.";
                                   try
                                   {
                                       // this can throw a application called an interface that was marshalled for a different thread. (Exception from HRESULT: 0x8001010E (RPC_E_WRONG_THREAD))
                                       // better to catch this as I have found a way to force it to the UI thread from the cross platform library
                                       await _dialogService.ShowMessage(message,
                                         "Confirmation",
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
