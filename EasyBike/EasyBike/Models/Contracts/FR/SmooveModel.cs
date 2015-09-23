
using System;

namespace EasyBike.Models.Contracts.FR
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class vcs
    {

        [System.Xml.Serialization.XmlElementAttribute("sl", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public GrenoStation Node
        {
            get;
            set;
        }

    }
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class GrenoStation
    {
        [System.Xml.Serialization.XmlElementAttribute("si", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public station[] Stations
        {
            get;
            set;
        }


    }
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class station : StationModelBase
    {
        private string idField;
        private int nbBikesField;
        private int nbEmptyDocksField;
        public override string Id { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("id", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Id2
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
        [System.Xml.Serialization.XmlAttributeAttribute("to", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int TotalDocks
        {
            get;
            set;
        }

        private string longitudeStr;
        [System.Xml.Serialization.XmlAttributeAttribute("lg", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LongitudeStr
        {
            get
            {
                return longitudeStr;
            }
            set
            {
                longitudeStr = value;
            }
        }

        public override double Longitude { get; set; }

        public override double Latitude { get; set; }

        private string latitudeStr;
        [System.Xml.Serialization.XmlAttributeAttribute("la", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LatitudeStr
        {
            get
            {
                return latitudeStr;
            }
            set
            {
                latitudeStr = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("av", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int NbBikesField
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
        [System.Xml.Serialization.XmlAttributeAttribute("fr", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int NbEmptyDocksField
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

        public override int? AvailableBikes { get { return nbBikesField; } set { } }

        public override int? AvailableBikeStands { get { return NbEmptyDocksField; } set { } }
    }


}
