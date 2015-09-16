using EasyBike.Extensions;
using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.US
{
    public class CapitalBikeShareContract : Contract
    {
        public CapitalBikeShareContract()
        {
            StationsUrl = "http://www.capitalbikeshare.com/data/stations/bikeStations.xml";
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
                return responseBodyAsText.FromXmlString<stations>("").Stations.ToList<StationModelBase>();
            }
        }
    }
}
