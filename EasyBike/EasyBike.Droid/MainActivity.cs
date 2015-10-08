using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using EasyBike.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using EasyBike.Droid.Helpers;

namespace EasyBike.Droid
{
    [Activity (Label = "EasyBike.Droid", MainLauncher = true, Icon = "@drawable/icon")]
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
        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            RefreshButton.SetCommand(
                "Click",
                Vm.GoToDownloadCitiesCommand);
            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button> (Resource.Id.GoToContractView);
			
			button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
			};

        }
	}
}


