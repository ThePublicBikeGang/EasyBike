using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Android.Support.V7.App;
using Android.OS;

namespace EasyBike.Droid.Helpers
{
    public class ActivityBaseEx : AppCompatActivity // ActivityBase
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

         /// <summary>
        /// The activity that is currently in the foreground.
        /// </summary>
        public static ActivityBaseEx CurrentActivity
        {
            get;
            private set;
        }

        /// <summary>
        /// If possible, discards the current page and displays the previous page
        /// on the navigation stack.
        /// </summary>
        public static void GoBack()
        {
            if (CurrentActivity != null)
            {
                CurrentActivity.OnBackPressed();
            }
        }

        /// <summary>
        /// Overrides <see cref="Activity.OnResume"/>. If you override
        /// this method in your own Activities, make sure to call
        /// base.OnResume to allow the <see cref="NavigationService"/>
        /// to work properly.
        /// </summary>
        protected override void OnResume()
        {
            CurrentActivity = this;
            
            if (string.IsNullOrEmpty(ActivityKey))
            {
                ActivityKey = NextPageKey;
                NextPageKey = null;
            }

            base.OnResume();
        }

        internal string ActivityKey
        {
            get;
            private set;
        }

        internal static string NextPageKey
        {
            get;
            set;
        }
    }
}