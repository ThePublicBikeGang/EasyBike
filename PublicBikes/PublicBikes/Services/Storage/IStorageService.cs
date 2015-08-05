using PublicBikes.Services.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicBikes.Models.Storage
{
    public interface IStorageService
    {
        Task StoreContractAsync(Contract contract);
        Task RemoveContractAsync(Contract contract);
        Task ClearAsync();
        Task<IEnumerable<Contract>> LoadStoredContractsAsync();

        // settings
        Task<SettingsModel> GetSettingsAsync();
        Task SetSettingsAsync(SettingsModel settings);
    }
}
