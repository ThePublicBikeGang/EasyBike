using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.CN
{
    public class ShanghaiContract : Contract
    {
        public ShanghaiContract()
        {
            ServiceProvider = "Shanghai Forever Bicycle Rental (no availability)";
            StationsUrl = "http://self.chinarmb.com/FormStations.aspx";
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString()))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync();
                string pattern = @"(?<=new GLatLng\()([^\)]+)";
                if (Regex.IsMatch(responseBodyAsText, pattern))
                {
                    Regex regex = new Regex(pattern, RegexOptions.None);

                    var stations = new List<ShangaiModel>();
                    foreach (Match myMatch in regex.Matches(responseBodyAsText))
                    {
                        if (myMatch.Success)
                        {
                            var values = myMatch.Captures[0].Value.Split(',');
                            double latitutde, longitude;
                            if (!double.TryParse(values[0].Trim(), NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out latitutde))
                                continue;
                            double.TryParse(values[1].Trim(), NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out longitude);

                            var station = new ShangaiModel()
                            {
                                Latitude = latitutde,
                                Longitude = longitude,
                            };
                            stations.Add(station);
                        }
                    }
                    return stations.ToList<StationModelBase>();
                }
            }
            return null;
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            return await InnerGetStationsAsync().ConfigureAwait(false);
        }
    }

    public class ShangaiModel : StationModelBase
    {
        [JsonProperty("latitude")]
        public override double Latitude { get; set; }

        [JsonProperty("longitude")]
        public override double Longitude { get; set; }

        [JsonProperty("bikes_available")]
        public override int? AvailableBikes { get; set; }

        [JsonProperty("docks_available")]
        public override int? AvailableBikeStands { get; set; }
    }
}


