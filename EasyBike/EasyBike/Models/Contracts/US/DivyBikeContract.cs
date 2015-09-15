using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace EasyBike.Models.Contracts.US
{
    public class DivyBikeContract : Contract
    {
        public DivyBikeContract()
        {
            ServiceProvider = "Divvy";
            StationsUrl = "http://www.divvybikes.com/stations/json";
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            return await InnerRefreshAsync().ConfigureAwait(false);
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString(), Id))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<DivyBikeModel>(responseBodyAsText).Stations.ToList<StationModelBase>();
            }
        }
    }
}
