namespace EasyBike.Droid.Models
{
    public class PlaceApiModel
    {
        public Prediction[] predictions { get; set; }
        public string status { get; set; }
    }

    public class Prediction
    {
        public string description { get; set; }
        public string id { get; set; }
        public Matched_Substrings[] matched_substrings { get; set; }
        public string place_id { get; set; }
        public string reference { get; set; }
        public Term[] terms { get; set; }
        public string[] types { get; set; }

        public override string ToString()
        {
            return description;
        }
    }

    public class Matched_Substrings
    {
        public int length { get; set; }
        public int offset { get; set; }
    }

    public class Term
    {
        public int offset { get; set; }
        public string value { get; set; }
    }
}