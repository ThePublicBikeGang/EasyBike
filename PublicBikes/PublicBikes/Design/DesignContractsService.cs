using PublicBikes.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicBikes.Design
{
    public class DesignContractsService : IContractService
    {
        public Task AddContractAsync(Contract contract)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Contract>> GetContractsAsync()
    {
        return new ContractList().Contracts;
    }

        public List<Contract> GetStaticContracts()
        {
            throw new NotImplementedException();
        }

        public List<Station> GetStations()
        {
            throw new NotImplementedException();
        }

        public Task RemoveContractAsync(Contract contract)
        {
            throw new NotImplementedException();
        }
    }
}
