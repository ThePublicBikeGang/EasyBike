using EasyBike.Models.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Connectivity.Plugin;
using EasyBike.Services;
using EasyBike.Helpers;
using System.Diagnostics;

namespace EasyBike.Models
{
    public class ContractService : IContractService
    {
        private readonly IStorageService _storageService;
        private readonly ILocalisationService _localisationService;

        private List<Contract> contracts = new List<Contract>();
        private List<Station> stations = new List<Station>();
        private List<Station> refreshingPool = new List<Station>();

        public event EventHandler ContractRefreshed;
        public event EventHandler StationRefreshed;

        public ContractService(ILocalisationService localisationService, IStorageService storageService)
        {
            _localisationService = localisationService;
            _storageService = storageService;
           // GetContractsAsync().ConfigureAwait(false);
        }

        public async Task AddContractAsync(Contract contract)
        {
            await _storageService.StoreContractAsync(contract).ConfigureAwait(false);
            contracts.Add(contract);
            AggregateStations(contract);
            //ContractRefreshed?.Invoke(contract, EventArgs.Empty);
        }

        public async Task RemoveAllContractsAsync()
        {
            await _storageService.RemoveAllContractsAsync();
            stations.Clear();
            contracts.Clear();
        }

        public async Task RemoveContractAsync(Contract contract)
        {
            await _storageService.RemoveContractAsync(contract).ConfigureAwait(false);
            foreach (var station in stations.Where(s => s.ContractStorageName == contract.StorageName).ToList())
            {
                stations.Remove(station);
            }
            contracts.Remove(contract);
        }

        public List<Station> GetStations()
        {
            return stations;
        }

        public List<Country> GetCountries()
        {
            return ContractList.Countries;
        }

        public async void AddStationToRefreshingPool(Station station)
        {
            station.IsInRefreshPool = true;
            var contract = station.Contract;
            if (contract != null)
            {
                if (await contract.RefreshAsync(station).ConfigureAwait(false))
                {
                    if (station.IsUiRefreshNeeded)
                    {
                        StationRefreshed?.Invoke(station, EventArgs.Empty);
                    }
                }
                refreshingPool.Add(station);
                if (!IsStationWorkerRunning)
                {
                    StartRefreshStationsAsync();
                }
            }
        }

        public void RemoveStationFromRefreshingPool(Station station)
        {
            refreshingPool.Remove(station);
            station.IsInRefreshPool = false;
        }

        public async Task<List<Contract>> GetContractsAsync()
        {
            if (contracts.Count == 0)
            {
                var storedContracts = (await _storageService.LoadStoredContractsAsync().ConfigureAwait(false)).ToList();

                foreach (var contract in storedContracts)
                {
                    foreach (var station in contract.Stations)
                    {
                        station.Contract = contract;
                        station.IsUiRefreshNeeded = true;
                        stations.Add(station);
                    }
                }
                contracts = storedContracts;
                StartRefreshAsync();
            }
            return contracts;
        }

        private void AggregateStations(Contract contract)
        {
            foreach (var station in contract.Stations)
            {
                station.Contract = contract;
                stations.Add(station);
            }
        }


        private int timer = 1000;
        private async void StartRefreshAsync()
        {
            while (true)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    // DispatcherHelper.
                    // At this time, DispatcherHelper cannot be used in a portable class library. Laurent works on a solution.
                    // var mapCenter = _localisationService.GetCurrentMapCenter();
                    contracts.ToList().Where(c => !c.StationRefreshGranularity).AsParallel().ForAll(async (c) =>
                    {
                        if (await c.RefreshAsync().ConfigureAwait(false))
                        {
                            ContractRefreshed?.Invoke(c, EventArgs.Empty);
                        }

                        //var station = c.Stations.FirstOrDefault();
                        //if(station != null)
                        //{
                        //    if(mapCenter != null)
                        //    {
                        //        var distance = MapHelper.CalcDistance(mapCenter.Latitude, mapCenter.Longitude, station.Latitude, station.Longitude);
                        //        if (distance > 100)
                        //        {
                        //            Debug.WriteLine("DISTANCE : " + distance + " ...");
                        //            return;
                        //        }
                        //        else
                        //        {
                        //            Debug.WriteLine("DISTANCE : " + distance + " Refresh!");
                        //            if (await c.RefreshAsync().ConfigureAwait(false))
                        //            {
                        //                ContractRefreshed?.Invoke(c, EventArgs.Empty);
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (await c.RefreshAsync().ConfigureAwait(false))
                        //        {
                        //            ContractRefreshed?.Invoke(c, EventArgs.Empty);
                        //        }
                        //    }
                        //}
                    });
                }

                await Task.Delay(timer).ConfigureAwait(false);
                timer = timer <= 20000 ? timer + 5000 : timer;
            }
        }

        private bool IsStationWorkerRunning = false;
        private async void StartRefreshStationsAsync()
        {
            IsStationWorkerRunning = true;
            while (refreshingPool.Count > 0)
            {
                await Task.Delay(15000).ConfigureAwait(false);
                if (CrossConnectivity.Current.IsConnected)
                {
                    //Parallel.ForEach(refreshingPool.ToList(), async (station) =>
                    //{
                    //    if (await station.Contract.RefreshAsync(station).ConfigureAwait(false))
                    //    {
                    //        if (station.IsUiRefreshNeeded)
                    //        {
                    //            StationRefreshed?.Invoke(station, EventArgs.Empty);
                    //        }
                    //    }
                    //});
                    refreshingPool.ToList().AsParallel().ForAll(async (station) =>
                    {
                        if (await station.Contract.RefreshAsync(station).ConfigureAwait(false))
                        {
                            if (station.IsUiRefreshNeeded)
                            {
                                StationRefreshed?.Invoke(station, EventArgs.Empty);
                            }
                        }
                    });
                }
                await Task.Delay(5000).ConfigureAwait(false);
            }
            IsStationWorkerRunning = false;
        }
    }
}
