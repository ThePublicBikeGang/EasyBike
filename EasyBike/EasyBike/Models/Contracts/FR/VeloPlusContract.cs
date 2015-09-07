using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.FR
{
    public class VeloPlusContract : Contract
    {
        public VeloPlusContract()
        {
            ServiceProvider = "Vélo'+";
            StationsUrl = "https://www.agglo-veloplus.fr/fr/carte/";
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

                string replacePattern = @"(// var spots =.*[\s\S]*?)(?<=;)";
                var regex = new Regex(replacePattern).Match(responseBodyAsText);
                if (regex != null && regex.Captures.Count > 0)
                {
                    responseBodyAsText = responseBodyAsText.Replace(regex.Captures[0].Value, "");

                }
                string pattern = @"(?<=var spots =)(.*[\s\S]);";
                if (Regex.IsMatch(responseBodyAsText, pattern))
                {
                    regex = new Regex(pattern).Match(responseBodyAsText);
                    if (regex != null && regex.Captures.Count > 0)
                    {
                        var text = regex.Captures[0].Value;
                        text = text.Replace(";", "");
                        return JsonConvert.DeserializeObject<List<VeloPlusModel>>(text).ToList<StationModelBase>();
                    }
                }
            }
            return null;
        }
    }
}
