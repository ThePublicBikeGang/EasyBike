using EasyBike.ViewModels;
using GalaSoft.MvvmLight.Views;
using EasyBike.Droid.Views;
using GalaSoft.MvvmLight.Ioc;

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
                    var nav = new NavigationService();
                    nav.Configure(ViewModelLocator.ContractsPageKey, typeof(ContractsActivity));

                    SimpleIoc.Default.Register<INavigationService>(() => nav);
                    SimpleIoc.Default.Register<IDialogService, DialogService>();

                    _locator = new ViewModelLocator();
                }

                return _locator;
            }
        }
    }
}