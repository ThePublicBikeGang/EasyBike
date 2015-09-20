using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.CN
{
    public class HuiminOperateContract : Contract
    {
        public HuiminOperateContract()
        {
            ServiceProvider = "Guangzhou Huimin Operation System Management";
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString()))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync();

                string pattern = @" (\{.*[\s\S]*?\})";
                if (Regex.IsMatch(responseBodyAsText, pattern))
                {
                    var regex = new Regex(pattern).Match(responseBodyAsText);
                    if (regex != null && regex.Captures.Count > 0)
                    {
                        dynamic stationsModel = JsonConvert.DeserializeObject<dynamic>(regex.Captures[0].Value);

                        var stations = new List<HuiminOperateModel>();
                        foreach (var station in stationsModel)
                        {
                            double latitude, longitude;
                            if (!double.TryParse(station.Value["lng"].Value, NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out longitude))
                                continue;
                            double.TryParse(station.Value["lat"].Value, NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out latitude);


                            latitude = latitude - 0.005689;
                            longitude = longitude - 0.00652;

                            stations.Add(new HuiminOperateModel()
                            {
                                AvailableBikes = (int)station.Value["DQCSZ"].Value,
                                AvailableBikeStands = (int)station.Value["kzcs"].Value,
                                Latitude = latitude,
                                Longitude = longitude
                            });
                        }
                        return stations;
                    }
                }
            }
            return null;
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            return await InnerGetStationsAsync().ConfigureAwait(false);
        }
    }

    public class HuiminOperateModel : StationBaseModel
    {
        [JsonProperty("lat")]
        public override double Latitude { get; set; }

        [JsonProperty("lng")]
        public override double Longitude { get; set; }

        [JsonProperty("DQCSZ")]
        public override int AvailableBikes { get; set; }

        [JsonProperty("kzcs")]
        public override int? AvailableBikeStands { get; set; }
    }
}
