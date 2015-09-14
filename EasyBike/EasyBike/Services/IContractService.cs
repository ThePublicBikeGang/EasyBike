using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBike.Models
{
    public interface IContractService
    {
        List<Country> GetCountries();
        Task<List<Contract>> GetContractsAsync();
        Task RemoveContractAsync(Contract contract);
        Task RemoveAllContractsAsync();
        Task AddContractAsync(Contract contract);
        List<Station> GetStations();
        void AddStationToRefreshingPool(Station station);
        void RemoveStationFromRefreshingPool(Station station);

        event EventHandler ContractRefreshed;
        event EventHandler StationRefreshed;
    }
}
