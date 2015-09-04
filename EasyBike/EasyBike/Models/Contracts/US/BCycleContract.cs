using EasyBike.Config;
using GalaSoft.MvvmLight.Ioc;
using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.US
{
    public class BCycleContract : Contract
    {
        private static string apiKey;

        public BCycleContract()
        {
            ServiceProvider = "B-cycle";
            StationsUrl = "https://publicapi.bcycle.com/api/1.0/ListProgramKiosks/{0}";
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            return await InnerRefreshAsync().ConfigureAwait(false);
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            if (apiKey == null)
            {
                apiKey = (await SimpleIoc.Default.GetInstance<IConfigService>().GetConfigAsync()).BCycleApiKey;
            }
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString(), Id))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<List<BCycleModel>>(responseBodyAsText).ToList<StationModelBase>();
            }
        }
    }
}
