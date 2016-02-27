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
			AvailabilityUrl = "http://whitebikes.info/command.php?action=map:markers";
			StationRefreshGranularity = true;// TODO À quoi ça sert ?
		}

		public override async Task<List<StationModelBase>> InnerGetStationsAsync()
		{
			return await InnerRefreshAsync();
		}

		public override async Task<StationModelBase> InnerRefreshAsync(Station station)
		{
			using (var client = new HttpClient(new NativeMessageHandler()))
			{
				HttpResponseMessage response = await client.GetAsync(new Uri(AvailabilityUrl)).ConfigureAwait(false);
				var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
				return responseBodyAsText.FromJsonString<OpenSourceBikeShareModel>();
			}
		}

		public override async Task<List<StationModelBase>> InnerRefreshAsync()
		{
			using (var client = new HttpClient(new NativeMessageHandler()))
			{
				HttpResponseMessage response = await client.GetAsync(new Uri(StationsUrl)).ConfigureAwait(false);
				var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
				return responseBodyAsText.FromJsonString<OpenSourceBikeShareModel[]>().ToList<StationModelBase>();
			}
		}
	}
}

