using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace EasyBike.Models.Contracts
{
	public class OpenSourceBikeShareModel : StationModelBase
	{
		[JsonProperty(PropertyName = "standId")]
		public override string Id { get; set; }

		[JsonProperty(PropertyName = "available_bike_stands")]
		public override int? AvailableBikeStands { get; set; }

		[JsonProperty(PropertyName = "bikecount")]
		public override int? AvailableBikes { get; set; }

		[JsonProperty(PropertyName = "lat")]
		public override double Latitude { get; set; }

		[JsonProperty(PropertyName = "lon")]
		public override double Longitude { get; set; }

		/*[JsonProperty(PropertyName = "banking")]
		public override bool Banking { get; set; }*/

		[JsonProperty(PropertyName = "standDescription")]
		public Position Description { get; set; }

		[JsonProperty(PropertyName = "standName")]
		public Position Name { get; set; }

		[JsonProperty(PropertyName = "standPhoto")]
		public Position Photo { get; set; }
	}
}

