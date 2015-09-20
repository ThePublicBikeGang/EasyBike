using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.GR
{
    public class EasyBikeContract : Contract
    {
        public EasyBikeContract()
        {
            ServiceProvider = "Easy Bike";
            StationsUrl = "http://map.easybike.gr/";
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString()))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string pattern = @"(?<=var list =)(.*[\s\S]*?])(?=;)";
                if (Regex.IsMatch(responseBodyAsText, pattern))
                {
                    var regex = new Regex(pattern).Match(responseBodyAsText);
                    if (regex != null && regex.Captures.Count > 0)
                    {
                        var text = regex.Captures[0].Value;
                        text = text.Replace("lat:", "\"lat\":");
                        text = text.Replace("lng:", "\"lng\":");
                        text = text.Replace("data:", "\"data\":");
                        text = text.Replace("desc:", "\"desc\":");
                        text = text.Replace("text:", "\"text\":");
                        text = text.Replace("logo:", "\"logo\":");
                        text = text.Replace("options:", "\"options\":");
                        text = text.Replace("icon:", "\"icon\":");
                        text = text.Replace("green_bike", "\"green_bike\"");
                        text = text.Remove(text.LastIndexOf(","), 1);
                        var stations = JsonConvert.DeserializeObject<List<EasyBikeModel>>(text);
                        foreach (var station in stations)
                        {
                            pattern = @"\d+";
                            if (Regex.IsMatch(station.Data.Text, pattern))
                            {
                                var matches = new Regex(pattern).Matches(station.Data.Text);
                                station.AvailableBikes = int.Parse(matches[2].Value);
                                station.AvailableBikeStands = int.Parse(matches[1].Value);
                            }
                        }
                        return stations.ToList<StationModelBase>();

                    }

                }
                return null;
            }
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            return await InnerGetStationsAsync().ConfigureAwait(false);
        }
    }

    public class EasyBikeModel : StationModelBase
    {
        [JsonProperty("lat")]
        public override double Latitude { get; set; }

        [JsonProperty("lng")]
        public override double Longitude { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        public override int? AvailableBikeStands { get; set; }

        public override int AvailableBikes { get; set; }
    }

    public class Data
    {
        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

    }
}

