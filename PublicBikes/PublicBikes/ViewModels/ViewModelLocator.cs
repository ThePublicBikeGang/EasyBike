using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using PublicBikes.Models;
using PublicBikes.Config;
using PublicBikes.Design;
using GalaSoft.MvvmLight.Views;
using PublicBikes.Models.Storage;
using PublicBikes.Notification;
using PublicBikes.Services;

namespace PublicBikes.ViewModels
{
    public class ViewModelLocator
    {
        public const string AddCommentPageKey = "AddCommentPage";
        public const string ContractsPageKey = "ContractsPage";
        public const string SettingsPageKey = "SettingsPage";

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public ContractsViewModel ContractsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ContractsViewModel>();
            }
        }

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IStorageService, StorageService>();
            SimpleIoc.Default.Register<IRefreshService, RefreshService>(true);
            SimpleIoc.Default.Register<INotificationService, NotificationService>();

            if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IContractService, DesignContractsService>();
                SimpleIoc.Default.Register<INavigationService, DesignNavigationService>();
            }
            else
            {
                SimpleIoc.Default.Register<IContractService, ContractService>(true);
                // SimpleIoc.Default.Register<ContractsViewModel>();
            }

            SimpleIoc.Default.Register<IConfigService, ConfigService>();
            SimpleIoc.Default.Register<ISettingsService, SettingsService>(true);

            // ViewModels
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<ContractsViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
        }
    }
}