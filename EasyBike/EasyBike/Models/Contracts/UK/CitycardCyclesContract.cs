using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.UK
{
    /// <summary>
    /// website: http://www.nottinghamcity.gov.uk/CitycardCycles
    /// http://www.citycardcycles.co.uk/CustomerPortal/locate.aspx/
    /// </summary>
    public class CitycardCyclesContract : Contract
    {
        public CitycardCyclesContract()
        {
            ServiceProvider = "Citycard Cycles";
            StationsUrl = "https://www.google.com/maps/d/embed?mid=zb0mL1oLfseA.kKf_t6RIlfCc";
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "&?" + Guid.NewGuid().ToString()))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var pattern = @"((location).*?(\[\\\"")(.*?)(\\)(.*?)(\d2.*?)(\\))+";
                if (Regex.IsMatch(responseBodyAsText, pattern))
                {
                    var regex = new Regex(pattern, RegexOptions.None);
                    var stations = new List<StationModelBase>();
                    foreach (Match myMatch in regex.Matches(responseBodyAsText))
                    {
                        if (myMatch.Success)
                        {
                            var values = myMatch.Groups[7].Value.Split(',');
                            if(values.Length == 2)
                            {
                                var station = new CitycardCyclesModel();
                                station.Latitude = double.Parse(values[0], NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, new CultureInfo("en-US"));
                                station.Longitude = double.Parse(values[1].Trim(), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, new CultureInfo("en-US"));
                                stations.Add(station);
                            }
                        }
                    }
                    return stations;
                }
                return null;
            }
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            return await InnerGetStationsAsync().ConfigureAwait(false);
        }
    }

    public class CitycardCyclesModel : StationModelBase
    {
        public override int? AvailableBikeStands { get; set; }
        public override int? AvailableBikes { get; set; }
    }
}

