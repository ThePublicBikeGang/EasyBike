using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.DE
{
    public class CallABikeContract : Contract
    {
        public CallABikeContract()
        {
            StationsUrl = "https://www.callabike-interaktiv.de/kundenbuchung/hal2ajax_process.php?mapstadt_id={0}&with_staedte=N&ajxmod=hal2map&callee=getMarker";
            ServiceProvider = "Call a Bike (no dock availabilty :/)";
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
                return JsonConvert.DeserializeObject<CallABikeModel>(responseBodyAsText).stations.ToList<StationModelBase>();
            }
        }
    }
}
