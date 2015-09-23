using EasyBike.Extensions;
using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.US
{
    /// <summary>
    /// Info : http://nextbike.net/maps/nextbike-live.xml
    /// or https://nextbike.net/maps/nextbike-official.xml
    /// </summary>
    public class NextBikeContract : Contract
    {
        public NextBikeContract()
        {
            ServiceProvider = "NextBike";
            StationsUrl = "http://nextbike.net/maps/nextbike-live.xml?city={0}";
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            return await InnerRefreshAsync().ConfigureAwait(false);
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "&" + Guid.NewGuid().ToString(), Id))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return responseBodyAsText.FromXmlString<markers>("").Items.FirstOrDefault().city.FirstOrDefault().place.ToList<StationModelBase>();
            }
        }
    }
}
