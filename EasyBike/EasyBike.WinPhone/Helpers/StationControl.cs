using EasyBike.Models;
using EasyBike.Models.Storage;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace EasyBike.WinPhone.Helpers
{
    public class StationControl : ContentControl
    {
        static MapControl map;
        static SolidColorBrush emptyColorBrush = new SolidColorBrush(Color.FromArgb(150, 155, 155, 155));
        static SolidColorBrush redColorBrush = new SolidColorBrush(Color.FromArgb(255, 211, 67, 14));
        static SolidColorBrush orangeColorBrush = new SolidColorBrush(Color.FromArgb(255, 230, 178, 0));
        static SolidColorBrush greenColorBrush = new SolidColorBrush(Color.FromArgb(255, 95, 205, 0));

        private ISettingsService _settingsService;
        private IContractService _contractService;


        public StationControl(MapControl map)
        {
            StationControl.map = map;
            Tapped += Control_Tapped;
            Holding += Control_Holding;
            ManipulationMode = Windows.UI.Xaml.Input.ManipulationModes.All;
            _settingsService = SimpleIoc.Default.GetInstance<ISettingsService>();
            _contractService = SimpleIoc.Default.GetInstance<IContractService>();
        }

        private async void Control_Holding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            try
            {
                if (Stations.Count == 1)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        MainPage.mainPage.ShowFlyout(this);
                    });
                }
            }
            catch { }
        }

        public List<Station> Stations = new List<Station>();
        public TextBlock ClusterTextBlock;
        public Windows.UI.Xaml.Shapes.Path StationPath;
        protected override void OnApplyTemplate()
        {
            ClusterTextBlock = GetTemplateChild("textBlockClusterNumber") as TextBlock;
            StationPath = GetTemplateChild("path") as Windows.UI.Xaml.Shapes.Path;
        }

        private static StationControl previousVelibTapped;
        private async void Control_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            try
            {
                if (Stations.Count > 1)
                {
                    if (_settingsService.Settings.IsCompassMode)
                    {
                        MainPage.mainPage.StopCompassAndUserLocationTracking();
                    }
                    await map.TrySetViewBoundsAsync(MapExtensions.GetAreaFromLocations(Stations.Select(s => (Geopoint)s.Location).ToList()), new Thickness(20, 20, 20, 20), MapAnimationKind.Default);
                }
              
                else
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        MainPage.mainPage.SelectItem(this, false);
                    });
                }
            }
            catch 
            {
            }
        }


        public void AddStation(Station station)
        {
            station.Control = this;
            Stations.Add(station);
            NeedRefresh = true;
        }

        public void FinaliseUiCycle(CoreDispatcher dispatcher, Geopoint location, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            var station = Stations.FirstOrDefault();
            SetValue(MapControl.LocationProperty, location);
            DataContext = station;

            if (Stations.Count == 1)
            {
                SwitchModeVelibParking(station);
            }
            else if (Stations.Count > 1)
            {
                ClusterTextBlock.Text = Stations.Count.ToString();
                ShowCluster();
            }
            NeedRefresh = false;
        }

        public void RefreshStation(Station station)
        {
            if(StationPath.Fill != emptyColorBrush)
            {
                ShowPulseAnimation();
            }
            if (station.ImageAvailable != null)
            {
                VisualStateManager.GoToState(this, "ShowNumberImage", false);
            }
            ShowStationColorAsync(station);
        }

        public void SwitchModeVelibParking(Station station)
        {
            if (station == null)
                return;

            ShowVelibStation();
            ShowStationColorAsync(station);
        }

        public bool NeedRefresh;
        public bool ToRemove;
        public void RemoveVelib(Station station)
        {
            NeedRefresh = true;
            station.Control = null;
            Stations.Remove(station);
            if(station.IsInRefreshPool) {
                _contractService.RemoveStationFromRefreshingPool(station);
            }
        }

        public Point MapLocation { get; set; }

        public bool alreadyHandled;
        public bool Selected { get; set; }

        public StationControl()
        {
            DefaultStyleKey = typeof(StationControl);
        }

        public void ShowCluster()
        {
            VisualStateManager.GoToState(this, "Normal", false);
            VisualStateManager.GoToState(this, "ShowCluster", false);
            // VisualStateManager.GoToState(this, "Clear", false);
            // VisualStateManager.GoToState(this, "Loaded", false);
            Opacity = 1;
            IsHitTestVisible = true;
        }
        public void ShowPulseAnimation()
        {
            VisualStateManager.GoToState(this, "Normal", false);
            VisualStateManager.GoToState(this, "ShowStation", false);
        }


        public void ShowVelibStation()
        {
            ShowPulseAnimation();

            var station = Stations.FirstOrDefault();
            if (station.ImageAvailable != null)
            {
                VisualStateManager.GoToState(this, "ShowNumberImage", false);
            }
            StationPath.Fill = emptyColorBrush;
            Opacity = 1;
            IsHitTestVisible = true;
        }

        public async void ShowStationColorAsync(Station station)
        {
            if (!station.Loaded)
                return;
            station.IsUiRefreshNeeded = false;
            if (_settingsService.Settings.IsBikeMode)
            {
                if (station.ImageAvailable != null)
                {
                    using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                    {
                        var image = new BitmapImage();
                        image.DecodePixelHeight = 15;
                        await stream.WriteAsync(((byte[])station.ImageAvailable).AsBuffer());
                        stream.Seek(0);
                        await image.SetSourceAsync(stream);
                        station.ImageNumber = image;
                    }
                }
                else
                {
                    station.AvailableStr = station.AvailableBikes.ToString();
                    ShowColor(station.AvailableBikes);
                }
            }
            else
            {
                if (station.ImageDocks != null)
                {
                    using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                    {
                        var image = new BitmapImage();
                        await stream.WriteAsync(((byte[])station.ImageDocks).AsBuffer());
                        stream.Seek(0);
                        await image.SetSourceAsync(stream);
                        station.ImageNumber = image;
                    }
                }
                else
                {
                    station.AvailableStr = station.AvailableBikeStands.HasValue ? station.AvailableBikeStands.ToString() : "?";
                    ShowColor(station.AvailableBikeStands);
                }
            }
        }



        private void ShowColor(int? number)
        {
            if (Stations.Count != 1)
                return;

            var color = emptyColorBrush;
            if (number == -1 || !number.HasValue)
            {
            }
            else
            {

                if (number == 0)
                    color = redColorBrush;
                else if (number < 5)
                    color = orangeColorBrush;
                else if (number >= 5)
                    color = greenColorBrush;
            }

            if(StationPath.Fill != color)
            {
                if (StationPath.Fill != emptyColorBrush)
                {
                  //  ShowPulseAnimation();
                }
                StationPath.Fill = color;
            }
            


        }

        public void Hide()
        {
            Opacity = 0;
            IsHitTestVisible = false;
            NeedRefresh = false;
            //VisualStateManager.GoToState(this, "Hide", false);
        }


        public Geopoint Location;

        internal Geopoint GetLocation()
        {
            var firstStation = Stations.FirstOrDefault();
            if (Stations.Count == 1 && firstStation != null)
            {
                return Location = (Geopoint)firstStation.Location;
            }


            double x = 0;
            double y = 0;
            foreach (var station in Stations.ToList())
            {
                x += ((Geopoint)station.Location).Position.Latitude;
                y += ((Geopoint)station.Location).Position.Longitude;
            }
            Location = new Geopoint(new BasicGeoposition { Latitude = x / Stations.Count, Longitude = y / Stations.Count });
            return Location;
        }

        // store the offsetLocation in order to reuse it for each draw cycle
        public Point OffsetLocation;
        public Point GetOffsetLocation(MapControl _map)
        {
            if (OffsetLocation.X == 0)
                _map.GetOffsetFromLocation(this.GetLocation(), out OffsetLocation);
            return OffsetLocation;
        }
        public Point GetOffsetLocation2(BasicGeoposition origin, double zoomLevel)
        {
            if (OffsetLocation.X == 0)
                OffsetLocation = origin.GetOffsetedLocation(this.GetLocation().Position, zoomLevel);
            return OffsetLocation;
        }

    }
}
