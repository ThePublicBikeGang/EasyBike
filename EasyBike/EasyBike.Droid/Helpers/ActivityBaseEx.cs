using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace EasyBike.Droid.Helpers
{
    public class ActivityBaseEx : ActivityBase //AppCompatActivity 
    {
        public IDialogService Dialog
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IDialogService>();
            }
        }

        public NavigationService GlobalNavigation
        {
            get
            {
                return (NavigationService)ServiceLocator.Current.GetInstance<INavigationService>();
            }
        }
    }
}