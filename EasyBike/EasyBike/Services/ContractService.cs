using EasyBike.Models.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GalaSoft.MvvmLight.Views;
using System;
using System.Diagnostics;
using EasyBike.Notification;

namespace EasyBike.Models
{
    public class ContractService : IContractService
    {
        private readonly IStorageService _storageService;

        private List<Contract> contracts = new List<Contract>();
        private List<Station> stations = new List<Station>();
        private List<Station> refreshingPool = new List<Station>();

        public event EventHandler ContractRefreshed;
        public event EventHandler StationRefreshed;

        public ContractService(IStorageService storageService)
        {
            _storageService = storageService;
            GetContractsAsync().ConfigureAwait(false);
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
            
            if (await station.Contract.RefreshAsync(station).ConfigureAwait(false))
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
                        station.Loaded = false;
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
                contracts.ToList().Where(c=> !c.StationRefreshGranularity).AsParallel().ForAll(async (c) =>
                {
                    if (await c.RefreshAsync().ConfigureAwait(false))
                    {
                        ContractRefreshed?.Invoke(c, EventArgs.Empty);
                    }
                });
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
                await Task.Delay(5000).ConfigureAwait(false);
            }
            IsStationWorkerRunning = false;
        }
    }
}
