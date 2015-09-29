using EasyBike.Extensions;
using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.FR
{
    public class SmooveContract : Contract
    {
        public SmooveContract()
        {
            ServiceProvider = "Smoove";
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
                var models = responseBodyAsText.FromXmlString<vcs>("").Node.Stations.ToList();
                // for duplicates :/
                var dupplicates = models.GroupBy(x => x.LatitudeStr).Where(g => g.Count() > 1).ToList();
                if (dupplicates.Count > 0)
                {
                    var aggregatedStation = dupplicates.Select(t =>
                        new station
                        {
                            AvailableBikes = t.Sum(b => b.AvailableBikes),
                            AvailableBikeStands = t.Sum(b => b.AvailableBikeStands),
                            Id = t.FirstOrDefault().Id,
                            LatitudeStr = t.FirstOrDefault().LatitudeStr,
                            LongitudeStr = t.FirstOrDefault().LongitudeStr,
                            TotalDocks = t.Sum(b => b.TotalDocks)

                        });
                    models.RemoveAll(t => aggregatedStation.Any(v => v.LatitudeStr == t.LatitudeStr && v.LongitudeStr == t.LongitudeStr));
                    models.Add(aggregatedStation.FirstOrDefault());
                }
                foreach (var station in models)
                {
                    double latitude, longitude = 0;
                    if (!double.TryParse(station.LatitudeStr, NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out latitude))
                        continue;
                    double.TryParse(station.LongitudeStr, NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out longitude);
                    station.Latitude = latitude;
                    station.Longitude = longitude;
                }

                return models.ToList<StationModelBase>();
            }
        }
    }
}
