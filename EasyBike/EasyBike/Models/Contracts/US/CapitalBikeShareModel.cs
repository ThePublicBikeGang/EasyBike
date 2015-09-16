using System;

namespace EasyBike.Models.Contracts.US
{

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class stations
    {

        private stationsStation[] stationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("station", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public stationsStation[] Stations
        {
            get
            {
                return this.stationField;
            }
            set
            {
                this.stationField = value;
            }
        }

    }
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class stationsStation : StationModelBase
    {
        private int idField;

        private string nameField;

        private double latField;

        private double longField;

        private string lockedField;

        private int nbBikesField;

        private int nbEmptyDocksField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int InternalId
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("name", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }



        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("lat", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double Lat
        {
            get
            {
                return this.latField;
            }
            set
            {
                latField = value;
            }
        }
        public override double Latitude {
            get
            {
                return this.latField;
            }
            set
            {
                latField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("long", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double Long
        {
            get
            {
                return this.longField;
            }
            set
            {
                this.longField = value;
            }
        }
        public override double Longitude
        {
            get
            {
                return this.longField;
            }
            set
            {
                this.longField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string locked
        {
            get
            {
                return this.lockedField;
            }
            set
            {
                this.lockedField = value;
            }
        }

        public override int AvailableBikes
        {
            get
            {
                return this.nbBikesField;
            }
            set
            {
                this.nbBikesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("nbBikes", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int NbBikes
        {
            get
            {
                return this.nbBikesField;
            }
            set
            {
                this.nbBikesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("nbEmptyDocks", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int AvailableDocks
        {
            get
            {
                return this.nbEmptyDocksField;
            }
            set
            {
                this.nbEmptyDocksField = value;
            }
        }

        public override int? AvailableBikeStands
        {
            get
            {
                return nbEmptyDocksField;
            }
            set
            {
            }
        }
    }
}
