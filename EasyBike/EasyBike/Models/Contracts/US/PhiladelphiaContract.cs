using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.US
{
    public class PhiladelphiaContract 
    {
        //public BCycleContract()
        //{
        //    this.ServiceProvider = "B-cycle";
        //    DirectDownloadAvailability = true;
        //    StationsUrl = "https://publicapi.bcycle.com/api/1.0/ListProgramKiosks/{0}";
        //}

        //public override async Task<List<Station>> GetStationsAsync()
        //{
        //    if (apiKey == null)
        //    {
        //        apiKey = (await SimpleIoc.Default.GetInstance<IConfigService>().GetConfigAsync()).BCycleApiKey;
        //    }
        //    using (var client = new HttpClient(new NativeMessageHandler()))
        //    {
        //        client.DefaultRequestHeaders.Add("ApiKey", apiKey);
        //        HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString(), Id))).ConfigureAwait(false);
        //        var serviceProviderModels = JsonConvert.DeserializeObject<List<BCycleModel>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        //        var stations = new List<Station>(serviceProviderModels.Count);
        //        foreach (var serviceProviderModel in serviceProviderModels)
        //        {
        //            stations.Add(new Station()
        //            {
        //                Latitude = serviceProviderModel.Latitude,
        //                Longitude = serviceProviderModel.Longitude,
        //                AvailableBikes = serviceProviderModel.AvailableBikes,
        //                AvailableBikeStands = serviceProviderModel.AvailableBikeStands,
        //                ContractStorageName = StorageName
        //            });
        //        }
        //        return stations;
        //    }
        //}

        //public override async Task<List<StationModelBase>> InnerRefreshAsync()
        //{
        //    if (apiKey == null)
        //    {
        //        apiKey = (await SimpleIoc.Default.GetInstance<IConfigService>().GetConfigAsync()).BCycleApiKey;
        //    }
        //    using (var client = new HttpClient(new NativeMessageHandler()))
        //    {
        //        client.DefaultRequestHeaders.Add("ApiKey", apiKey);
        //        HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString(), Id))).ConfigureAwait(false);
        //        var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        //        return JsonConvert.DeserializeObject<List<BCycleModel>>(responseBodyAsText).ToList<StationModelBase>();
        //    }
        //}
    }
}
