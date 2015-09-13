
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
            AvailabilityUrl = "https://api.jcdecaux.com/vls/v1/stations/{0}?contract={1}&apiKey={2}&t={3}";
            StationRefreshGranularity = true;
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            return await InnerRefreshAsync();
        }

        public override async Task<StationModelBase> InnerRefreshAsync(Station station)
        {
            if (apiKey == null)
            {
                apiKey = (await SimpleIoc.Default.GetInstance<IConfigService>().GetConfigAsync()).JcDecauxApiKey;
            }
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(AvailabilityUrl, station.Id, Name, apiKey, DateTime.Now.Ticks))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<JcDecauxModel>(responseBodyAsText);
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
