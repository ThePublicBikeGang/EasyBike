using EasyBike.Models.Contracts;
using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.FR
{
    public class VeoliaContract : Contract
    {
        public VeoliaContract()
        {
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            return await InnerRefreshAsync().ConfigureAwait(false);
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString()))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var models = JsonConvert.DeserializeObject<VeoliaModel>(responseBodyAsText);
                return JsonConvert.DeserializeObject<VeoliaModel>(responseBodyAsText).stand.ToList<StationModelBase>();
            }
        }
    }
}

public class VeoliaModel
{
    public string org { get; set; }
    public string cou { get; set; }
    public string lng { get; set; }
    public string lat { get; set; }
    public string zoom { get; set; }
    public string desc { get; set; }
    public string web { get; set; }
    public string reg { get; set; }
    public string gmt { get; set; }
    public VeoliaStation[] stand { get; set; }
    public string loc { get; set; }
}

public class VeoliaStation : StationModelBase
{
    //    {
    //    public string wcom { get; set; }
    //    public string disp { get; set; }
    //    public string neutral { get; set; }
    //    public string lng { get; set; }
    //    public string lat { get; set; }
    //    public string tc { get; set; }
    //    public string ac { get; set; }
    //    public string ap { get; set; }
    //    public string ab { get; set; }
    //    public string id { get; set; }
    //    public string name { get; set; }
    //}
    [JsonProperty(PropertyName = "id")]
    public override string Id { get; set; }

    [JsonProperty(PropertyName = "ab")]
    public override int? AvailableBikes { get; set; }

    [JsonProperty(PropertyName = "ap")]
    public override int? AvailableBikeStands { get; set; }

    [JsonProperty(PropertyName = "lat")]
    public string Lat { get; set; }

    [JsonProperty(PropertyName = "lng")]
    public string Lng { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "disp")]
    public string innerStatus { get; set; }

    [OnDeserialized]
    internal new void OnDeserializedMethod(StreamingContext context)
    {
        Latitude = Math.Round(double.Parse(Lat), 5);
        Longitude = Math.Round(double.Parse(Lng), 5);
        Status = innerStatus == "1" ? true : false;
    }
}
