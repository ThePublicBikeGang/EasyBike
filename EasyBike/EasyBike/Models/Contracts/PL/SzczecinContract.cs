using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.PL
{
    public class SzczecinContract : Contract
    {
        public SzczecinContract()
        {
            DirectDownloadAvailability = true;
            StationsUrl = "http://atektura.nazwa.pl/atektura.pl/bike_s";
        }

        public override async Task<List<Station>> GetStationsAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl))).ConfigureAwait(false);
                var serviceProviderModels = JsonConvert.DeserializeObject<SzczecinModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false)).Stations;
                var stations = new List<Station>(serviceProviderModels.Length);
                foreach (var serviceProviderModel in serviceProviderModels)
                {
                    stations.Add(new Station()
                    {
                        Latitude = serviceProviderModel.Latitude,
                        Longitude = serviceProviderModel.Longitude,
                        AvailableBikes = serviceProviderModel.AvailableBikes,
                        AvailableBikeStands = serviceProviderModel.AvailableBikeStands,
                        ContractStorageName = StorageName
                    });
                }
                return stations;
            }
        }


        // Barclays refresh every 3 minutes the stations informations :/
        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString()))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<SzczecinModel>(responseBodyAsText).Stations.ToList<StationModelBase>();
            }
        }
    }
}
