using Android.App;
using Android.Widget;
using Android.OS;
using EasyBike.ViewModels;
using GalaSoft.MvvmLight.Helpers;

namespace EasyBike.Droid
{
    // TODO 
    // Attribution Requirements
    // If you use the Google Maps Android API in your application, you must include the Google Play Services attribution text as part of a "Legal Notices" section in your application.Including legal notices as an independent menu item, or as part of an "About" menu item, is recommended.
    // The attribution text is available by making a call to GoogleApiAvailability.getOpenSourceSoftwareLicenseInfo.

    [Activity(Label = "EasyBike.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public partial class MainActivity
    {
        int count = 1;
        private Binding _lastLoadedBinding;

        public MainViewModel Vm
        {
            get
            {
                return App.Locator.Main;
            }
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //RefreshButton.SetCommand(
            //    "Click",
            //    Vm.GoToDownloadCitiesCommand);
            // Get our button from the layout resource,
            // and attach an event to it
           // Button button = FindViewById<Button>(Resource.Id.GoToContractView);

            //button.Click += delegate
            //{
            //    button.Text = string.Format("{0} clicks!", count++);
            //};

        }
    }
}


