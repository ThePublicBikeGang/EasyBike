using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts.FR
{
    // Info : http://www.metromobilite.fr/pages/OpenData/OpenDataConsignesMV.html#
    // https://twitter.com/metromobilite
    public class GrenobleContract : Contract
    {
        public GrenobleContract()
        {
            StationsUrl = "http://www.metromobilite.fr/data/Carto/Statique/Parkings.geojson";
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString()))).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<Rootobject>(await response.Content.ReadAsStringAsync().ConfigureAwait(false)).features.ToList<StationModelBase>();
            }
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            return await InnerGetStationsAsync();
        }
    }

    public class Rootobject
    {
        public string eacute { get; set; }
        public string type { get; set; }
        public Feature[] features { get; set; }
    }

    public class Feature : StationModelBase
    {
        public string type { get; set; }
        public Properties properties { get; set; }
        public Geometry geometry { get; set; }

        public override int? AvailableBikes { get; set; }

        public override int? AvailableBikeStands { get; set; }

        public override double Latitude { get { return geometry.coordinates[1]; } set { } }

        public override double Longitude { get { return geometry.coordinates[0]; } set { } }
    }

    public class Properties
    {
        public string CODE { get; set; }
        public string LIBELLE { get; set; }
        public string ADRESSE { get; set; }
        public string TYPE { get; set; }
        public int TOTAL { get; set; }
        public string LIGNES { get; set; }
        public int DISPO { get; set; }
        public int NSV_ID { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public float[] coordinates { get; set; }
    }
}


