
using GalaSoft.MvvmLight.Ioc;
using ModernHttpClient;
using Newtonsoft.Json;
using EasyBike.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts
{
    /// <summary>
    /// doc : https://developer.jcdecaux.com/
    /// </summary>
    public class JcDecauxContract : Contract
    {
        private static string apiKey;

        public JcDecauxContract()
        {
            ServiceProvider = "JCDecaux";
            StationsUrl = "https://api.jcdecaux.com/vls/v1/stations?contract={0}&apiKey={1}&t={2}";
            AvailabilityUrl = "https://api.jcdecaux.com/vls/v1/stations/{0}?contract={1}&apiKey={2}";
        }

        public override async Task<List<Station>> GetStationsAsync()
        {
            if (apiKey == null)
            {
                apiKey = (await SimpleIoc.Default.GetInstance<IConfigService>().GetConfigAsync()).JcDecauxApiKey;
            }
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl, Name, apiKey, DateTime.Now.Ticks))).ConfigureAwait(false);
                var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var serviceProviderModels = JsonConvert.DeserializeObject<List<JcDecauxModel>>(data);
                var stations = new List<Station>(serviceProviderModels.Count);
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

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            if (apiKey == null)
            {
                apiKey = (await SimpleIoc.Default.GetInstance<IConfigService>().GetConfigAsync()).JcDecauxApiKey;
            }
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl, Name, apiKey, DateTime.Now.Ticks))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<List<JcDecauxModel>>(responseBodyAsText).ToList<StationModelBase>();
            }
        }
    }
}
