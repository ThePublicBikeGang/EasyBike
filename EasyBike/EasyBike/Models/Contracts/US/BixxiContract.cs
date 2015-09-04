using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.US
{
    public class BixxiContract : Contract
    {
        public BixxiContract()
        {
            ServiceProvider = "Bixxi";
            StationsUrl = "http://www.melbournebikeshare.com.au/stationmap/data";
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
                responseBodyAsText = responseBodyAsText.Replace('\\', ' ');
                return JsonConvert.DeserializeObject<List<BixxiModel>>(responseBodyAsText).ToList<StationModelBase>();
            }
        }
    }
}
