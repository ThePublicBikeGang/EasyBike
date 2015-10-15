using Android.App;
using Android.Widget;
using Android.OS;
using EasyBike.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using Android.Gms.Maps;
using Android.Support.V7.App;
using System;

namespace EasyBike.Droid
{
    // TODO 
    // Attribution Requirements
    // If you use the Google Maps Android API in your application, you must include the Google Play Services attribution text as part of a "Legal Notices" section in your application.Including legal notices as an independent menu item, or as part of an "About" menu item, is recommended.
    // The attribution text is available by making a call to GoogleApiAvailability.getOpenSourceSoftwareLicenseInfo.

    [Activity(Label = "EasyBike.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public partial class MainActivity : IOnMapReadyCallback
    {
        int count = 1;
        private Binding _lastLoadedBinding;

        private GoogleMap _googleMap;
        private MapFragment _map;

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
            SetContentView(Resource.Layout.Main);

            GoogleMapOptions mapOptions = new GoogleMapOptions()
                .InvokeMapType(GoogleMap.MapTypeNormal)
                .InvokeZoomControlsEnabled(false)
                .InvokeCompassEnabled(true);

            FragmentTransaction fragTx = FragmentManager.BeginTransaction();
            var  _mapFragment = MapFragment.NewInstance(mapOptions);
            fragTx.Add(Resource.Id.map, _mapFragment, "map");
            fragTx.Commit();

            //     MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            //GoogleMap _googleMap = mapFrag.Map;
            //if (_googleMap != null)
            //{
            //    // The GoogleMap object is ready to go.
            //}
            RefreshButton.SetCommand(
                "Click",
                Vm.GoToDownloadCitiesCommand);
          
             Button button = FindViewById<Button>(Resource.Id.GoToContractView);

        //button.Click += delegate
        //{
        //    button.Text = string.Format("{0} clicks!", count++);
        //};

    }

        public async void OnMapReady(GoogleMap googleMap)
        {
            _googleMap = googleMap;
            //Setup and customize your Google Map
            _googleMap.UiSettings.CompassEnabled = true;
            _googleMap.UiSettings.MyLocationButtonEnabled = true;
            _googleMap.UiSettings.MapToolbarEnabled = true;
        }
    }
}


