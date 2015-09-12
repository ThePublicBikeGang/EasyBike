using System;

namespace EasyBike.Models
{
    public interface IRefreshService
    {
        void AddContract(Contract contract);
        void RemoveContract(Contract contract);
        void RemoveAllContracts();
        event EventHandler ContractRefreshed;
    }
}
