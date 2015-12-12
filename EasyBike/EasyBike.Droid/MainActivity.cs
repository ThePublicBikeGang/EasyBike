using Android.App;
using Android.Widget;
using Android.Preferences;
using EasyBike.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using Android.Gms.Maps;
using System;
using Com.Google.Maps.Android.Clustering;
using Android.Gms.Maps.Model;
using EasyBike.Droid.Models;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using GalaSoft.MvvmLight.Ioc;
using EasyBike.Models;
using System.Linq;
using EasyBike.Models.Stations;
using EasyBike.Droid.Helpers;
using System.Threading.Tasks;
using Com.Google.Maps.Android.Clustering.View;
using System.Diagnostics;
using Android.OS;
using Android;
using Java.Security;
using Android.Support.V4.Content;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Graphics;
using Android.Content;
using Android.Util;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace EasyBike.Droid
{
    // TODO 
    // Attribution Requirements
    // If you use the Google Maps Android API in your application, you must include the Google Play Services attribution text as part of a "Legal Notices" section in your application.Including legal notices as an independent menu item, or as part of an "About" menu item, is recommended.
    // The attribution text is available by making a call to GoogleApiAvailability.getOpenSourceSoftwareLicenseInfo.

    // FOR MARSHMALLOW : check this for Location https://blog.xamarin.com/requesting-runtime-permissions-in-android-marshmallow/
    // https://developers.google.com/maps/documentation/android-api/location#runtime-permission

    //http://www.sitepoint.com/material-design-android-design-support-library/
    [Activity(Label = "EasyBike.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public partial class MainActivity : IOnMapReadyCallback, ClusterManager.IOnClusterClickListener, ClusterManager.IOnClusterItemClickListener
    {
        private Binding _lastLoadedBinding;

        private MapFragment _mapFragment;
        private GoogleMap _map;
        private ClusterManager _clusterManager;
        public CancellationTokenSource cts = new CancellationTokenSource();
        private TimeSpan throttleTime = TimeSpan.FromMilliseconds(150);
        private StationRenderer _clusterRender;
        DrawerLayout drawerLayout;
        NavigationView navigationView;

		private ISharedPreferences preferences;

        public MainViewModel Vm
        {
            get
            {
                return App.Locator.Main;
            }
        }
        /// <Docs>The options menu in which you place your items.</Docs>
		/// <returns>To be added.</returns>
		/// <summary>
		/// This is the menu for the Toolbar/Action Bar to use
		/// </summary>
		/// <param name="menu">Menu.</param>
		public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.home, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }

        public class NavigationItemSelectedListener : Java.Lang.Object, NavigationView.IOnNavigationItemSelectedListener
        {
            MainActivity _context;
            public NavigationItemSelectedListener(MainActivity context)
            {
                _context = context;
            }
            public bool OnNavigationItemSelected(IMenuItem menuItem)
            {
                //Check to see which item was being clicked and perform appropriate action
                switch (menuItem.ItemId)
                {
                    //Replacing the main content with ContentFragment Which is our Inbox View;
                    case Resource.Id.nav_cities:
                        _context.Vm.GoToDownloadCitiesCommand.Execute(null);
                        return true;
                    case Resource.Id.nav_about:
                        _context.Vm.AboutCommand.Execute(null);
                        return true;

                }
                return true;
            }
        }
        protected override void OnCreate(Bundle bundle)
        {
			Log.Debug ("MyActivity", "Begin OnCreate");
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

			preferences = PreferenceManager.GetDefaultSharedPreferences (this);

            //var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);

            //Enable support action bar to display hamburger
            //SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            //SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                //react to click here and swap fragments or navigate
                drawerLayout.CloseDrawers();
            };

            navigationView.SetNavigationItemSelectedListener(new NavigationItemSelectedListener(this));

            //RefreshButton.SetCommand("Click", Vm.GoToDownloadCitiesCommand);


            var test = Vm.AboutCommand;

            //button.Click += delegate
            //{
            //    button.Text = string.Format("{0} clicks!", count++);
            //};
        }

		protected override void OnPause() 
		{
			base.OnPause ();
			Log.Debug ("MyActivity", "Begin OnPause");
			CameraPosition camPosition = _map.CameraPosition;
			ISharedPreferencesEditor editor = preferences.Edit();
			editor.PutFloat ("Latitude", (float) camPosition.Target.Latitude);
			editor.PutFloat ("Longitude", (float) camPosition.Target.Longitude);
			editor.PutFloat ("Zoom", camPosition.Zoom);
			editor.Apply ();
		}

        protected override void OnResume()
        {
            base.OnResume();
			Log.Debug ("MyActivity", "Begin OnResume");
            SetupMapIfNeeded();
        }

        private bool _gettingMap;
        private void SetupMapIfNeeded()
        {
            if (_map == null && !_gettingMap)
            {
                _gettingMap = true;
                GoogleMapOptions mapOptions = new GoogleMapOptions()
                .InvokeMapType(GoogleMap.MapTypeNormal)
                //.InvokeZoomControlsEnabled(true)
                .InvokeMapToolbarEnabled(true)
                .InvokeCompassEnabled(true);

                FragmentTransaction fragTx = FragmentManager.BeginTransaction();
                _mapFragment = MapFragment.NewInstance(mapOptions);
                fragTx.Add(Resource.Id.map, _mapFragment, "map");
                fragTx.Commit();
                _mapFragment.GetMapAsync(this);
            }
        }

        //Cluster override methods
        public bool OnClusterClick(ICluster cluster)
        {
            LatLngBounds.Builder builder = new LatLngBounds.Builder();
            foreach (ClusterItem item in cluster.Items)
            {
                builder.Include(item.Position);
            }
            var bounds = builder.Build();
            //Observable.Interval(TimeSpan.FromMilliseconds(300)).Subscribe(t =>
            //{
            //    _map.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(bounds, 100));
            //});
            Task.Run(async () =>
            {
                await Task.Delay(300);
                RunOnUiThread(() =>
                {
                    _map.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(bounds, 100));
                });
            });
            //new Handler().PostDelayed(() =>
            //{
            //    _map.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(bounds, 100));
            //}, 300);

            return false;
        }

        public bool OnClusterItemClick(Java.Lang.Object marker)
        {
            Toast.MakeText(this, "Marker clicked", ToastLength.Short).Show();
            return false;


        }
		public void SetViewPoint(LatLng latlng, float zoom, bool animated)
        {
			// As explained here: https://stackoverflow.com/a/14167568
			// This is the way of intializing the map
			CameraPosition cameraPosition = new CameraPosition.Builder().Target(latlng).Zoom(zoom).Build();

            if (animated)
            {
                _map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
            }
            else
            {
                _map.MoveCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
            }
        }



        private void OnStationRefreshed(object sender, EventArgs e)
        {
            var station = (sender as Station);
            var control = (station.Control as Marker);
            if (station != null && control != null)
            {
                RefreshStation(station, control);
            }
        }

        private void OnContractRefreshed(object sender, EventArgs e)
        {
            var contract = (sender as Contract);
            foreach (var clusterItem in StationControls.Where(c => c.Station.IsUiRefreshNeeded && c.Station.ContractStorageName == contract.StorageName).ToList())
            {
                var station = clusterItem.Station;
                var control = (station.Control as Marker);
                if (station != null && control != null)
                {
                    RefreshStation(station, control);
                }
            }
        }

        private void RefreshStation(Station station, Marker control)
        {
            RunOnUiThread(() =>
            {
                try
                {
                    station.IsUiRefreshNeeded = false;
                    // this can raise a IllegalArgumentException: Released unknown imageData reference
                    // as the marker may not be on the map anymore so better to check again for null ref
                    control.SetIcon(_clusterRender.CreateBikeIcon(station));
                }
                catch { }
            });
        }

        public readonly List<Station> Items = new List<Station>();
        public readonly List<ClusterItem> StationControls = new List<ClusterItem>();
        private const int MAX_CONTROLS = 38;
        private double MAXDISTANCE = 100;
        private IContractService _contractService;
        public async void OnMapReady(GoogleMap googleMap)
        {
			Log.Debug ("MyActivity", "Begin OnMapReady");
            // TODO TO HELP DEBUG auto download paris to help dev on performances 
//            var contractToTest = "Paris";
//            var contractService = SimpleIoc.Default.GetInstance<IContractService>();
//            var contract = contractService.GetCountries().First(country => country.Contracts.Any(c => c.Name == contractToTest)).Contracts.First(c => c.Name == contractToTest);
//            await SimpleIoc.Default.GetInstance<ContractsViewModel>().AddOrRemoveContract(contract);


            _contractService = SimpleIoc.Default.GetInstance<IContractService>();
            _contractService.ContractRefreshed += OnContractRefreshed;
            _contractService.StationRefreshed += OnStationRefreshed;


            _map = googleMap;
            //Setup and customize your Google Map
            _map.UiSettings.CompassEnabled = true;
            _map.MyLocationEnabled = true;
            _map.UiSettings.MyLocationButtonEnabled = true;
            _map.UiSettings.MapToolbarEnabled = true;
			Log.Debug ("MyActivity", preferences.GetFloat ("Zoom", -1.0F).ToString());
			SetViewPoint(new LatLng(preferences.GetFloat ("Latitude", 48.879918F), preferences.GetFloat ("Longitude", 2.354810F)), preferences.GetFloat ("Zoom", 14.5F), false);

            _clusterManager = new ClusterManager(this, _map);
            _clusterRender = new StationRenderer(this, _map, _clusterManager);
            _clusterManager.SetRenderer(_clusterRender);
            _clusterManager.SetOnClusterClickListener(this);
            _clusterManager.SetOnClusterItemClickListener(this);
            _map.SetOnCameraChangeListener(_clusterManager);
            _map.SetOnMarkerClickListener(_clusterManager);

            _contractService = SimpleIoc.Default.GetInstance<IContractService>();
            var mapObserver = Observable.FromEventPattern(_map, "CameraChange");
            TaskCompletionSource<bool> tcs;
            mapObserver
                .Do((e) =>
                {
                    cts.Cancel();
                    cts = new CancellationTokenSource();
                })
                .Throttle(throttleTime)
                .Select(async x =>
                {
                    var stations = _contractService.GetStations();
                    // some services can provide wrong values in lat or lon... just take care of it
                    foreach (var station in stations.Where(s => s.Location == null))
                    {
                        station.Location = new LatLng(station.Latitude, station.Longitude);
                    }
                    LatLngBounds bounds = null;
                    tcs = new TaskCompletionSource<bool>();
                    RunOnUiThread(() =>
                   {
                       try
                       {
                           // can return null
                           bounds = _map.Projection.VisibleRegion.LatLngBounds;
                       }
                       catch { }
                       tcs.SetResult(true);
                   });

                    await tcs.Task;


                    var collection = new AddRemoveCollection();
                    if (bounds != null)
                    {
                        // extends slightly the bound view
                        // to provide a better experience
                        //bounds = MapHelper.extendLimits(bounds, 1);
                        collection.ToRemove = Items.Where(t => !bounds.Contains((LatLng)t.Location)).ToList();
                        collection.ToAdd = stations.Where(t => !Items.Contains(t)
                            && bounds.Contains((LatLng)t.Location)).Take(MAX_CONTROLS).ToList();
                        if (Items.Count > MAX_CONTROLS + collection.ToRemove.Count)
                            collection.ToAdd.Clear();

                    }
                    // precalculate the items offset (that deffer well calculation)
                    //foreach (var velib in collection.ToAdd)
                    //{
                    //    velib.GetOffsetLocation2(leftCornerLocation, zoomLevel);
                    //}
                    return collection;
                })
                .Switch()
                .Subscribe(x =>
                {
                    if (x == null)
                        return;


                    RunOnUiThread(() =>
                    {
                        RefreshView(x, cts.Token);
                    });

                });
        }

        private async void RefreshView(AddRemoveCollection addRemoveCollection, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            // remove out of view items
            foreach (var station in Items.Where(t => addRemoveCollection.ToRemove.Contains(t)).ToList())
            {

                var item = StationControls.First(c => c.Station.Latitude == station.Latitude && c.Station.Longitude == station.Longitude);
                _clusterManager.RemoveItem(item);
                StationControls.Remove(item);
                Items.Remove(station);
                station.Control = null;
                if (station.IsInRefreshPool)
                {
                    _contractService.RemoveStationFromRefreshingPool(station);
                }
                if (token.IsCancellationRequested)
                {
                    return;
                }
            }


            foreach (var station in addRemoveCollection.ToAdd.Where(t => !Items.Contains(t)).ToList())
            {
                var item = new ClusterItem(station.Latitude, station.Longitude)
                {
                    Station = station
                };
                StationControls.Add(item);
                _clusterManager.AddItem(item);
                Items.Add(station);
                if (token.IsCancellationRequested)
                {
                    return;
                }
            }
            _clusterManager.Cluster();
        }


        private void _map_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
        }

    }
}


//private void addColorsToMarkers()
//{
//    // Iterate over all the features stored in the layer
//    for (GeoJsonFeature feature : mLayer.getFeatures())
//    {
//        // Check if the magnitude property exists
//        if (feature.hasProperty("mag") && feature.hasProperty("place"))
//        {
//            double magnitude = Double.parseDouble(feature.getProperty("mag"));

//            // Get the icon for the feature
//            BitmapDescriptor pointIcon = BitmapDescriptorFactory
//                    .defaultMarker(magnitudeToColor(magnitude));

//            // Create a new point style
//            GeoJsonPointStyle pointStyle = new GeoJsonPointStyle();

//            // Set options for the point style
//            pointStyle.setIcon(pointIcon);
//            pointStyle.setTitle("Magnitude of " + magnitude);
//            pointStyle.setSnippet("Earthquake occured " + feature.getProperty("place"));

//            // Assign the point style to the feature
//            feature.setPointStyle(pointStyle);
//        }
//    }
//}
