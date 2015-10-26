using GalaSoft.MvvmLight.Ioc;
using EasyBike.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using EasyBike.Models.Stations;

namespace EasyBike.WinPhone.Helpers
{
   
    public class ClusterGenerator
    {
        private const int MAX_CONTROLS = 38;
        private double MAXDISTANCE = 100;
        private IContractService _contractService;
        public readonly List<StationControl> StationControls = new List<StationControl>(MAX_CONTROLS);
        private MapControl _map;
        private TimeSpan throttleTime = TimeSpan.FromMilliseconds(150);
        private CoreDispatcher dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        public readonly List<Station> Items = new List<Station>();
        public ControlTemplate ItemTemplate { get; set; }
        public CancellationTokenSource cts = new CancellationTokenSource();

        BasicGeoposition leftCornerLocation;
        List<Geopoint> mapLocations = null;
        double zoomLevel = 20;

        public ClusterGenerator(MapControl map, ControlTemplate itemTemplate)
        {
            _map = map;
            ItemTemplate = itemTemplate;
            GenerateMapItems();

            _contractService = SimpleIoc.Default.GetInstance<IContractService>();
            _contractService.ContractRefreshed += OnContractRefreshed;
            _contractService.StationRefreshed += OnStationRefreshed;

            // maps event
            var mapObserver = Observable.FromEventPattern(map, "CenterChanged");
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
                            station.Location = new Geopoint(new BasicGeoposition { Latitude = station.Latitude, Longitude = station.Longitude });
                        }
                  

                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        //mapArea = _map.GetViewArea();
                        mapLocations = _map.GetViewLocations();
                        if (mapLocations != null)
                            leftCornerLocation = mapLocations.First().Position;
                        zoomLevel = _map.ZoomLevel;

                        // TESTING only 
                        //velibControls[0].SetValue(MapControl.LocationProperty, new Geopoint(new BasicGeoposition()
                        //    {
                        //        Latitude = mapArea.NorthwestCorner.Latitude,
                        //        Longitude = mapArea.NorthwestCorner.Longitude,
                        //    }));
                        //velibControls[0].ShowVelibStation();
                        //velibControls[1].SetValue(MapControl.LocationProperty, new Geopoint(new BasicGeoposition()
                        //{
                        //    Latitude = mapArea.SoutheastCorner.Latitude,
                        //    Longitude = mapArea.SoutheastCorner.Longitude,
                        //}));
                        //velibControls[1].ShowVelibStation();

                    });
                    //return null;
                    // that could happend is the zoom is really low and the map is turned
                    if (mapLocations == null)
                        return null;

                    var collection = new AddRemoveCollection();
                    collection.ToAdd = stations.Where(t => !Items.Contains(t)
                        && MapExtensions.Contains(mapLocations, t.Latitude, t.Longitude)).Take(MAX_CONTROLS).ToList();
                    if (Items.Count > MAX_CONTROLS + 5)
                        collection.ToAdd.Clear();
                    collection.ToRemove = Items.Where(t => !MapExtensions.Contains(mapLocations, t.Latitude, t.Longitude)).ToList();

                    // precalculate the items offset (that deffer well calculation)
                    foreach (var velib in collection.ToAdd)
                    {
                        velib.GetOffsetLocation2(leftCornerLocation, zoomLevel);
                    }
                    return collection;
                })
                .Switch()
                .Subscribe(x =>
                {
                    if (x == null)
                        return;
                    RefreshView(x, cts.Token);
                });
        }

        private async void OnStationRefreshed(object sender, EventArgs e)
        {
            var station = (sender as Station);
            var control = station.Control;
            if (station != null && control != null)
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => (control as StationControl).RefreshStation(station));
            }
        }

        private async void OnContractRefreshed(object sender, EventArgs e)
        {
            var contract = (sender as Contract);
            foreach (var control in StationControls.Where(c => c.Stations.Count == 1 && c.Stations.FirstOrDefault().IsUiRefreshNeeded && c.Stations.FirstOrDefault().ContractStorageName == contract.StorageName).ToList())
            {
                var station = control.Stations.FirstOrDefault();
                if (station != null && station.IsUiRefreshNeeded) 
                {
                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => control.RefreshStation(station));
                }
            }
        }

        double previousZoom;
        private async void RefreshView(AddRemoveCollection addRemoveCollection, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            // remove out of view items
            foreach (var velib in Items.Where(t => addRemoveCollection.ToRemove.Contains(t)).ToList())
            {
                if (velib.Control != null)
                    ((StationControl)velib.Control).RemoveVelib(velib);
                Items.Remove(velib);
                if (token.IsCancellationRequested)
                {
                    return;
                }
            }

            if (zoomLevel > 15)
                MAXDISTANCE = 1;
            else
                MAXDISTANCE = 100;

            // refresh clusters by removing them from current cluster if required
            // and send them back to the ToAddPool to be retreated
            // (when zoom in)
            if (previousZoom < zoomLevel)
            {
                foreach (var control in StationControls.Where(t => t.Stations.Count > 1))
                {
                    foreach (var velib in control.Stations.ToList().Where(t => t.GetOffsetLocation2(leftCornerLocation, zoomLevel)
                        .GetDistanceTo(control.GetOffsetLocation2(leftCornerLocation, zoomLevel)) > MAXDISTANCE).ToList())
                    {

                        addRemoveCollection.ToAdd.Add(velib);
                        Items.Remove(velib);
                        control.RemoveVelib(velib);
                        await Task.Delay(1);
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }
                    }
                }
            }
            // (when dezoom)
            // refresh clusters clustering clusters
            if (previousZoom > zoomLevel)
            {
                foreach (var alreadyAddedVelib in Items.ToList())
                {
                    if (alreadyAddedVelib.Control == null)
                        continue;

                    Point locationOffset = alreadyAddedVelib.GetOffsetLocation2(leftCornerLocation, zoomLevel);
                    foreach (var alreadyAddedVelib2 in Items.Where(t => t.GetHashCode() != alreadyAddedVelib.GetHashCode()).ToList())
                    {
                        if (alreadyAddedVelib2.Control == null)
                            continue;
                        var loc = alreadyAddedVelib2.GetOffsetLocation2(leftCornerLocation, zoomLevel);
                        double distance = loc.GetDistanceTo(locationOffset);
                        if (distance < MAXDISTANCE && alreadyAddedVelib.Control != alreadyAddedVelib2.Control)
                        {
                            // add the velib the ToAdd collection to be handled again
                            addRemoveCollection.ToAdd.Add(alreadyAddedVelib2);
                            Items.Remove(alreadyAddedVelib2);
                            ((StationControl)alreadyAddedVelib2.Control).RemoveVelib(alreadyAddedVelib2);

                        }
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }
                    }
                }
            }


            previousZoom = zoomLevel;

            foreach (var station in addRemoveCollection.ToAdd)
            {
                AddToCollection(station);
                if (token.IsCancellationRequested)
                {
                    return;
                }
            }

            var list = StationControls.Where(c => c.NeedRefresh && c.Stations.Count == 0);
            foreach (var control in StationControls.Where(c => c.NeedRefresh && c.Stations.Count == 0))
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    control.Hide();
                }
                );
            }
            // finalise the ui cycle
            foreach (var control in StationControls.Where(c => c.NeedRefresh && c.Stations.Count != 0))
            {

                await Task.Delay(TimeSpan.FromMilliseconds(25));
                if (token.IsCancellationRequested)
                {
                    break;
                }
                var stations = control.Stations;
                if (stations.Count == 1)
                {
                    var station = stations.FirstOrDefault();
                    if (station != null)
                    {
                        if (station.Contract.StationRefreshGranularity)
                        {
                            if (!station.IsInRefreshPool)
                            {
                                _contractService.AddStationToRefreshingPool(station);
                            }
                        }
                    }
                }
                var location = control.GetLocation();
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => control.FinaliseUiCycle(dispatcher, location, token));
            }

            foreach (var station in Items)
            {
                // reinit calculated location to refresh it next UI cycle
                station.OffsetLocation = null;
                var control = station.Control as StationControl;
                if (control != null)
                {
                    control.Location = null;
                    control.OffsetLocation.X = 0;
                }
            }

        }
        private void AddToCollection(Station station)
        {
            bool added = false;
            // merge to other velib cluster if required
            // otherwise create a new cluster
            foreach (var allreadyAddedVelib in Items.ToList())
            {
                var control = allreadyAddedVelib.Control;
                if (control == null)
                    continue;
                double distance = (control as StationControl).GetOffsetLocation2(leftCornerLocation, zoomLevel)
                    .GetDistanceTo(station.GetOffsetLocation2(leftCornerLocation, zoomLevel));
                if (distance < MAXDISTANCE)
                {
                    (control as StationControl).AddStation(station);
                    added = true;
                    break;
                }
            }
            if (!added)
            {
                var mapItem = StationControls.FirstOrDefault(t => t.Stations.Count == 0);
                // no more remaining controls
                if (mapItem == null)
                    return;
                mapItem.AddStation(station);
            }
            Items.Add(station);
        }
        private void GenerateMapItems()
        {
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                for (int i = 0; i < MAX_CONTROLS; i++)
                {
                    var item = new StationControl(_map);
                    item.Template = ItemTemplate;
                    item.Opacity = 0;
                    StationControls.Add(item);
                    _map.Children.Add(item);
                }
            });
        }
    }
}
