using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.US
{
    public class BixxiMinneapolisContract : Contract
    {
        public BixxiMinneapolisContract()
        {
            ServiceProvider = "Bixi";
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString()))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<BixxiModelMinneapolis>(responseBodyAsText).Stations.ToList<StationModelBase>();

            }
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            return await InnerGetStationsAsync().ConfigureAwait(false);
        }
    }

    public class BixxiModelMinneapolis
    {
        [JsonProperty("stations")]
        public Stations[] Stations;
    }

    public class Stations : StationModelBase
    {
        [JsonProperty("la")]
        public override double Latitude { get; set; }

        [JsonProperty("lo")]
        public override double Longitude { get; set; }

        [JsonProperty("ba")]
        public override int? AvailableBikes { get; set; }

        [JsonProperty("da")]
        public override int? AvailableBikeStands { get; set; }

    }

}
