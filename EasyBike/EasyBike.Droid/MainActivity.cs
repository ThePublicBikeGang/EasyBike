using Android.App;
using Android.Widget;
using Android.Preferences;
using EasyBike.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using Android.Gms.Maps;
using System;
using System.Globalization;
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
using Android.Locations;
using EasyBike.Models.Storage;
using EasyBike.Models.Favorites;

// These shortcuts are used to prevent the use by default of these classes in Android.Views
using MenuItemCompat = Android.Support.V4.View.MenuItemCompat;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using ShareActionProvider = Android.Support.V7.Widget.ShareActionProvider;
using ActionMode = Android.Support.V7.View.ActionMode;
using System.Reactive.Subjects;
using Plugin.Geolocator;

namespace EasyBike.Droid
{
    // TODO
    // Attribution Requirements
    // If you use the Google Maps Android API in your application, you must include the Google Play Services attribution text as part of a "Legal Notices" section in your application.Including legal notices as an independent menu item, or as part of an "About" menu item, is recommended.
    // The attribution text is available by making a call to GoogleApiAvailability.getOpenSourceSoftwareLicenseInfo.

    // FOR MARSHMALLOW : check this for Location https://blog.xamarin.com/requesting-runtime-permissions-in-android-marshmallow/
    // https://developers.google.com/maps/documentation/android-api/location#runtime-permission

    //http://www.sitepoint.com/material-design-android-design-support-library/
    [Activity(MainLauncher = true)]
    public partial class MainActivity : IOnMapReadyCallback, ActionMode.ICallback, ClusterManager.IOnClusterClickListener, ClusterManager.IOnClusterItemClickListener
    {
        private Binding _lastLoadedBinding;

        private FragmentTransaction _fragTx;
        private MapFragment _mapFragment;
        public static GoogleMap _map { get; set; }
        private ClusterManager _clusterManager;
        public CancellationTokenSource cts = new CancellationTokenSource();
        private TimeSpan throttleTime = TimeSpan.FromMilliseconds(150);
        private StationRenderer _clusterRender;
        private Marker longClickMarker;

        DrawerLayout drawerLayout;
        NavigationView navigationView;

        // For the contextual action bar and the share button
        private ActionMode _actionMode;
        private ShareActionProvider _shareActionProvider;
        private Intent _shareIntent = new Intent(Intent.ActionSend);
        private LatLng currentMarkerPosition;

        // switch mode buttons
        FloatingActionButton _bikesButton;
        FloatingActionButton _parkingButton;

        FloatingActionButton _locationButton;

        //
        private ISettingsService _settingsService;
        private IFavoritesService _favoritesService;

        public static bool MapIsTouched;

        public MainViewModel MainViewModel
        {
            get
            {
                return App.Locator.Main;
            }
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
                        _context.MainViewModel.GoToDownloadCitiesCommand.Execute(null);
                        return true;
                    case Resource.Id.nav_about:
                        _context.MainViewModel.AboutCommand.Execute(null);
                        return true;
                    case Resource.Id.nav_favorites:
                        _context.MainViewModel.GoToFavoritsCommand.Execute(null);
                        return true;
                }
                return true;
            }
        }
        // helper to detect when the user is moving the map
        public class FrameOnGenericMotionListener : Java.Lang.Object, View.IOnGenericMotionListener
        {
            MainActivity _context;

            public FrameOnGenericMotionListener(MainActivity context)
            {
                _context = context;
            }

            public bool OnGenericMotion(View v, MotionEvent e)
            {
                throw new NotImplementedException();
            }

            public bool OnTouch(View v, MotionEvent e)
            {
                System.Diagnostics.Debug.WriteLine(e.Action);
                if (e.Action == MotionEventActions.Move)
                {
                    if (_context._stickToUserLocation)
                    {
                        _context.UnStickUserLocation();
                    }
                }
                return false;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            Log.Debug("MyActivity", "Begin OnCreate");
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            StartLocationTracking();

            //var uiOptions = (int)this.Window.DecorView.SystemUiVisibility;
            //var newUiOptions = (int)uiOptions;
            //newUiOptions &= ~(int)SystemUiFlags.LowProfile;
            //newUiOptions &= ~(int)SystemUiFlags.Fullscreen;
            //newUiOptions &= ~(int)SystemUiFlags.HideNavigation;
            //newUiOptions &= ~(int)SystemUiFlags.Immersive;
            //newUiOptions |= (int)SystemUiFlags.ImmersiveSticky;
            //this.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;
            //Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);



            //Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);
            ////Enable support action bar to display hamburger
            //SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            //SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _bikesButton = FindViewById<FloatingActionButton>(Resource.Id.bikesButton);
            _bikesButton.Click += BikesButton_Click;
            _parkingButton = FindViewById<FloatingActionButton>(Resource.Id.parkingButton);
            _parkingButton.Click += ParkingButton_Click;

         

            _locationButton = FindViewById<FloatingActionButton>(Resource.Id.locationButton);
            _locationButton.Click += LocationButton_Click;
            UnStickUserLocation();

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                //react to click here and swap fragments or navigate
                drawerLayout.CloseDrawers();
            };

            navigationView.SetNavigationItemSelectedListener(new NavigationItemSelectedListener(this));

            // trigger the creation of the injected dependencies
            var unused = MainViewModel.AboutCommand;
            _settingsService = SimpleIoc.Default.GetInstance<ISettingsService>();
            _favoritesService = SimpleIoc.Default.GetInstance<IFavoritesService>();
            // TODO pour le debug des favoris :
            _favoritesService.AddFavoriteAsync(new Favorite {
                Address = "Test address", Latitude = 0.0, Longitude = 0.0, Name = "Test name"
            });
        }

        protected async override void OnPause()
        {
            base.OnPause();
            Log.Debug("MyActivity", "Begin OnPause");
            await _settingsService.SaveSettingAsync();
        }

        private LatLng _lastUserLocation;
        public  bool _stickToUserLocation;
        private void StartLocationTracking()
        {
            var locator = CrossGeolocator.Current;
            // New in iOS 9 allowsBackgroundLocationUpdates must be set if you are running a background agent to track location. I have exposed this on the Geolocator via:
            locator.AllowsBackgroundUpdates = true;
            locator.DesiredAccuracy = 100; //100 is new default
            locator.PositionChanged += Locator_PositionChanged;
            locator.StartListeningAsync(5000, 5000, false);
        }
        public void UnStickUserLocation()
        {
            _stickToUserLocation = false;
            _locationButton.Background.SetAlpha(150);
        }

        private async void LocationButton_Click(object sender, EventArgs e)
        {
            _stickToUserLocation = true;
            _locationButton.Background.SetAlpha(255);
            if (_lastUserLocation == null)
            {
                try
                {
                    // Get a quick last known location
                    var locationManager = (LocationManager)GetSystemService("location");
                    // Getting the name of the best provider
                    var provider = locationManager.GetBestProvider(new Criteria(), true);
                    // Getting Current Location
                    var previousLocation = locationManager.GetLastKnownLocation(provider);
                    _lastUserLocation = new LatLng(previousLocation.Latitude, previousLocation.Longitude);
                    _map.AnimateCamera(CameraUpdateFactory.NewLatLng(new LatLng(_lastUserLocation.Latitude, _lastUserLocation.Longitude)));
                }
                catch { }
            }

            if (_lastUserLocation != null)
            {
                _map.AnimateCamera(CameraUpdateFactory.NewLatLng(new LatLng(_lastUserLocation.Latitude, _lastUserLocation.Longitude)));
            }
            else
            {
                var locator = CrossGeolocator.Current;
                try
                {
                    var location = await locator.GetPositionAsync(15000, null, false);
                    _map.AnimateCamera(CameraUpdateFactory.NewLatLng(new LatLng(location.Latitude, location.Longitude)));
                }
                catch { /*ignore*/ }
            }

        }

        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            _lastUserLocation = new LatLng(e.Position.Latitude, e.Position.Longitude);
            if (_stickToUserLocation)
            {
                _map.AnimateCamera(CameraUpdateFactory.NewLatLng(_lastUserLocation));
            }
        }

        public override void OnBackPressed()
        {
            if (drawerLayout.IsDrawerOpen(navigationView))
            {
                drawerLayout.CloseDrawer(navigationView);
            }
            else
            {
                base.OnBackPressed();
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
            Log.Debug("MyActivity", "Begin OnCreateOptionsMenu");
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Log.Debug("MyActivity", "Begin OnOptionsItemSelected");
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            Log.Debug("MyActivity", "Begin OnCreateActionMode");
            MenuInflater.Inflate(Resource.Menu.actionbar, menu);

            _shareActionProvider = (ShareActionProvider)MenuItemCompat.GetActionProvider(menu.FindItem(Resource.Id.menu_share));
            _shareIntent = _createShareIntent();
            _setShareIntent(_shareIntent);

            return true;
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            return false;
        }

        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            Log.Debug("MyActivity", "Begin OnActionItemClicked");
            switch (item.ItemId)
            {
                case Resource.Id.menu_share:
                    mode.Finish();
                    return true;
                case Resource.Id.menu_route:
                    if (currentMarkerPosition != null)
                    {
                        StartActivity(_createRouteIntent(currentMarkerPosition.Latitude, currentMarkerPosition.Longitude));
                    }
                    return true;
                case Resource.Id.menu_favorite:
                    AlertDialog dialog = null;
                    dialog = new AlertDialog.Builder(this)
                        .SetTitle(Resources.GetString(Resource.String.favoriteDialogTitle))
                        .SetView(this.LayoutInflater.Inflate(Resource.Layout.DialogAddFavorite, null))
                        .SetPositiveButton(Android.Resource.String.Ok, (sender, EventArgs) =>
                        {
                            var favoriteName = dialog.FindViewById<EditText>(Resource.Id.favoriteName).Text.ToString();
                            Log.Debug("MyActivity", "Add to favorite: " + favoriteName);
                            if (favoriteName.Trim() == "")
                            {
                                Toast.MakeText(this, Resources.GetString(Resource.String.favoriteEmptyName), ToastLength.Short).Show();
                            }
                            else
                            {
                                // TODO Ajout à réaliser
                                Toast.MakeText(this, Resources.GetString(Resource.String.favoriteAdded), ToastLength.Short).Show();
                            }
                        }).SetNegativeButton(Android.Resource.String.Cancel, (sender, EventArgs) => { })
                        .Create();
                    dialog.Show();
                    return true;
                default:
                    return false;
            }
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
            _actionMode = null;
            if (longClickMarker != null)
            {
                longClickMarker.Remove();
            }
        }

        private void _setShareIntent(Intent shareIntent)
        {
            if (_shareActionProvider != null)
            {
                Log.Debug("MyActivity", "_setShareIntent");
                _shareActionProvider.SetShareIntent(shareIntent);
            }
        }

        private Intent _createShareIntent()
        {
            Log.Debug("MyActivity", "Begin _createShareIntent");
            var shareIntent = new Intent(Intent.ActionSend);
            var text = "Unknown position";
            if (currentMarkerPosition != null)
            {
                text = currentMarkerPosition.ToString();
            }
            shareIntent.PutExtra(Intent.ExtraText, text);
            shareIntent.SetType("text/plain");
            return shareIntent;
        }

        private Intent _createRouteIntent(double latitude, double longitude)
        {
            var strLatitude = latitude.ToString("G", CultureInfo.CreateSpecificCulture("en-US"));
            var strLongitude = longitude.ToString("G", CultureInfo.CreateSpecificCulture("en-US"));
            // mode = b for bicycling
            Android.Net.Uri uri = Android.Net.Uri.Parse("google.navigation:mode=b&q=" + strLatitude + "," + strLongitude);
            return new Intent(Intent.ActionView, uri);
        }

        private void IncreaseButtonVisibility(FloatingActionButton button)
        {
            button.Background.SetAlpha(255);
        }
        private void DecreaseButtonVisibility(FloatingActionButton button)
        {
            button.Background.SetAlpha(100);
        }
        /// <summary>
        /// set the visual state of the bike/parking mode buttons 
        /// </summary>
        private void SwitchModeStationParkingVisualState()
        {
            if (_settingsService.Settings.IsBikeMode)
            {
                IncreaseButtonVisibility(_bikesButton);
                DecreaseButtonVisibility(_parkingButton);
            }
            else
            {
                IncreaseButtonVisibility(_parkingButton);
                DecreaseButtonVisibility(_bikesButton);
            }
            //_parkingButton.Elevation = 0;
            //_bikesButton.Elevation = 0;
        }

        /// <summary>
        /// Switch between Parking view and Bike view
        /// </summary>
        private void SwitchModeStationParking()
        {

            _settingsService.Settings.IsBikeMode = !_settingsService.Settings.IsBikeMode;

            foreach (var clusterItem in StationControls.ToList())
            {
                var station = clusterItem.Station;
                var control = (station.Control as Marker);
                if (station != null && control != null)
                {
                    RefreshStation(station, control);
                }
            }

            SwitchModeStationParkingVisualState();
        }

        private void ParkingButton_Click(object sender, EventArgs e)
        {
            SwitchModeStationParking();
        }

        private void BikesButton_Click(object sender, EventArgs e)
        {
            SwitchModeStationParking();
        }

        protected override void OnResume()
        {
            base.OnResume();
            Log.Debug("MyActivity", "Begin OnResume");
            SetupMapIfNeeded();
        }

        private bool _gettingMap;

        private async void SetupMapIfNeeded()
        {
            if (_map == null && !_gettingMap)
            {
                // TODO À quoi sert cette variable ? On peut supprimer je pense.
                // a priori SetupMapIfNeeded peut-être appelé pluiseurs fois d'affilé, c'est pour prévenir ça
                _gettingMap = true;
                GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeNormal)
                    //.InvokeZoomControlsEnabled(true)
                    //.InvokeMapToolbarEnabled(true)
                    .InvokeCompassEnabled(true)
                    .InvokeCamera(await GetStartingCameraPosition());

                _fragTx = FragmentManager.BeginTransaction();
                _mapFragment = MapFragmentExtended.NewInstance(mapOptions);

           

                _fragTx.Add(Resource.Id.map, _mapFragment, "map");
                _fragTx.Commit();

                _mapFragment.GetMapAsync(this);
            }

        }

        /// <Docs>Get the camera position when starting the app.</Docs>
        /// <returns>A CameraPosition with Latitude, Longitude and Zoom set.</returns>
        /// <summary>
        /// This is the menu for the Toolbar/Action Bar to use
        /// </summary>
        private async Task<CameraPosition> GetStartingCameraPosition()
        {
            //            double latitude = 48.879918;
            //            double longitude = 2.354810;
            //            float zoom = 14.5F;
            var settings = await _settingsService.GetSettingsAsync();
            //            if (settings.LastLocation != null) {
            //                latitude = 
            //            }
            return new CameraPosition.Builder()
                .Target(new LatLng(settings.LastLocation.Latitude, settings.LastLocation.Longitude))
                .Zoom((float)settings.LastLocation.ZoomLevel).Build();
        }

        //Cluster override methods
        public bool OnClusterClick(ICluster cluster)
        {
            Log.Debug("MyActivity", "Begin OnClusterClick");
            UnStickUserLocation();
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
            Log.Debug("MyActivity", "Begin OnClusterItemClick");
            UnStickUserLocation();
            if (marker is ClusterItem)
            {
                currentMarkerPosition = ((ClusterItem)marker).Position;
                _actionMode = StartSupportActionMode(this);
            }
            return false;
        }

        public void SetViewPoint(CameraPosition cameraPosition, bool animated)
        {
            // As explained here: https://stackoverflow.com/a/14167568
            // This is the way of intializing the map position
            // CameraPosition cameraPosition = new CameraPosition.Builder().Target(latlng).Zoom(zoom).Build();

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
            //var station = (sender as Station);
            //var control = (station.Control as Marker);
            //if (station != null && control != null)
            //{
            //    RefreshStation(station, control);
            //}
            //RunOnUiThread(() =>
            //{
            //    _clusterManager.Cluster();
            //});
        }

        private void OnContractRefreshed(object sender, EventArgs e)
        {
            RunOnUiThread(() =>
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
                //RunOnUiThread(() =>
                //{
                //    _clusterManager.Cluster();
                //});
            });
        }

        private void RefreshStation(Station station, Marker control)
        {
            try
            {
                station.IsUiRefreshNeeded = false;
                // this can raise a IllegalArgumentException: Released unknown imageData reference
                // as the marker may not be on the map anymore so better to check again for null ref
                control.SetIcon(_clusterRender.CreateStationIcon(station));

            }
            catch
            {
                Log.Debug("MyActivity", "Control VISIBILITY: " + control.Visible);
                Log.Debug("MyActivity", "Control POS: " + control.Position);
            }
        }

        public readonly List<Station> Items = new List<Station>();
        public readonly List<ClusterItem> StationControls = new List<ClusterItem>();
        private const int MAX_CONTROLS = 38;
        private double MAXDISTANCE = 100; // TODO useless?
        private IContractService _contractService;

        private Subject<AddressesFromLocationDTO> AddressesFromLocationStream = new Subject<AddressesFromLocationDTO>();

        public async void OnMapReady(GoogleMap googleMap)
        {
            _mapFragment.View.SetOnGenericMotionListener(new FrameOnGenericMotionListener(this));
            Log.Debug("MyActivity", "Begin OnMapReady");
            // TODO TO HELP DEBUG auto download paris to help dev on performances 
                        var contractToTest = "Paris";
                        var contractService = SimpleIoc.Default.GetInstance<IContractService>();
                        var contract = contractService.GetCountries().First(country => country.Contracts.Any(c => c.Name == contractToTest)).Contracts.First(c => c.Name == contractToTest);
                        await SimpleIoc.Default.GetInstance<ContractsViewModel>().AddOrRemoveContract(contract);
            _contractService = SimpleIoc.Default.GetInstance<IContractService>();
            _contractService.ContractRefreshed += OnContractRefreshed;
            _contractService.StationRefreshed += OnStationRefreshed;

            // set the initial visual state of the bike/parking buttons
            SwitchModeStationParkingVisualState();

            _map = googleMap;
            //Setup and customize your Google Map
            _map.UiSettings.CompassEnabled = true;
            _map.MyLocationEnabled = true;


            _map.UiSettings.MyLocationButtonEnabled = false;
            _map.UiSettings.MapToolbarEnabled = false;

            // add padding to prevent action bar to hide the position button
            //var dp = (int)(48 * Resources.DisplayMetrics.Xdpi / Resources.DisplayMetrics.Density);
            //_map.SetPadding(0, dp, 0, 0);
            //_map.vie
            //View locationButton = suppormanagerObj.getView().findViewById(2);
            //var rlp = (RelativeLayout.LayoutParams)locationButton.getLayoutParams(); 
            //// position on right bottom 
            //rlp.addRule(RelativeLayout.ALIGN_PARENT_TOP, 0); rlp.addRule(RelativeLayout.ALIGN_PARENT_BOTTOM, RelativeLayout.TRUE); 



            // Initialize the camera position
            SetViewPoint(await GetStartingCameraPosition(), false);

            // Initialize the marker with the stations
            _clusterManager = new ClusterManager(this, _map);
            _clusterRender = new StationRenderer(this, _map, _clusterManager);
            _clusterManager.SetRenderer(_clusterRender);
            _clusterManager.SetOnClusterClickListener(this);
            _clusterManager.SetOnClusterItemClickListener(this);
            _map.SetOnCameraChangeListener(_clusterManager);
            _map.SetOnMarkerClickListener(_clusterManager);

            // check if the app contains a least one city, otherwise, tells the user to download one
            MainViewModel.MainPageLoadedCommand.Execute(null);

            // On long click, display the address on a info window
            Observable.FromEventPattern<GoogleMap.MapLongClickEventArgs>(_map, "MapLongClick")
                .Select(e => Observable.FromAsync(token => Task.Run(async () =>
                {
                    currentMarkerPosition = e.EventArgs.Point;
                    var latLongString = $"(lat: { Math.Round(currentMarkerPosition.Latitude, 4)}, lon: { Math.Round(currentMarkerPosition.Longitude, 4)})";

                    RunOnUiThread(() =>
                    {
                        if (longClickMarker != null)
                        {
                            // Remove a previously created marker
                            longClickMarker.Remove();
                        }

                        var markerOptions = new MarkerOptions().SetPosition(currentMarkerPosition);
                        // Create and show the marker
                        longClickMarker = _map.AddMarker(markerOptions);
                        longClickMarker.Title = Resources.GetString(Resource.String.mapMarkerResolving);
                        longClickMarker.Snippet = latLongString;
                        longClickMarker.ShowInfoWindow();

                        _actionMode = _actionMode ?? StartSupportActionMode(this);

                    });

                    IList<Address> addresses = new List<Address>();
                    try
                    {
                        // Convert latitude and longitude to an address (GeoCoder)
                        addresses = await (new Geocoder(this).GetFromLocationAsync(currentMarkerPosition.Latitude, currentMarkerPosition.Longitude, 1));
                    }
                    catch (Exception ex)
                    {
                        Log.Debug("MyActivity", "Geocoder crashed: " + ex.Message);
                    }
                    return new AddressesFromLocationDTO { Addresses = addresses, Location = latLongString };

                }, token)))
                .Switch()
                .ObserveOn(SynchronizationContext.Current)
            .Subscribe(x =>
            {
                if (x.Addresses.Any())
                {
                    longClickMarker.Title = x.Addresses[0].GetAddressLine(0);
                    longClickMarker.Snippet = $"{x.Addresses[0].Locality} {x.Location}";
                }
                else
                {
                    longClickMarker.Title = Resources.GetString(Resource.String.mapMarkerImpossible);
                }
                longClickMarker.ShowInfoWindow();
            });

            // Initialize the behavior when long clicking somewhere on the map
            //_map.MapLongClick += async (sender, e) =>
            //{
            //    // On long click, display the address on a info window
            //    if (longClickMarker != null)
            //    {
            //        // Remove a previously created marker
            //        longClickMarker.Remove();
            //    }

            //    IList<Address> addresses = new List<Address>();
            //    currentMarkerPosition = e.Point;
            //    var latLongString = $"(latitude: { Math.Round(currentMarkerPosition.Latitude)}, longitude: { Math.Round(currentMarkerPosition.Longitude, 4)})";
            //    try
            //    {

            //        // Resolve the addresses (can throw an exception)
            //        var task = new Geocoder(this).GetFromLocationAsync(currentMarkerPosition.Latitude, currentMarkerPosition.Longitude, 1);
            //        AddressesFromLocationStream.OnNext(new AddressesFromLocationDTO { AddressesTask = task, Location = currentMarkerPosition });
            //    }
            //    catch (Exception)
            //    {
            //        // Ignore
            //    }
            //    finally
            //    {
            //        AddressesFromLocationStream.OnNext(new AddressesFromLocationDTO { Addresses = addresses, Location = currentMarkerPosition });
            //    }
            //};

            _map.MapClick += (sender, e) =>
            {
                if (longClickMarker != null)
                {
                    Log.Debug("MyActivity", "Remove long click marker");
                    longClickMarker.Remove();
                }
                if (_actionMode != null)
                {
                    Log.Debug("MyActivity", "Finish action mode");
                    _actionMode.Finish();
                }
            };

            _contractService = SimpleIoc.Default.GetInstance<IContractService>();
            var mapObserver = Observable.FromEventPattern(_map, "CameraChange");
            TaskCompletionSource<bool> tcs;
            mapObserver
                .Do((e) =>
                {
                    cts.Cancel();
                    cts = new CancellationTokenSource();
                }).Throttle(throttleTime)
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
                        catch
                        {
                        }
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

        private void RefreshView(AddRemoveCollection addRemoveCollection, CancellationToken token)
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
    }
}

public class AddressesFromLocationDTO
{
    public IList<Address> Addresses { get; set; }
    public string Location { get; set; }
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
