using EasyBike.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBike.Design
{
    public class DesignContractsService : IContractService
    {
        public event EventHandler ContractRefreshed;
        public event EventHandler StationRefreshed;

        public Task AddContractAsync(Contract contract)
        {
            return Task.FromResult<Contract>(null);
        }

        public void AddStationToRefreshingPool(Station station)
        {
        }

        public async Task<List<Contract>> GetContractsAsync()
    {
        return new List<Contract>();
    }

        public List<Country> GetCountries()
        {
            return new List<Country>();
        }

        public List<Station> GetStations()
        {
            return new List<Station>();
        }

        public Task RemoveAllContractsAsync()
        {
            return Task.FromResult<Contract>(null);
        }

        public Task RemoveContractAsync(Contract contract)
        {
            return Task.FromResult<Contract>(null);
        }

        public void RemoveStationFromRefreshingPool(Station station)
        {
        }
    }
}
