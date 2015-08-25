using Akavache;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Splat;
using EasyBike.Services.Settings;
using System;

namespace EasyBike.Models.Storage
{
    /// <summary>
    /// Akavache documentation : https://github.com/akavache/Akavache
    /// </summary>
    public class StorageService : IStorageService
    {
        public StorageService()
        {
            BlobCache.ApplicationName = "EasyBike";
            var jsonSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            Locator.CurrentMutable.RegisterConstant(jsonSettings, typeof(JsonSerializerSettings));
        }

        public async Task StoreAsync<T>(string key, T obj)
        {
            await BlobCache.LocalMachine.InsertObject(key, obj);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            return await BlobCache.LocalMachine.GetObject<T>(key);
        }

        public async Task<IEnumerable<T>> GetAsync<T>()
        {
            return await BlobCache.LocalMachine.GetAllObjects<T>();
        }

        public async Task RemoveAsync(string key)
        {
            await BlobCache.LocalMachine.Invalidate(key);
        }


        public async Task StoreContractAsync(Contract contract)
        {
            await BlobCache.LocalMachine.InsertObject(contract.StorageName, contract);
        }

        public async Task RemoveContractAsync(Contract contract)
        {
            await BlobCache.LocalMachine.Invalidate(contract.StorageName);
        }

        public async Task<IEnumerable<Contract>> LoadStoredContractsAsync()
        {
            return await BlobCache.LocalMachine.GetAllObjects<Contract>();
        }

        public async Task ClearAsync()
        {
            await BlobCache.LocalMachine.InvalidateAll();
        }

        public async Task<SettingsModel> GetSettingsAsync()
        {
            try
            {
                return await BlobCache.LocalMachine.GetObject<SettingsModel>("settings");
            }
            catch
            {
                return new SettingsModel();
            }
        }

        public async Task SetSettingsAsync(SettingsModel settings)
        {
            await BlobCache.LocalMachine.InsertObject("settings", settings);
        }
    }
}
