using System;
using System.Threading.Tasks;
using EasyBike.Extensions;
using System.Net.Http;
using ModernHttpClient;
using System.Collections.Generic;
using System.Linq;

namespace EasyBike.Models.Contracts
{
	/// <summary>
	/// website : http://opensourcebikeshare.com/
	/// </summary>
	public class OpenSourceBikeShareContract : Contract
	{
		public OpenSourceBikeShareContract()
		{
			ServiceProvider = "Open Source Bike Share";
			StationsUrl = "http://whitebikes.info/command.php?action=map:markers";
		}

		public override async Task<List<StationModelBase>> InnerGetStationsAsync()
		{
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(StationsUrl + "&g=" + Guid.NewGuid().ToString())).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return responseBodyAsText.FromJsonString<OpenSourceBikeShareModel[]>().ToList<StationModelBase>();
            }
		}

		public override async Task<List<StationModelBase>> InnerRefreshAsync()
		{
            return await InnerGetStationsAsync().ConfigureAwait(false);
        }
	}
}

