using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PublicBikes.Models
{
    public class RefreshService : IRefreshService
    {
        private List<Contract> contracts = new List<Contract>();
        public event EventHandler ContractRefreshed;

        public RefreshService()
        {
            StartRefreshAsync();
        }
        private async void StartRefreshAsync()
        {
            while (true)
            {
                contracts.ToList().Where(c => c.DirectDownloadAvailability && !c.DownloadingAvailability).AsParallel().ForAll(async (c) =>
                 {
                     Debug.WriteLine("refreshing contract : " + c.Name);
                     await c.RefreshAsync().ConfigureAwait(false);
                     ContractRefreshed?.Invoke(c, EventArgs.Empty);
                 });
                await Task.Delay(3000).ConfigureAwait(false);
            }
        }

        public async void AddContract(Contract contract)
        {
            contracts.Add(contract);
            await contract.RefreshAsync().ConfigureAwait(false);
            ContractRefreshed?.Invoke(contract, EventArgs.Empty);
        }

        public void RemoveContract(Contract contract)
        {
            var contractToRemove = contracts.FirstOrDefault(c => c.StorageName == contract.StorageName);
            if(contractToRemove != null)
            {
                contracts.Remove(contractToRemove);
            }
        }
    }
}
