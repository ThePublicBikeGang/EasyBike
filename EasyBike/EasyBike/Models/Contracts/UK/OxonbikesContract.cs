using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.UK
{
    /// <summary>
    /// website: https://www.oxonbikes.co.uk
    /// </summary>
    public class OxonbikesContract : Contract
    {
        public OxonbikesContract()
        {
            ServiceProvider = "OXONBIKE";
            StationsUrl = "https://www.oxonbikes.co.uk/";
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString()))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var pattern = @"(?<=Operator.StationsData =)(.*])";
                if (Regex.IsMatch(responseBodyAsText, pattern))
                {
                    var regex = new Regex(pattern).Match(responseBodyAsText);
                    if (regex != null && regex.Captures.Count > 0)
                    {
                        var text = regex.Captures[0].Value;
                        var stations = JsonConvert.DeserializeObject<OxonbikesModel[]>(text);
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

    public class OxonbikesModel : StationModelBase
    {
        public override int? AvailableBikeStands { get; set; }

        [JsonProperty("none")]
        public override int? AvailableBikes { get; set; }

        [JsonProperty("AvailableBikes")]
        public AvailablebikesInfo AvailablebikesInfo { get; set; }

        public int TotalLocks { get; set; }
        public int TotalAvailableBikes { get; set; }
        public int id { get; set; }
        public int LockedInExternalLockCount { get; set; }
        public Biketype BikeType { get; set; }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context)
        {
            Longitude = Math.Round(Longitude, 5);
            Latitude = Math.Round(Latitude, 5);
            if(AvailablebikesInfo.TKLocationBikeInfo == null)
            {
                Status = false;
            }
            else
            {
                AvailableBikes = AvailablebikesInfo.TKLocationBikeInfo.Count;
                AvailableBikeStands = TotalLocks - AvailableBikes;
            }
        }
    }


    public class AvailablebikesInfo
    {
        public Tklocationbikeinfo TKLocationBikeInfo { get; set; }
    }

    public class Tklocationbikeinfo
    {
        public int Count { get; set; }
        public object LockedInExternalLockCount { get; set; }
        public string Type { get; set; }
    }

    public class Biketype
    {
        public int Oxford_bike { get; set; }
    }
}

