using System;

namespace PublicBikes.Models
{
    public interface IRefreshService
    {
        void AddContract(Contract contract);
        void RemoveContract(Contract contract);
        event EventHandler ContractRefreshed;
    }
}
