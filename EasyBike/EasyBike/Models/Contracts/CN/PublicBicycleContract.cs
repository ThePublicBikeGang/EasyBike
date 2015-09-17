using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.CN
{
    /// <summary>
    /// http://www.ibike668.com/
    /// http://www.ibike668.com/CaseMap.asp
    /// http://ws.uibike.com/cityjson.php
    /// </summary>
    public class PublicBicycleContract : Contract
    {
        public PublicBicycleContract()
        {
            ServiceProvider = "Public Bicycle";
            StationRefreshGranularity = true;
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString()))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync();
                string pattern = @"(\[.*[\s\S]*?])";
                if (Regex.IsMatch(responseBodyAsText, pattern))
                {
                    var regex = new Regex(pattern).Match(responseBodyAsText);
                    if (regex != null && regex.Captures.Count > 0)
                    {
                        return JsonConvert.DeserializeObject<List<PublicBicycleModel>>(regex.Captures[0].Value).Cast<StationModelBase>().ToList();
                    }
                }
            }
            return null;
        }

        public override async Task<StationModelBase> InnerRefreshAsync(Station station)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage responseBikes = await client.GetAsync(new Uri(string.Format(AvailabilityUrl, station.Id, 1) + "&t=" + Guid.NewGuid()));
                HttpResponseMessage responseStands = await client.GetAsync(new Uri(string.Format(AvailabilityUrl, station.Id, 2) + "&t=" + Guid.NewGuid()));

                var bikesImageByteArray = await responseBikes.Content.ReadAsByteArrayAsync();
                var standsImageByteArray = await responseStands.Content.ReadAsByteArrayAsync();
                station.ImageAvailable = bikesImageByteArray;
                station.ImageDocks = standsImageByteArray;
                station.IsUiRefreshNeeded = true;
                station.Loaded = true;
                return null;
            }
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            return null;
        }
    }
}
