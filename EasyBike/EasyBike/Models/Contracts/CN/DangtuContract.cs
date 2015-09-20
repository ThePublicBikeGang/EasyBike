using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.CN
{
    public class DangtuContract : Contract
    {
        private string _tokenUrl = "http://218.93.33.59:85/map/maanshanmap/dangtuindex.asp";
        public DangtuContract()
        {
            ServiceProvider = "Public Bicycle";
            StationsUrl = "http://218.93.33.59:85/map/maanshanmap/ibikestation.asp";
            StationRefreshGranularity = true;
            ImageAvailability = true;
        }
        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(_tokenUrl + "?" + Guid.NewGuid().ToString()))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync();
                var pattern = @"(?<=ibikestation.asp\?)([^""]*)";
                if (Regex.IsMatch(responseBodyAsText, pattern))
                {
                    var regex = new Regex(pattern).Match(responseBodyAsText);
                    if (regex != null && regex.Captures.Count > 0)
                    {
                        var id = regex.Captures[0].Value;

                        response = await client.GetAsync(new Uri(string.Format("{0}?{1}", StationsUrl, id)));
                        responseBodyAsText = await response.Content.ReadAsStringAsync();

                        pattern = @"(\[.*[\s\S]*?])";
                        if (Regex.IsMatch(responseBodyAsText, pattern))
                        {
                            regex = new Regex(pattern).Match(responseBodyAsText);
                            if (regex != null && regex.Captures.Count > 0)
                            {
                                return JsonConvert.DeserializeObject<List<PublicBicycleModel>>(regex.Captures[0].Value).Cast<StationModelBase>().ToList();
                            }
                        }
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

