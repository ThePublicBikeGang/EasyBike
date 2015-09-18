using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.DE
{
      public class MVGContract : Contract
    {
        public MVGContract()
        {
            ServiceProvider = "MVG Mainzer Verkehrsgesellschaft";
            StationsUrl = "http://mobil.mvg-mainz.de/stationen-karte.html?type=1296727025&tx_mvgmeinrad_mvgmeinradstationenfull%5Baction%5D=getStationsAjax&tx_mvgmeinrad_mvgmeinradstationenfull%5Bcontroller%5D=Benutzer";
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            return await InnerRefreshAsync().ConfigureAwait(false);
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "&" + Guid.NewGuid().ToString()))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<List<MVGModel>>(responseBodyAsText).ToList<StationModelBase>();
            }
        }
    }
}
