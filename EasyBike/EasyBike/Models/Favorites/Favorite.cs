namespace EasyBike.Models.Favorites
{
    public class Favorite
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool FromStation { get; set; }


        public override string ToString()
        {
            return Name + Latitude;
        }
    }
}

