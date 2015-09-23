using EasyBike.Models.Contracts;
using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyBike.Models
{
    internal class ShanghaiContract : Contract
    {
        public ShanghaiContract()
        {
            StationsUrl = "http://self.chinarmb.com/FormStations.aspx";
            ServiceProvider = "Shanghai Forever Bicycle Rental (no availability)";
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString(), Id))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string pattern = @"(?<=new GLatLng\()([^\)]+)";
                if (Regex.IsMatch(responseBodyAsText, pattern))
                {
                    Regex regex = new Regex(pattern, RegexOptions.None);

                    var stations  = new List<ShanghaiModel>();
                    foreach (Match myMatch in regex.Matches(responseBodyAsText))
                    {
                        if (myMatch.Success)
                        {
                            var values = myMatch.Captures[0].Value.Split(',');
                            double latitutde, longitude;
                            if (!double.TryParse(values[0].Trim(), NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out latitutde))
                                continue;
                            double.TryParse(values[1].Trim(), NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out longitude);

                            stations.Add(new ShanghaiModel()
                            {
                                Latitude = latitutde,
                                Longitude = longitude,
                            });

                            //stationModel.AvailableStr = "?";
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

    public class ShanghaiModel : StationModelBase
    {
        public override double Latitude { get; set; }

        public override double Longitude { get; set; }

        public override int? AvailableBikes { get; set; }

        public override int? AvailableBikeStands { get; set; }
    }
}
