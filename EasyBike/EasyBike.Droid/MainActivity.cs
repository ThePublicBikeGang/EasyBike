using Android.App;
using Android.Widget;
using Android.OS;
using EasyBike.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using Android.Gms.Maps;
using Android.Support.V7.App;
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

namespace EasyBike.Droid
{
    // TODO 
    // Attribution Requirements
    // If you use the Google Maps Android API in your application, you must include the Google Play Services attribution text as part of a "Legal Notices" section in your application.Including legal notices as an independent menu item, or as part of an "About" menu item, is recommended.
    // The attribution text is available by making a call to GoogleApiAvailability.getOpenSourceSoftwareLicenseInfo.

    [Activity(Label = "EasyBike.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public partial class MainActivity : IOnMapReadyCallback, ClusterManager.IOnClusterClickListener, ClusterManager.IOnClusterItemClickListener
    {
        private Binding _lastLoadedBinding;

        private MapFragment _mapFragment;
        private GoogleMap _map;
        private ClusterManager _clusterManager;
        public CancellationTokenSource cts = new CancellationTokenSource();
        private TimeSpan throttleTime = TimeSpan.FromMilliseconds(150);
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

            //     MapFragment mapFrag = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            //GoogleMap _googleMap = mapFrag.Map;
            //if (_googleMap != null)
            //{
            //    // The GoogleMap object is ready to go.
            //}
            RefreshButton.SetCommand("Click", Vm.GoToDownloadCitiesCommand);
          
            Button button = FindViewById<Button>(Resource.Id.GoToContractView);
            var mClusterManager = new ClusterManager(this, _map);
            //button.Click += delegate
            //{
            //    button.Text = string.Format("{0} clicks!", count++);
            //};

        }


        protected override void OnResume()
        {
            base.OnResume();
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
                .InvokeZoomControlsEnabled(true)
                .InvokeCompassEnabled(true);

                FragmentTransaction fragTx = FragmentManager.BeginTransaction();
                _mapFragment = MapFragment.NewInstance(mapOptions);
                fragTx.Add(Resource.Id.map, _mapFragment, "map");
                fragTx.Commit();
                _mapFragment.GetMapAsync(this);
            }
        }

        private void AddClusterItems()
        {
            double lat = 63.430515;
            double lng = 10.395053;

            List<ClusterItem> items = new List<ClusterItem>();

            for (var i = 0; i < 10; i++)
            {
                double offset = i / 60d;
                lat = lat + offset;
                lng = lng + offset;

                var item = new ClusterItem(lat, lng);
                items.Add(item);
            }

            _clusterManager.AddItems(items);
            _clusterManager.Cluster();
        }
        //Cluster override methods
        public bool OnClusterClick(ICluster cluster)
        {
            Toast.MakeText(this, cluster.Items.Count + " items in cluster", ToastLength.Short).Show();
            return false;
        }

        public bool OnClusterItemClick(Java.Lang.Object marker)
        {
            Toast.MakeText(this, "Marker clicked", ToastLength.Short).Show();
            return false;
        }
        public void SetViewPoint(LatLng latlng, bool animated)
        {
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(latlng);
            builder.Zoom(14.5F);
            CameraPosition cameraPosition = builder.Build();

            if (animated)
            {
                _map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
            }
            else
            {
                _map.MoveCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
            }
        }



        public async void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;
            //Setup and customize your Google Map
            _map.UiSettings.CompassEnabled = true;
            _map.UiSettings.MyLocationButtonEnabled = true;
            _map.UiSettings.MapToolbarEnabled = true;


            SetViewPoint(new LatLng(63.430515, 10.395053), false);

            _clusterManager = new ClusterManager(this, _map);
            _clusterManager.SetOnClusterClickListener(this);
            _clusterManager.SetOnClusterItemClickListener(this);
            _map.SetOnCameraChangeListener(_clusterManager);
            _map.SetOnMarkerClickListener(_clusterManager);


            var mapObserver = Observable.FromEventPattern(_map, "CameraChange");
            mapObserver
                .Do((e) =>
                {
                    cts.Cancel();
                    cts = new CancellationTokenSource();
                })
                .Throttle(throttleTime)
                .Select(async x =>
                {
                    var stations = SimpleIoc.Default.GetInstance<IContractService>().GetStations();
                    // some services can provide wrong values in lat or lon... just take care of it
                    foreach (var station in stations.Where(s => s.Location == null))
                    {
                        station.Location = new LatLng(station.Latitude, station.Longitude);

                    }
                    return stations;
                })
                .Switch()
                .Subscribe(x =>
                {
                    if (x == null)
                        return;
                    RunOnUiThread(() =>
                    {
                        AddClusterItems();
                    });
                    
                });

            AddClusterItems();
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
