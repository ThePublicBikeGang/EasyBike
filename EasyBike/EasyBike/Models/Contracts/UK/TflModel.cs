
namespace EasyBike.Models.Contracts.UK
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class stations
    {

        private stationsStation[] stationField;

        private string lastUpdateField;

        private string versionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("station", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public stationsStation[] station
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string lastUpdate
        {
            get
            {
                return this.lastUpdateField;
            }
            set
            {
                this.lastUpdateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class stationsStation : StationModelBase
    {

        private int idField;

        private string nameField;

        private string terminalNameField;

        private double latField;

        private double longField;

        private string installedField;

        private string lockedField;

        private string installDateField;

        private string removalDateField;

        private string temporaryField;

        private int nbBikesField;

        private int nbEmptyDocksField;

        private int nbDocksField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int id
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string terminalName
        {
            get
            {
                return this.terminalNameField;
            }
            set
            {
                this.terminalNameField = value;
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string installed
        {
            get
            {
                return this.installedField;
            }
            set
            {
                this.installedField = value;
            }
        }



        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string installDate
        {
            get
            {
                return this.installDateField;
            }
            set
            {
                this.installDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string removalDate
        {
            get
            {
                return this.removalDateField;
            }
            set
            {
                this.removalDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string temporary
        {
            get
            {
                return this.temporaryField;
            }
            set
            {
                this.temporaryField = value;
            }
        }



        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int nbDocks
        {
            get
            {
                return this.nbDocksField;
            }
            set
            {
                this.nbDocksField = value;
            }
        }

        public double lat
        {
            get
            {
                return this.latField;
            }
            set
            {
                this.latField = value;
            }
        }

        public override double Latitude
        {
            get
            {
                return this.latField;
            }
            set
            {
                this.latField = value;
            }
        }

        public double @long
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

        public int nbBikes
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int nbEmptyDocks
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
                return this.nbEmptyDocksField;
            }
            set
            {
                this.nbEmptyDocksField = value.HasValue ? value.Value : 0;
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
        public override bool Status { get { return lockedField == "false" ? true : false; }
            set { }
        }

        public override bool Banking { get; set; }
    }

}
//        <id>1</id>
//<name>River Street , Clerkenwell</name>
//<terminalName>001023</terminalName>
//<lat>51.52916347</lat>
//<long>-0.109970527</long>
//<installed>true</installed>
//<locked>false</locked>
//<installDate>1278947280000</installDate>
//<removalDate/>
//<temporary>false</temporary>
//<nbBikes>5</nbBikes>
//<nbEmptyDocks>13</nbEmptyDocks>
//<nbDocks>19</nbDocks>
