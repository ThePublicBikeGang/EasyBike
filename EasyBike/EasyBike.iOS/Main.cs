using UIKit;
using EasyBike.ViewModels;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Ioc;
using EasyBike.Services;
using EasyBike.iOS.Services;

namespace EasyBike.iOS
{
	public class Application
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
                    //nav.Configure(ViewModelLocator.ContractsPageKey, typeof(ContractsActivity));
                    //nav.Configure(ViewModelLocator.AboutPageKey, typeof(AboutActivity));
                    //nav.Configure(ViewModelLocator.FavoritesPageKey, typeof(FavoritesActivity));
                    //nav.Configure(ViewModelLocator.HowToPageKey, typeof(TutorialActivity));

                    SimpleIoc.Default.Register<INavigationService>(() => nav);
                    SimpleIoc.Default.Register<IDialogService, DialogService>();
                    SimpleIoc.Default.Register<ILocalisationService, LocalisationService>();

                    _locator = new ViewModelLocator();
                }

                return _locator;
            }
        }

        // This is the main entry point of the application.
        static void Main (string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
