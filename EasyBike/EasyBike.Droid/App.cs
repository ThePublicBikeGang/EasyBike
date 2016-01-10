using EasyBike.ViewModels;
using GalaSoft.MvvmLight.Views;
using EasyBike.Droid.Views;
using GalaSoft.MvvmLight.Ioc;
using EasyBike.Services;
using EasyBike.Droid.Services;
using EasyBike.Droid.Helpers;

namespace EasyBike.Droid
{
    public static class App
    {
        private static ViewModelLocator _locator;

        public static ViewModelLocator Locator
        {
            get
            {
                if (_locator == null)
                {
                    // First time initialization
                    var nav = new ExtendedNavigationService();
                    nav.Configure(ViewModelLocator.ContractsPageKey, typeof(ContractsActivity));
                    nav.Configure(ViewModelLocator.AboutPageKey, typeof(AboutActivity));

                    SimpleIoc.Default.Register<INavigationService>(() => nav);
                    SimpleIoc.Default.Register<IDialogService, ExtendedDialogService>();
                    SimpleIoc.Default.Register<ILocalisationService, LocalisationService>();

                    _locator = new ViewModelLocator();
                }

                return _locator;
            }
        }
    }
}