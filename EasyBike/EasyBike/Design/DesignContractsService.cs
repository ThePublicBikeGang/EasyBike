using EasyBike.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBike.Design
{
    public class DesignContractsService : IContractService
    {
        public Task AddContractAsync(Contract contract)
        {
            return Task.FromResult<Contract>(null);
        }

        public async Task<List<Contract>> GetContractsAsync()
    {
        return new ContractList().Contracts;
    }

        public List<Contract> GetStaticContracts()
        {
            return new ContractList().Contracts;
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
    }
}
