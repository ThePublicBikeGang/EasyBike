namespace EasyBike.Droid.Models
{
    public class PlaceApiDetailModel
    {
        public object[] html_attributions { get; set; }
        public Result result { get; set; }
        public string status { get; set; }
    }

    public class Result
    {
        public Address_Components[] address_components { get; set; }
        public string adr_address { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string icon { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string place_id { get; set; }
        public string reference { get; set; }
        public string scope { get; set; }
        public string[] types { get; set; }
        public string url { get; set; }
        public string vicinity { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
        public Viewport viewport { get; set; }
    }

    public class Location
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Northeast
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Southwest
    {
        public float lat { get; set; }
        public float lng { get; set; }
    }

    public class Address_Components
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }

}