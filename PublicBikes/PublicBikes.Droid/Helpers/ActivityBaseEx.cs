using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace PublicBikes.Droid.Helpers
{
    public class ActivityBaseEx : ActivityBase
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