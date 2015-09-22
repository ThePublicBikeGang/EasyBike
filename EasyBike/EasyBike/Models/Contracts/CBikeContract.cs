using EasyBike.Extensions;
using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
                return responseBodyAsText.FromXmlString<BIKEStationData>("").BIKEStation.Cast<StationModelBase>().ToList();
            }
        }

        public override async Task<List<StationModelBase>> InnerRefreshAsync()
        {
            return await InnerGetStationsAsync().ConfigureAwait(false);
        }
    }


    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class BIKEStationData
    {

        private BIKEStationDataStation[] bIKEStationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Station", IsNullable = false)]
        public BIKEStationDataStation[] BIKEStation
        {
            get
            {
                return this.bIKEStationField;
            }
            set
            {
                this.bIKEStationField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BIKEStationDataStation : StationModelBase
    {

        private ushort stationIDField;

        private byte stationNOField;

        private string stationPicField;

        private string stationPic2Field;

        private string stationPic3Field;

        private string stationMapField;

        private string stationNameField;

        private string stationAddressField;

        private double stationLatField;

        private double stationLonField;

        private string stationDescField;

        private int stationNums1Field;

        private int stationNums2Field;

        /// <remarks/>
        public ushort StationID
        {
            get
            {
                return this.stationIDField;
            }
            set
            {
                this.stationIDField = value;
            }
        }

        /// <remarks/>
        public byte StationNO
        {
            get
            {
                return this.stationNOField;
            }
            set
            {
                this.stationNOField = value;
            }
        }

        /// <remarks/>
        public string StationPic
        {
            get
            {
                return this.stationPicField;
            }
            set
            {
                this.stationPicField = value;
            }
        }

        /// <remarks/>
        public string StationPic2
        {
            get
            {
                return this.stationPic2Field;
            }
            set
            {
                this.stationPic2Field = value;
            }
        }

        /// <remarks/>
        public string StationPic3
        {
            get
            {
                return this.stationPic3Field;
            }
            set
            {
                this.stationPic3Field = value;
            }
        }

        /// <remarks/>
        public string StationMap
        {
            get
            {
                return this.stationMapField;
            }
            set
            {
                this.stationMapField = value;
            }
        }

        /// <remarks/>
        public string StationName
        {
            get
            {
                return this.stationNameField;
            }
            set
            {
                this.stationNameField = value;
            }
        }

        /// <remarks/>
        public string StationAddress
        {
            get
            {
                return this.stationAddressField;
            }
            set
            {
                this.stationAddressField = value;
            }
        }

        /// <remarks/>
        public double StationLat
        {
            get
            {
                return this.stationLatField;
            }
            set
            {
                this.stationLatField = value;
            }
        }

        /// <remarks/>
        public double StationLon
        {
            get
            {
                return this.stationLonField;
            }
            set
            {
                this.stationLonField = value;
            }
        }

        /// <remarks/>
        public string StationDesc
        {
            get
            {
                return this.stationDescField;
            }
            set
            {
                this.stationDescField = value;
            }
        }

        /// <remarks/>
        public int StationNums1
        {
            get
            {
                return this.stationNums1Field;
            }
            set
            {
                this.stationNums1Field = value;
            }
        }

        /// <remarks/>
        public int StationNums2
        {
            get
            {
                return this.stationNums2Field;
            }
            set
            {
                this.stationNums2Field = value;
            }
        }

        public override int AvailableBikes { get { return stationNums1Field; } set { } }

        public override int? AvailableBikeStands { get { return stationNums2Field; } set { } }

        public override double Latitude { get { return stationLatField; } set { } }

        public override double Longitude { get { return stationLonField; } set { } }
    }
}
