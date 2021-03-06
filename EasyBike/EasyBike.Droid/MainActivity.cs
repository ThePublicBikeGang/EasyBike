﻿using Android.App;
using Android.Widget;
using EasyBike.ViewModels;
using Android.Gms.Maps;
using System;
using System.Net.Http;
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
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Graphics;
using Android.Content;
using Android.Locations;
using EasyBike.Models.Storage;
using EasyBike.Models.Favorites;

// These shortcuts are used to prevent the use by default of these classes in Android.Views
using Toolbar = Android.Support.V7.Widget.Toolbar;
using ActionMode = Android.Support.V7.View.ActionMode;
using System.Reactive.Subjects;
using Plugin.Geolocator;
using Newtonsoft.Json;
using ModernHttpClient;
using Android.Views.InputMethods;
using Com.Google.Maps.Android.UI;
using Android.Support.V4.Content.Res;
using Plugin.Compass;
using Plugin.Compass.Abstractions;
using EasyBike.Droid.Models.Direction;
using Android.Content.Res;
using Android.Support.V4.View;
using Com.Readystatesoftware.Systembartint;
using Android.Runtime;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight.Views;
using Plugin.Geolocator.Abstractions;
using EasyBike.Resources;
using Android.Views.Animations;
using Android.Graphics.Drawables;
using Android.Animation;

namespace EasyBike.Droid
{
    // TODO
    // Attribution Requirements
    // If you use the Google Maps Android API in your application, you must include the Google Play Services attribution text as part of a "Legal Notices" section in your application.Including legal notices as an independent menu item, or as part of an "About" menu item, is recommended.
    // The attribution text is available by making a call to GoogleApiAvailability.getOpenSourceSoftwareLicenseInfo.

    // FOR MARSHMALLOW : check this for Location https://blog.xamarin.com/requesting-runtime-permissions-in-android-marshmallow/
    // https://developers.google.com/maps/documentation/android-api/location#runtime-permission

    //http://www.sitepoint.com/material-design-android-design-support-library/
    [Activity(Label = "EasyBike", MainLauncher = true,
        LaunchMode = Android.Content.PM.LaunchMode.SingleTask,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "http", DataHost = "easybikeapp.com")]

    public partial class MainActivity : IOnMapReadyCallback, ActionMode.ICallback, ClusterManager.IOnClusterClickListener, ClusterManager.IOnClusterItemClickListener
    {
        private FragmentTransaction _fragTx;
        private MapFragment _mapFragment;
        public static GoogleMap _map { get; set; }
        private ClusterManager _clusterManager;
        public CancellationTokenSource cts = new CancellationTokenSource();
        private TimeSpan throttleTime = TimeSpan.FromMilliseconds(150);
        private StationRenderer _clusterRender;
        private Marker longClickMarker;

        // action bars and nav
        //private CustomActionBarDrawerToggle _drawerToggle;
        DrawerLayout _drawerLayout;
        NavigationView navigationView;

        // For the contextual action bar and the share button
        public ActionMode ActionMode;
        private Intent _shareIntent = new Intent(Intent.ActionSend);
        private LatLng currentMarkerPosition;
        private Marker _selectedMarker;

        // switch mode buttons
        private FloatingActionButton _bikesButton;
        private FloatingActionButton _parkingButton;
        private FloatingActionButton _locationButton;
        private FloatingActionButton _tileButton;

        private ProgressBar _searchProgressBar;

        private TextView _currentTileName;
        private Animation _currentTileNameAnimation;
        private Animation _disappearTileNameAnimation;

        // Place API
        const string strGoogleApiKey = "AIzaSyCwF6w1Zp2nBhXGq277dKOOQqAPBKH1QuM";
        const string strAutoCompleteGoogleApi = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=";
        const string strGeoCodingUrl = "https://maps.googleapis.com/maps/api/geocode/json";
        const string strPlaceApiDetailsUrl = "https://maps.googleapis.com/maps/api/place/details/json?placeid={0}&key={1}";

        AutoCompleteTextView AutoCompleteSearchPlaceTextView;
        GooglePlacesAutocompleteAdapter googlePlacesAutocompleteAdapter;
        //GoogleMapPlaceClass objMapClass;
        //GeoCodeJSONClass objGeoCodeJSONClass;

        private IGeolocator _locator;

        // Directions
        private Android.Gms.Maps.Model.Polyline _currentPolyline;

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
                _context.CloseDrawer();

                // provide a smoother animation of the drawer when closing
                Task.Run(async () =>
                {
                    await Task.Delay(200);

                    //Check to see which item was being clicked and perform appropriate action
                    switch (menuItem.ItemId)
                    {
                        //Replacing the main content with ContentFragment Which is our Inbox View;
                        case Resource.Id.nav_cities:
                            _context.MainViewModel.GoToDownloadCitiesCommand.Execute(null);
                            break;
                        case Resource.Id.nav_about:
                            _context.MainViewModel.AboutCommand.Execute(null);
                            break;
                        case Resource.Id.nav_tutorial:
                            _context.MainViewModel.HowToUseThisAppCommand.Execute(null);
                            break;
                        case Resource.Id.nav_favorites:
                            Intent i = new Intent(_context, typeof(FavoritesActivity));
                            _context.StartActivityForResult(i, 1);
                            //_context.MainViewModel.GoToFavoritsCommand.Execute(null);
                            break;
                        case Resource.Id.nav_share:
                            var shareIntent = new Intent(Intent.ActionSend);
                            shareIntent.PutExtra(Intent.ExtraText, StaticResources.FormatShareMessage());
                            shareIntent.SetType("text/plain");
                            _context.StartActivity(Intent.CreateChooser(shareIntent, _context.Resources.GetString(Resource.String.share)));
                            //mode.Finish();
                            break;
                    }
                });
                return true;
            }
        }

        /// <summary>
        /// when return to the activity from another with parameters
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            switch (requestCode)
            {
                // return from favorites
                case 1:
                    {
                        if (data != null && data.GetStringExtra("favorite") != null)
                        {
                            try
                            {
                                var favorite = JsonConvert.DeserializeObject<Favorite>(data.GetStringExtra("favorite"));
                                var position = new LatLng(favorite.Latitude, favorite.Longitude);
                                UnStickUserLocation();
                                if (!favorite.FromStation)
                                {
                                    AddPlaceMarker(position, favorite.Name, favorite.Address);
                                }

                                _map.AnimateCamera(CameraUpdateFactory.NewLatLng(position));
                                SelectItem(position);
                            }
                            catch
                            {
                                // ignore
                            }
                        }
                    }
                    break;
            }
        }

        public class HomeButtonClickListener : Java.Lang.Object, View.IOnClickListener
        {
            private MainActivity _context;
            public HomeButtonClickListener(MainActivity context)
            {
                _context = context;
            }
            public void OnClick(View v)
            {
                if (_context.ActionMode != null)
                {
                    _context.ActionMode.Finish();
                }
                _context._drawerLayout.OpenDrawer(GravityCompat.Start);


            }
        }

        private class CustomDrawerToggle : Java.Lang.Object, DrawerLayout.IDrawerListener
        {
            private MainActivity _context;
            public CustomDrawerToggle(MainActivity context)
            {
                _context = context;
            }
            public void OnDrawerClosed(View drawerView)
            {
            }

            public void OnDrawerOpened(View drawerView)
            {
            }

            public void OnDrawerSlide(View drawerView, float slideOffset)
            {
                if (_context.ActionMode != null)
                {
                    _context.ActionMode.Finish();
                }
            }

            public void OnDrawerStateChanged(int newState)
            {
            }
        }


        private void ParseIntent(Intent intent = null)
        {
            var intentData = intent == null ? Intent.Data : intent.Data;
            if (intentData != null)
            {
                // Read data from incoming intents
                var intentDataString = intentData.ToString();
                try
                {

                    string pattern = @"(?<=lt=)-?[0-9]\d*\.*\,*\d+";
                    if (Regex.IsMatch(intentDataString, pattern))
                    {
                        var regex = new Regex(pattern).Match(intentDataString);
                        if (regex != null && regex.Captures.Count > 0)
                        {
                            _parameterLat = double.Parse(regex.Captures[0].Value.Replace(',', '.'), CultureInfo.InvariantCulture);
                        }
                    }
                    pattern = @"(?<=ln=)-?[0-9]\d*\.*\,*\d+";
                    if (Regex.IsMatch(intentDataString, pattern))
                    {
                        var regex = new Regex(pattern).Match(intentDataString);
                        if (regex != null && regex.Captures.Count > 0)
                        {
                            _parameterLon = double.Parse(regex.Captures[0].Value.Replace(',', '.'), CultureInfo.InvariantCulture);
                        }
                    }
                    //pattern = @"(?<=text=)-?.*";
                    //if (Regex.IsMatch(intentDataString, pattern))
                    //{
                    //    var regex = new Regex(pattern).Match(intentDataString);
                    //    if (regex != null && regex.Captures.Count > 0)
                    //    {
                    //        _parameterText = (regex.Captures[0].Value).Replace("%20", " ");
                    //    }
                    //}
                }
                catch (Exception e)
                {
                    SimpleIoc.Default.GetInstance<IDialogService>().ShowMessage("Unable to find the passed location :(", "");
                }
            }


        }

        protected override void OnNewIntent(Intent intent)
        {
            ParseIntent(intent);
            // show a location if there is values in the intent
            if (_parameterLat != 0 && _parameterLon != 0)
            {
                ShowIntentLocation();
            }
        }

        private Bitmap _iconUserLocation;
        private Bitmap _iconCompass;
        // Parameters passed as Intent argument to the application
        private double _parameterLat;
        private double _parameterLon;
        //private string _parameterText;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _locator = CrossGeolocator.Current;
            // New in iOS 9 allowsBackgroundLocationUpdates must be set if you are running a background agent to track location. I have exposed this on the Geolocator via:
            // disable because it drill down the battery very quickly
            _locator.AllowsBackgroundUpdates = false;
            _locator.DesiredAccuracy = 50;
            _locator.PositionChanged -= Locator_PositionChanged;
            _locator.PositionChanged += Locator_PositionChanged;

            // parse params to show any shared location if it exists
            ParseIntent();

            var tintManager = new SystemBarTintManager(this);
            // set the transparent color of the status bar, 30% darker
            tintManager.SetTintColor(Color.ParseColor("#30000000"));
            tintManager.SetNavigationBarTintEnabled(true);
            tintManager.StatusBarTintEnabled = true;

            // prevent the soft keyboard from pushing the view up
            Window.SetSoftInputMode(SoftInput.AdjustNothing);

            // prepare icons for location / compass button
            var iconGenerator = new IconGenerator(this);
            iconGenerator.SetBackground(ResourcesCompat.GetDrawable(Resources, Resource.Drawable.ic_location, null));
            _iconUserLocation = iconGenerator.MakeIcon();
            iconGenerator.SetBackground(ResourcesCompat.GetDrawable(Resources, Resource.Drawable.ic_compass, null));
            _iconCompass = iconGenerator.MakeIcon();
            //var uiOptions = (int)this.Window.DecorView.SystemUiVisibility;
            //var newUiOptions = (int)uiOptions;
            //newUiOptions &= ~(int)SystemUiFlags.LowProfile;
            //newUiOptions &= ~(int)SystemUiFlags.Fullscreen;
            //newUiOptions &= ~(int)SystemUiFlags.HideNavigation;
            //newUiOptions &= ~(int)SystemUiFlags.Immersive;
            //newUiOptions |= (int)SystemUiFlags.ImmersiveSticky;
            //this.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;
            //Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            //toolbar.Background.SetAlpha(200);
            ViewCompat.SetElevation(toolbar, 6f);
            SetSupportActionBar(toolbar);

            //Enable support action bar to display hamburger and back arrow
            // http://stackoverflow.com/questions/28071763/toolbar-navigation-hamburger-icon-missing
            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            //_drawerToggle = new CustomActionBarDrawerToggle(this, _drawerLayout, toolbar, Resource.String.ApplicationName, Resource.String.ApplicationName);
            //_drawerToggle.DrawerIndicatorEnabled = true;
            _drawerLayout.SetDrawerListener(new CustomDrawerToggle(this));
            //Enable support action bar to display hamburger


            var burgerImage = FindViewById<ImageButton>(Resource.Id.burgerImage);
            burgerImage.SetOnClickListener(new HomeButtonClickListener(this));

            // SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            SupportActionBar.SetHomeButtonEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.SetDisplayShowCustomEnabled(false);

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(new NavigationItemSelectedListener(this));

            _bikesButton = FindViewById<FloatingActionButton>(Resource.Id.bikesButton);
            _bikesButton.BackgroundTintList = ColorStateList.ValueOf(Resources.GetColor(Resource.Color.primary_light));
            _bikesButton.Click += BikesButton_Click;

            _parkingButton = FindViewById<FloatingActionButton>(Resource.Id.parkingButton);
            _parkingButton.BackgroundTintList = ColorStateList.ValueOf(Resources.GetColor(Resource.Color.primary_light));
            _parkingButton.Click += ParkingButton_Click;

            _locationButton = FindViewById<FloatingActionButton>(Resource.Id.locationButton);
            _locationButton.BackgroundTintList = ColorStateList.ValueOf(Color.White);
            _locationButton.SetColorFilter(Color.Gray);

            _searchProgressBar = FindViewById<ProgressBar>(Resource.Id.searchProgressBar);

            // Doesn't work on Kitkat 4.4, use SetColorFilter instead
            //_locationButton.ImageTintList = ColorStateList.ValueOf(Color.Black);
            _locationButton.Click += LocationButton_Click;

            _tileButton = FindViewById<FloatingActionButton>(Resource.Id.tileButton);
            _tileButton.BackgroundTintList = ColorStateList.ValueOf(Color.White);
            _tileButton.SetColorFilter(Color.DarkGray);
            _tileButton.Click += TileButton_Click;
            var parent = (View)_tileButton.Parent;

            // Gets the parent view and posts a Runnable on the UI thread. 
            // This ensures that the parent lays out its children before calling the getHitRect() method.
            // The getHitRect() method gets the child's hit rectangle (touchable area) in the parent's coordinates.
            parent.Post(() =>
            {
                var touchRect = new Rect();
                _tileButton.GetHitRect(touchRect);
                touchRect.Top -= 200;
                touchRect.Left -= 200;
                touchRect.Bottom += 200;
                touchRect.Right += 200;


                parent.TouchDelegate = new TouchDelegate(touchRect, _tileButton);
            });

            _currentTileName = FindViewById<TextView>(Resource.Id.currentTileName);
            _currentTileNameAnimation = AnimationUtils.LoadAnimation(this, Resource.Animation.placeholder);
            _disappearTileNameAnimation = AnimationUtils.LoadAnimation(this, Resource.Animation.disappearAnimation);
            _currentTileNameAnimation.AnimationEnd += _currentTileNameAnimation_AnimationEnd;

            UnStickUserLocation();

            AutoCompleteSearchPlaceTextView = FindViewById<AutoCompleteTextView>(Resource.Id.autoCompleteSearchPlaceTextView);
            AutoCompleteSearchPlaceTextView.ItemClick += AutoCompleteSearchPlaceTextView_ItemClick;
            googlePlacesAutocompleteAdapter = new GooglePlacesAutocompleteAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line);
            AutoCompleteSearchPlaceTextView.Adapter = googlePlacesAutocompleteAdapter;


            Observable.FromEventPattern(AutoCompleteSearchPlaceTextView, "TextChanged")
                .Throttle(TimeSpan.FromMilliseconds(300))
                .Where(x => AutoCompleteSearchPlaceTextView.Text.Length >= 2)
                .Subscribe(async x =>
                {
                    try
                    {
                        RunOnUiThread(() =>
                        {
                            _searchProgressBar.Visibility = ViewStates.Visible;
                        });

                        using (var client = new HttpClient(new NativeMessageHandler()))
                        {
                            var response = await client.GetAsync(strAutoCompleteGoogleApi + AutoCompleteSearchPlaceTextView.Text + "&key=" + strGoogleApiKey).ConfigureAwait(false);
                            var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var predictions = JsonConvert.DeserializeObject<PlaceApiModel>(responseBodyAsText).predictions.ToList();
                            googlePlacesAutocompleteAdapter.Results = predictions;
                            if (AutoCompleteSearchPlaceTextView.Text.Length >= 2)
                            {
                                RunOnUiThread(() =>
                                {
                                    googlePlacesAutocompleteAdapter.NotifyDataSetChanged();
                                    EndPlacesSearch();
                                });
                            }
                        }
                    }
                    catch
                    {
                        RunOnUiThread(() =>
                        {
                            EndPlacesSearch();
                        });
                    }
                });


            //navigationView.NavigationItemSelected += (sender, e) =>
            //{
            //    e.MenuItem.SetChecked(true);
            //    //react to click here and swap fragments or navigate
            //    drawerLayout.CloseDrawers();
            //};

            // trigger the creation of the injected dependencies
            _settingsService = SimpleIoc.Default.GetInstance<ISettingsService>();
            _favoritesService = SimpleIoc.Default.GetInstance<IFavoritesService>();
        }

        public void EndPlacesSearch()
        {
            _searchProgressBar.Visibility = ViewStates.Invisible;
        }

        private string _currentMapOverlay = StaticResources.TilesNormalName;
        private void TileButton_Click(object sender, EventArgs e)
        {
            UpdateOverlay();
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            //_drawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            // _drawerToggle.OnConfigurationChanged(newConfig);
        }

        private async void AutoCompleteSearchPlaceTextView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            CloseKeyboard();
            ResetMapCameraViewAndStickers();
            var prediction = googlePlacesAutocompleteAdapter.Results[e.Position];
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                var response = await client.GetAsync(string.Format(strPlaceApiDetailsUrl, prediction.place_id, strGoogleApiKey)).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var place = JsonConvert.DeserializeObject<PlaceApiDetailModel>(responseBodyAsText);
                RunOnUiThread(() =>
                {
                    var position = new LatLng(place.result.geometry.location.lat, place.result.geometry.location.lng);
                    AddPlaceMarker(position, place.result.formatted_address, FormatLatLng(position));
                    SelectItem(position);
                    _map.AnimateCamera(CameraUpdateFactory.NewLatLng(position));
                });
            }
        }

        protected override void OnPause()
        {
            _locator.StopListeningAsync();
            _settingsService.MapTile = _selectedTileName;
            _settingsService.SaveSettingAsync();
            base.OnPause();
        }

        private LatLng _lastUserLocation;
        public bool _stickToUserLocation;

        private async void StartLocationTracking()
        {

            /*var listeningLocationTracking = */
            await _locator.StartListeningAsync(3000, 5, false);
            //if (!listeningLocationTracking)
            //{
            //    Dialog.ShowError(new Exception(""), "Locationtracking not enable :/", "ok", new Action(() => { }));
            //}
        }

        public void UnStickUserLocation()
        {
            if (_stickToUserLocation)
            {
                _stickToUserLocation = _compassMode = false;
                //_locationButton.Background.SetAlpha(150);
                _locationButton.BackgroundTintList = ColorStateList.ValueOf(Color.White);
                _locationButton.SetColorFilter(Color.Gray);
                //_locationButton.ImageTintList = ColorStateList.ValueOf(Color.Black);
                _locationButton.SetImageBitmap(_iconUserLocation);
                CrossCompass.Current.Stop();
            }
            CloseKeyboard();
            if (ActionMode != null)
            {
                ActionMode.Finish();
            }
        }

        private void CloseKeyboard()
        {
            InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(Window.DecorView.RootView.WindowToken, 0);
            if (AutoCompleteSearchPlaceTextView != null)
            {
                AutoCompleteSearchPlaceTextView.ClearFocus();
            }
        }

        public void DisableCompass()
        {
            _map.StopAnimation();
            _map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(new CameraPosition(_map.CameraPosition.Target, 17, 0, 0)), 300, null);
            _prevHeading = 0;
            _compassMode = false;
            _locationButton.SetImageBitmap(_iconUserLocation);
        }

        private void ResetMapCameraViewAndStickers()
        {
            UnStickUserLocation();
            DisableCompass();
        }

        private bool CheckIfLocationIsActive()
        {
            var locator = CrossGeolocator.Current;
            if (locator.IsGeolocationAvailable && !locator.IsGeolocationEnabled)
            {
                AlertDialog dialog = null;
                dialog = new AlertDialog.Builder(this)
                    .SetTitle(Resources.GetString(Resource.String.activateLocationDialogTitle))
                    .SetMessage(Resources.GetString(Resource.String.activateLocationDialogMessage))
                    .SetPositiveButton(Android.Resource.String.Ok, (sender, EventArgs) =>
                    {
                        var myIntent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                        StartActivity(myIntent);
                    }).SetNegativeButton(Android.Resource.String.Cancel, (sender, EventArgs) => { })
                    .Create();
                dialog.Show();
                return false;
            }
            return true;
        }

        IObservable<System.Reactive.EventPattern<CompassChangedEventArgs>> CompassChangedStream;
        private bool _compassMode;
        // used to reduce the noise provided buy the compass
        private double _prevHeading = 0d;
        // used to skip some events
        private int _compassEventcounter = 0;
        private async void LocationButton_Click(object sender, EventArgs e)
        {
            if (!CheckIfLocationIsActive())
            {
                return;
            }

            if (_compassMode && _stickToUserLocation)
            {
                ResetMapCameraViewAndStickers();
                return;
            }

            _locationButton.BackgroundTintList = ColorStateList.ValueOf(Resources.GetColor(Resource.Color.primary_light));
            //_locationButton.ImageTintList = ColorStateList.ValueOf(Color.White);
            _locationButton.SetColorFilter(Color.White);
            if (_stickToUserLocation)
            {
                _compassEventcounter = 0;
                _locationButton.SetImageBitmap(_iconCompass);
                if (CompassChangedStream == null)
                {
                    CompassChangedStream = Observable.FromEventPattern<CompassChangedEventArgs>(CrossCompass.Current, "CompassChanged");
                }

                // first event, mimic google map app behavior
                CompassChangedStream.Take(1).Subscribe(async compassChangedEventArgs =>
                {
                    RunOnUiThread(() =>
                    {
                        _map.StopAnimation();
                        _map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(new CameraPosition(
                        _lastUserLocation == null ? _map.CameraPosition.Target : _lastUserLocation,
                        18.5f,
                        60, (float)compassChangedEventArgs.EventArgs.Heading)), 400, null);
                    });
                    _prevHeading = compassChangedEventArgs.EventArgs.Heading;
                    await Task.Delay(400);
                    // compass mode
                    _compassMode = true;

                });

                // Throttle doesn't work ?!!
                // issue open: https://github.com/JarleySoft/Xamarin.Plugins/issues/7
                CompassChangedStream.Where(c => _compassMode).Subscribe(compassChangedEventArgs =>
                {
                    // Skip some events
                    if (_compassEventcounter % 5 == 0)
                    {
                        if (Math.Abs(_prevHeading - compassChangedEventArgs.EventArgs.Heading) > 2)
                        {
                            RunOnUiThread(() =>
                            {
                                _map.StopAnimation();
                                _map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(new CameraPosition(
                                _lastUserLocation == null ? _map.CameraPosition.Target : _lastUserLocation,
                                _map.CameraPosition.Zoom,
                                _map.CameraPosition.Tilt, (float)compassChangedEventArgs.EventArgs.Heading)), 300, null);
                            });
                            _prevHeading = compassChangedEventArgs.EventArgs.Heading;
                        }
                    }
                    _compassEventcounter++;
                });
                CrossCompass.Current.Start();
            }
            else
            {
                _stickToUserLocation = true;
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
            RunOnUiThread(() =>
            {
                AddDirectionsAsync();
            });

            if (_stickToUserLocation)
            {
                RunOnUiThread(() =>
                {
                    _map.MoveCamera(CameraUpdateFactory.NewLatLng(_lastUserLocation));
                });
            }
        }

        public override void OnBackPressed()
        {
            if (_drawerLayout.IsDrawerOpen(navigationView))
            {
                CloseDrawer();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public void CloseDrawer()
        {
            _drawerLayout.CloseDrawers();
        }

        /// <Docs>The options menu in which you place your items.</Docs>
        /// <returns>To be added.</returns>
        /// <summary>
        /// This is the menu for the Toolbar/Action Bar to use
        /// </summary>
        /// <param name="menu">Menu.</param>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnCreateOptionsMenu(menu);
        }

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    Log.Debug("MyActivity", "Begin OnOptionsItemSelected");
        //    switch (item.ItemId)
        //    {
        //        case Android.Resource.Id.Home:
        //            CloseKeyboard();
        //            _drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
        //            return true;


        //    }
        //    return base.OnOptionsItemSelected(item);
        //}

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar, menu);
            return true;
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            return false;
        }

        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_share:
                    _shareIntent = _createShareIntent();
                    StartActivity(Intent.CreateChooser(_shareIntent, Resources.GetString(Resource.String.share)));
                    //mode.Finish();
                    return true;
                case Resource.Id.menu_route:
                    if (currentMarkerPosition != null)
                    {
                        try
                        {
                            StartActivity(_createRouteIntent(currentMarkerPosition.Latitude, currentMarkerPosition.Longitude));
                        }
                        catch { }
                    }
                    return true;
                case Resource.Id.menu_favorite:
                    AlertDialog dialog = null;
                    dialog = new AlertDialog.Builder(this)
                        .SetTitle(Resources.GetString(Resource.String.favoriteDialogTitle))
                        .SetView(LayoutInflater.Inflate(Resource.Layout.DialogAddFavorite, null))
                        .SetPositiveButton(Android.Resource.String.Ok, (sender, EventArgs) =>
                        {
                            var favoriteName = dialog.FindViewById<EditText>(Resource.Id.favoriteName).Text.ToString();
                            if (favoriteName.Trim() == "")
                            {
                                Toast.MakeText(this, Resources.GetString(Resource.String.favoriteEmptyName), ToastLength.Short).Show();
                            }
                            else
                            {
                                // add custom location as favorite
                                if (longClickMarker != null && longClickMarker.Position.Latitude == currentMarkerPosition.Latitude && longClickMarker.Position.Longitude == currentMarkerPosition.Longitude)
                                {
                                    _favoritesService.AddFavoriteAsync(new Favorite()
                                    {
                                        Latitude = currentMarkerPosition.Latitude,
                                        Longitude = currentMarkerPosition.Longitude,
                                        Name = favoriteName,
                                        FromStation = false,
                                        Address = _lastResolvedAddress + (!string.IsNullOrWhiteSpace(_lastResolvedLocality) ? ", " + _lastResolvedLocality : "")
                                    });
                                }
                                // add station as favorite
                                else
                                {
                                    _favoritesService.AddFavoriteAsync(new Favorite()
                                    {
                                        Latitude = currentMarkerPosition.Latitude,
                                        Longitude = currentMarkerPosition.Longitude,
                                        Name = favoriteName,
                                        FromStation = true,
                                        Address = _lastResolvedAddress + (!string.IsNullOrWhiteSpace(_lastResolvedLocality) ? ", " + _lastResolvedLocality : "")
                                    });
                                }


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
            ActionMode = null;
        }

        private Intent _createShareIntent()
        {
            var shareIntent = new Intent(Intent.ActionSend);
            var text = "Unknown position";
            if (currentMarkerPosition != null)
            {
                text = FormatShareLocationMessage();
            }
            shareIntent.PutExtra(Intent.ExtraText, text);
            shareIntent.SetType("text/plain");
            return shareIntent;
        }


        private string FormatShareLocationMessage()
        {
            var latitude = Math.Round(currentMarkerPosition.Latitude, 6).ToString(CultureInfo.InvariantCulture);
            var longitude = Math.Round(currentMarkerPosition.Longitude, 6).ToString(CultureInfo.InvariantCulture);
            string body = "Check out this location:\r\n";
            if (!string.IsNullOrWhiteSpace(_lastResolvedAddress))
            {
                body += _lastResolvedAddress + ", " + _lastResolvedLocality + "\r\n";
            }
            //var currentUsername = GetAndroidUsername();
            //var text = "";
            //if (!string.IsNullOrWhiteSpace(currentUsername))
            //{
            //    text += "Shared location by " + currentUsername;
            //}
            //if (!string.IsNullOrWhiteSpace(_lastResolvedAddress))
            //{
            //    text += ": " + _lastResolvedAddress;
            //}
            //text = text.Replace(" ", "%20");
            body += "\r\nUsing EasyBike? Click on the below links:";
            body += "\r\nAndroid: http://easybikeapp.com/?lt=" + latitude + "&ln=" + longitude;
            body += "\r\n\r\nIPhone: http://maps.apple.com/?q=" + latitude + "," + longitude + "&z=17";
            body += "\r\n\r\nWindows Phone: easybike://to/?lt=" + latitude + "&ln=" + longitude;
            body += "\r\n\r\nDon't have EasyBike? Get it now! -> " + StaticResources.WebSiteForStoresURLs;
            //body += "market://details?id=" + PackageName;

            return body;
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
            button.BackgroundTintList = ColorStateList.ValueOf(Resources.GetColor(Resource.Color.primary_light));
            button.SetColorFilter(Color.White);
        }

        private void DecreaseButtonVisibility(FloatingActionButton button)
        {
            button.BackgroundTintList = ColorStateList.ValueOf(Color.White);
            button.SetColorFilter(Color.Gray);
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
        }

        /// <summary>
        /// Switch between Parking view and Bike view
        /// </summary>
        private void SwitchModeStationParking()
        {
            _settingsService.Settings.IsBikeMode = !_settingsService.Settings.IsBikeMode;

            // refresh direction to provide more relevant path
            AddDirectionsAsync();

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

        protected override void OnDestroy()
        {
            // map doesn't load fine when pressing the back button and opening the app again from the running process list #85
            // creating the map again works
            _map.MarkerClick -= _map_MarkerClick;
            _map.MapClick -= _map_MapClick;
            _currentTileNameAnimation.AnimationEnd -= _currentTileNameAnimation_AnimationEnd;
            ClearPolyline();
            _map = null;
            base.OnDestroy();
        }

        protected override void OnResume()
        {
            SetupMapIfNeeded();
            StartLocationTracking();
            base.OnResume();
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
        /// <summary>
        /// disabled by _map_MarkerClick :/
        /// </summary>
        /// <param name="cluster"></param>
        /// <returns></returns>
        public bool OnClusterClick(ICluster cluster)
        {
            //UnStickUserLocation();
            //LatLngBounds.Builder builder = new LatLngBounds.Builder();
            //foreach (ClusterItem item in cluster.Items)
            //{
            //    builder.Include(item.Position);
            //}
            //var bounds = builder.Build();
            //Task.Run(async () =>
            //{
            //    await Task.Delay(300);
            //    RunOnUiThread(() =>
            //    {
            //        _map.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(bounds, 100));
            //    });
            //});

            return false;
        }

        /// <summary>
        /// disabled by _map_MarkerClick :/
        /// </summary>
        /// <param name="marker"></param>
        /// <returns></returns>
        public bool OnClusterItemClick(Java.Lang.Object marker)
        {
            //Log.Debug("MyActivity", "Begin OnClusterItemClick");
            //UnStickUserLocation();
            //if (marker is ClusterItem)
            //{
            //    SelectItem(((ClusterItem)marker).Position);
            //    _actionMode = StartSupportActionMode(this);
            //}
            return false;
        }

        private void AnimateStation(Marker marker)
        {
            ValueAnimator ani = ValueAnimator.OfFloat(0, 1);
            ani.SetDuration(200);
            ani.AddUpdateListener(new Animatorrr(marker));
            ani.Start();
        }

        private void _map_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            if (e.Marker.Title == "cluster")
            {
                UnStickUserLocation();
                _map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(e.Marker.Position, 16));
            }
            else
            {
                _selectedMarker = e.Marker;
                SelectItem(_selectedMarker.Position);
                if (string.IsNullOrWhiteSpace(_selectedMarker.Title)
                    || _selectedMarker.Title == Resources.GetString(Resource.String.mapMarkerImpossible)
                    || _selectedMarker.Title == Resources.GetString(Resource.String.mapMarkerResolving))
                {
                    _selectedMarker.Title = Resources.GetString(Resource.String.mapMarkerResolving);
                }

                _selectedMarker.Snippet = FormatLatLng(e.Marker.Position);
                _selectedMarker.ShowInfoWindow();
                AnimateStation(e.Marker);
            }
        }

        private void ClearPolyline()
        {
            if (_currentPolyline != null)
                _currentPolyline.Remove();
        }

        private string GetDirectionsUrl(LatLng origin, LatLng dest)
        {
            // Origin of route
            var str_origin = "origin=" + origin.Latitude + "," + origin.Longitude;
            // Destination of route
            var str_dest = "destination=" + dest.Latitude + "," + dest.Longitude;
            // Sensor enabled
            var sensor = "sensor=false";
            var mode = "mode=" + (_settingsService.Settings.IsBikeMode ? "walking" : "bicycling");
            // Building the parameters to the web service
            var parameters = str_origin + "&" + str_dest + "&" + sensor + "&" + mode;
            // Output format
            var output = "json";
            // Building the url to the web service
            var url = "https://maps.googleapis.com/maps/api/directions/" + output + "?" + parameters;
            return url;
        }

        private async void AddDirectionsAsync()
        {
            try
            {
                var directions = await GetDirectionsAsync();
                RunOnUiThread(() =>
                {
                    ClearPolyline();
                    if (ActionMode != null)
                        ActionMode.Title = $"{directions.Distance}, {directions.Duration} ({(_settingsService.Settings.IsBikeMode ? "walking" : "cycling")})";
                    _currentPolyline = _map.AddPolyline(directions.Polylines);
                });
            }
            catch { }
        }

        private TaskCompletionSource<DirectionHolder> directionsTCS = new TaskCompletionSource<DirectionHolder>();
        public Task<DirectionHolder> GetDirectionsAsync()
        {
            if (directionsTCS != null)
            {
                if (!directionsTCS.Task.IsCompleted)
                    directionsTCS.TrySetCanceled();
                directionsTCS = new TaskCompletionSource<DirectionHolder>();
            }

            if (_lastUserLocation != null && currentMarkerPosition != null)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        // Getting URL to the Google Directions API
                        var url = GetDirectionsUrl(_lastUserLocation, currentMarkerPosition);
                        using (var client = new HttpClient(new NativeMessageHandler()))
                        {
                            var response = await client.GetAsync(url).ConfigureAwait(false);
                            var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var directions = JsonConvert.DeserializeObject<DirectionsModel>(responseBodyAsText);
                            PolylineOptions lineOptions = new PolylineOptions();
                            lineOptions = lineOptions.InvokeColor(Resources.GetColor(Resource.Color.accent).ToArgb());
                            lineOptions = lineOptions.InvokeWidth(15);

                            if (directions.routes.FirstOrDefault() != null)
                            {
                                var duration = directions.routes.FirstOrDefault().legs.FirstOrDefault().duration;
                                var distance = directions.routes.FirstOrDefault().legs.FirstOrDefault().distance;
                                var points = MapHelper.DecodePolyline(directions.routes.FirstOrDefault().overview_polyline.points).AsEnumerable();
                                foreach (var point in points)
                                {
                                    lineOptions.Add(point);
                                }
                                directionsTCS.SetResult(new DirectionHolder
                                {
                                    Distance = distance.text,
                                    Duration = duration.text,
                                    Polylines = lineOptions
                                });

                            }
                        }
                    }
                    catch
                    {
                    }
                });

            }
            return directionsTCS.Task;
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
            var station = (sender as Station);
            var control = (station.Control as Marker);
            if (station != null && control != null)
            {
                RunOnUiThread(() =>
                {
                    RefreshStation(station, control);
                });
            }
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




        private async void RefreshStation(Station station, Marker control)
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
            _gettingMap = false;
            // TODO TO HELP DEBUG auto download paris to help dev on performances 
            //var contractToTest = "Paris";
            //var contractService = SimpleIoc.Default.GetInstance<IContractService>();
            //var contract = contractService.GetCountries().First(country => country.Contracts.Any(c => c.Name == contractToTest)).Contracts.First(c => c.Name == contractToTest);
            //await SimpleIoc.Default.GetInstance<ContractsViewModel>().AddOrRemoveContract(contract);
            _contractService = SimpleIoc.Default.GetInstance<IContractService>();
            _contractService.ContractRefreshed += OnContractRefreshed;
            _contractService.StationRefreshed += OnStationRefreshed;

            // set the initial visual state of the bike/parking buttons
            SwitchModeStationParkingVisualState();

            _map = googleMap;
            _maxZoom = _map.MaxZoomLevel;

            LoadPreviousTile();

            //Setup and customize your Google Map
            _map.UiSettings.CompassEnabled = true;
            _map.UiSettings.MyLocationButtonEnabled = false;
            _map.UiSettings.MapToolbarEnabled = false;
            _map.MyLocationEnabled = true;

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

            _map.SetOnCameraChangeListener(_clusterManager);
            // this disable the rest of the event handlers
            // so there is no way to distinct a click on a cluster and a marker 
            _map.MarkerClick += _map_MarkerClick;
            //_map.SetOnMarkerClickListener(new OnMarkerClickListener());
            //_clusterManager.SetOnClusterClickListener(this);
            //_clusterManager.SetOnClusterItemClickListener(this);

            // check if the app contains a least one city, otherwise, tells the user to download one
            MainViewModel.MainPageLoadedCommand.Execute(null);

            // On long click, display the address on a info window
            Observable.FromEventPattern<GoogleMap.MapLongClickEventArgs>(_map, "MapLongClick")
                .Subscribe(e =>
                {
                    AddPlaceMarker(e.EventArgs.Point, null, null);
                    SelectItem(e.EventArgs.Point);
                    //IList<Address> addresses = new List<Address>();
                    //try
                    //{
                    //    // Convert latitude and longitude to an address (GeoCoder)
                    //    addresses = await (new Geocoder(this).GetFromLocationAsync(currentMarkerPosition.Latitude, currentMarkerPosition.Longitude, 1));
                    //}
                    //catch (Exception ex)
                    //{
                    //    Log.Debug("MyActivity", "Geocoder crashed: " + ex.Message);
                    //}
                    //return new AddressesFromLocationDTO { Addresses = addresses, Location = FormatLatLng(e.EventArgs.Point) };

                });
            _map.MapClick += _map_MapClick;


            if (_parameterLat != 0 && _parameterLon != 0)
            {
                ShowIntentLocation();
            }
            else
            {
                // center the map on last user location
                GetPreviousLastUserLocation();
            }





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


            _contractService = SimpleIoc.Default.GetInstance<IContractService>();
            var mapObserver = Observable.FromEventPattern(_map, "CameraChange");
            TaskCompletionSource<bool> tcs;
            mapObserver
                .Do((e) =>
                {
                    if (_map.CameraPosition.Zoom > _maxZoom)
                    {
                        _map.MoveCamera(CameraUpdateFactory.ZoomTo(_maxZoom));
                    }
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

        private void _map_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            if (_selectedMarker != null)
            {
                _selectedMarker.HideInfoWindow();
            }
            if (ActionMode != null)
            {
                ActionMode.Finish();

            }
            CloseKeyboard();
        }

        private void ShowIntentLocation()
        {
            var position = new LatLng(_parameterLat, _parameterLon);
            // initiate the map on a location passed as parameter 
            AddPlaceMarker(position, null, FormatLatLng(position));
            SelectItem(position);
            _map.AnimateCamera(CameraUpdateFactory.NewLatLng(position));
        }

        //private string GetAndroidUsername()
        //{
        //    AccountManager manager = AccountManager.Get(this);
        //    var emails = manager.GetAccountsByType("com.google").Select(t => t.Name).ToList();
        //    if (emails.Count > 0 && emails[0] != null)
        //    {
        //        var chunks = emails[0].Split('@');
        //        if (chunks.Length > 1)
        //        {
        //            return chunks[0];
        //        }
        //    }
        //    return string.Empty;
        //}

        private string _lastResolvedAddress;
        private string _lastResolvedLocality;
        private TaskCompletionSource<AddressesFromLocationDTO> addressTCS = new TaskCompletionSource<AddressesFromLocationDTO>();
        private Task<AddressesFromLocationDTO> GetAddressAsync()
        {
            _lastResolvedAddress = string.Empty;
            _lastResolvedLocality = string.Empty;
            if (addressTCS != null)
            {
                if (!addressTCS.Task.IsCompleted)
                    addressTCS.TrySetCanceled();
                addressTCS = new TaskCompletionSource<AddressesFromLocationDTO>();
            }
            // Convert latitude and longitude to an address (GeoCoder)
            Task.Run(async () =>
            {
                try
                {
                    var addresses = await (new Geocoder(this).GetFromLocationAsync(currentMarkerPosition.Latitude, currentMarkerPosition.Longitude, 1));
                    addressTCS.TrySetResult(new AddressesFromLocationDTO()
                    {
                        Addresses = addresses,
                        Location = FormatLatLng(currentMarkerPosition)
                    });
                }
                catch (Exception ex)
                {
                    addressTCS.TrySetException(ex);
                }
            });
            return addressTCS.Task;
        }

        /// <summary>
        /// when app launch before geolocator has found out the user location,
        /// set the last user location to what is in memory
        /// </summary>
        private void GetPreviousLastUserLocation()
        {
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
                    if (_lastUserLocation != null)
                    {
                        _map.AnimateCamera(CameraUpdateFactory.NewLatLng(new LatLng(_lastUserLocation.Latitude, _lastUserLocation.Longitude)));
                    }
                }
                catch { }
            }
        }

        private string FormatLatLng(LatLng position)
        {
            return $"(lat: { Math.Round(position.Latitude, 4)}, lon: { Math.Round(position.Longitude, 4)})";
        }

        /// <summary>
        /// set the marke as the current marker and add directions
        /// </summary>
        private async void SelectItem(LatLng position)
        {
            currentMarkerPosition = position;
            if (ActionMode != null)
            {
                ActionMode.Finish();
            }
            // this needs some delays to appear properly
            Task.Run(async () =>
            {
                await Task.Delay(200);
                RunOnUiThread(() =>
                {
                    ActionMode = StartSupportActionMode(this);
                });
            });
            AddDirectionsAsync();
            if (string.IsNullOrWhiteSpace(_selectedMarker.Title)
                   || _selectedMarker.Title == Resources.GetString(Resource.String.mapMarkerImpossible)
                   || _selectedMarker.Title == Resources.GetString(Resource.String.mapMarkerResolving))
            {
                try
                {
                    var locationDetails = await GetAddressAsync();
                    _lastResolvedAddress = locationDetails.Addresses[0].GetAddressLine(0);
                    _lastResolvedLocality = locationDetails.Addresses[0].Locality;
                    if (_selectedMarker != null && _selectedMarker.Position.Latitude == currentMarkerPosition.Latitude && _selectedMarker.Position.Longitude == currentMarkerPosition.Longitude)
                    {
                        if (locationDetails.Addresses.Any())
                        {
                            _selectedMarker.Title = _lastResolvedAddress;
                            _selectedMarker.Snippet = $"{_lastResolvedLocality} {locationDetails.Location}";
                        }
                        else
                        {
                            _selectedMarker.Title = Resources.GetString(Resource.String.mapMarkerImpossible);
                        }
                        if (_selectedMarker.IsInfoWindowShown)
                        {
                            _selectedMarker.ShowInfoWindow();
                        }
                    }
                }
                catch (TaskCanceledException ex)
                {
                    // ignore task cancellation and etc
                }
                catch (Exception ex)
                {
                    if (_selectedMarker != null)
                    {
                        _selectedMarker.Title = Resources.GetString(Resource.String.mapMarkerImpossible);
                        if (_selectedMarker.IsInfoWindowShown)
                        {
                            _selectedMarker.ShowInfoWindow();
                        }
                    }
                }
            }


        }

        /// <summary>
        /// add a marker for resolved adresses or map long click
        /// </summary>
        /// <param name="position"></param>
        private void AddPlaceMarker(LatLng position, string title, string snippet)
        {
            RunOnUiThread(() =>
            {
                if (longClickMarker != null)
                {
                    // Remove a previously created marker
                    longClickMarker.Remove();
                }
                var markerOptions = new MarkerOptions().SetPosition(position);


                // Create and show the marker
                longClickMarker = _map.AddMarker(markerOptions);
                _selectedMarker = longClickMarker;
                longClickMarker.Title = title ?? Resources.GetString(Resource.String.mapMarkerResolving);
                longClickMarker.Snippet = snippet ?? FormatLatLng(position);
                longClickMarker.ShowInfoWindow();
                if (ActionMode != null)
                {
                    ActionMode.Finish();
                }
                ActionMode = StartSupportActionMode(this);
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
        /**
         * Changes the tiles used to display the map and sets max zoom level.
         *
         * @param overlayString tiles URL for custom tiles or description for
         *                      Google ones
         */

        private TileOverlay _selectedTileOverlay;
        private LinkedListNode<TileContainer> _selectedTile;
        private string _selectedTileName;
        /// <summary>
        /// Load the previous map tile choosen during last launch of the app
        /// </summary>
        private void LoadPreviousTile()
        {
            if (string.IsNullOrWhiteSpace(_settingsService.MapTile))
            {
                _settingsService.MapTile = StaticResources.TilesNormalName;
                return;
            }

            _selectedTile = StaticResources.TilesList.First;
            if (_selectedTile.Value.Name != _settingsService.MapTile)
            {
                do
                {
                    _selectedTile = _selectedTile.Next;
                    if (_selectedTile == null)
                    {
                        break;
                    }
                }
                while (_selectedTile.Value.Name != _settingsService.MapTile);
            }
            UpdateOverlay(true);
        }

        /// <summary>
        /// Set the next Overlay (tile source) from the list of overlays
        /// programatic = true happens when the update is forced by loading the previous tile source
        /// from the previous session (stored in settings)
        /// </summary>
        /// <param name="programatic"></param>
        private float _maxZoom;
        private void UpdateOverlay(bool programatic = false)
        {
            if (!programatic)
            {
                if (_selectedTile == null)
                {
                    _selectedTile = StaticResources.TilesList.First;
                }
                _selectedTile = _selectedTile.Next;
                if (_selectedTile == null)
                {
                    _selectedTile = StaticResources.TilesList.First;
                }
            }

            // if google map, just remove the previous overlay 
            if (_selectedTile.Value.NativeMapLayer)
            {
                if (_selectedTile.Value.Name == StaticResources.TilesHybridName)
                {
                    _map.MapType = GoogleMap.MapTypeHybrid;
                }
                else
                {
                    _map.MapType = GoogleMap.MapTypeNormal;
                }
                if (_selectedTileOverlay != null)
                {
                    _selectedTileOverlay.Remove();
                }
                _maxZoom = (int)_map.MaxZoomLevel;
            }
            else
            {
                _maxZoom = _selectedTile.Value.MaxZoom;
                if (_maxZoom == 0)
                {
                    _maxZoom = (int)_map.MaxZoomLevel;
                }
                if (_map.CameraPosition.Zoom > _maxZoom)
                {
                    _map.MoveCamera(CameraUpdateFactory.ZoomTo(_maxZoom));
                }

                CustomUrlTileProvider mTileProvider = new CustomUrlTileProvider(
                            _selectedTile.Value.TileSize,
                            _selectedTile.Value.TileSize, _selectedTile.Value.TilesUrl);
                var options = new TileOverlayOptions();
                options.InvokeTileProvider(mTileProvider);
                options.InvokeZIndex(-1);
                if (_selectedTileOverlay != null)
                {
                    _selectedTileOverlay.Remove();
                }
                _map.MapType = GoogleMap.MapTypeNone;
                _selectedTileOverlay = _map.AddTileOverlay(options);

            }

            _selectedTileName = _selectedTile.Value.Name;
            if (!programatic)
            {
                _currentTileName.Text = _selectedTile.Value.Name;
                _currentTileName.StartAnimation(_currentTileNameAnimation);
                _currentTileName.Visibility = Android.Views.ViewStates.Visible;
            }
        }


        private void _currentTileNameAnimation_AnimationEnd(object sender, Animation.AnimationEndEventArgs e)
        {
            _currentTileName.StartAnimation(_disappearTileNameAnimation);
        }
    }

    public class Animatorrr : Java.Lang.Object, ValueAnimator.IAnimatorUpdateListener
    {
        private Marker _marker;
        public Animatorrr(Marker marker)
        {
            _marker = marker;
        }

        public void OnAnimationUpdate(ValueAnimator animation)
        {
            _marker.Alpha = (float)animation.AnimatedValue;
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
