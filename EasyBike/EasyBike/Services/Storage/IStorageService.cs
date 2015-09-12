using EasyBike.Services.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBike.Models.Storage
{
    public interface IStorageService
    {
        Task StoreAsync<T>(string key, T obj);
        Task<T> GetAsync<T>(string key);
        Task<IEnumerable<T>> GetAsync<T>();
        Task RemoveAsync(string key);


        Task StoreContractAsync(Contract contract);
        Task RemoveContractAsync(Contract contract);
        Task RemoveAllContractsAsync();
        Task ClearAsync();
        Task<IEnumerable<Contract>> LoadStoredContractsAsync();

        // settings
        Task<SettingsModel> GetSettingsAsync();
        Task SetSettingsAsync(SettingsModel settings);

        // favorits
    }
}
