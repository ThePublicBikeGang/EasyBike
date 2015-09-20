using EasyBike.Extensions;
using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EasyBike.Models.Contracts
{
    public class CBikeContract : Contract
    {
        public CBikeContract()
        {
            ServiceProvider = "City Bike";
            StationsUrl = "http://www.c-bike.com.tw/xml/stationlistopendata.aspx";
        }

        public override async Task<List<StationModelBase>> InnerGetStationsAsync()
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(StationsUrl + "?" + Guid.NewGuid().ToString()))).ConfigureAwait(false);
                var responseBodyAsText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return responseBodyAsText.FromXmlString<BIKEStationData>("").Items.Cast<StationModelBase>().ToList();
            }
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            return await InnerGetStationsAsync().ConfigureAwait(false);
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class BIKEStationData
    {

        [System.Xml.Serialization.XmlElementAttribute("BIKEStation", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public BIKEStationDataBIKEStation[] Items
        {
            get;
            set;
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class BIKEStationDataBIKEStation
    {

        [System.Xml.Serialization.XmlElementAttribute("Station", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public BIKEStationDataBIKEStationStation[] Stations
        {
            get;
            set;
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class BIKEStationDataBIKEStationStation : StationModelBase
    {
        [System.Xml.Serialization.XmlElementAttribute("StationID", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int InnerId { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("StationName", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("StationLat", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public override double Latitude { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("StationLon", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public override double Longitude { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("StationNums1", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public override int AvailableBikes { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("StationNums2", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public override int? AvailableBikeStands { get; set; }
    }
}