using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicBikes.Models
{
    public interface IContractService
    {
        List<Contract> GetStaticContracts();
        Task<List<Contract>> GetContractsAsync();
        Task RemoveContractAsync(Contract contract);
        Task AddContractAsync(Contract contract);
        List<Station> GetStations();
    }
}
