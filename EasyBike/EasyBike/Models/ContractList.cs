using EasyBike.Models.Contracts;
using EasyBike.Models.Contracts.PL;
using EasyBike.Models.Contracts.US;
using System.Collections.Generic;

namespace EasyBike.Models
{
    public class ContractList
    {
        public ContractList()
        {

        }

        public List<Contract> Contracts
        {
            get { return contracts; }
            private set { }
        }

        private List<Contract> contracts = new List<Contract>()
        {
            #region CL
            new BCycleContract{Name = "Santiago",
            ServiceProvider= "Bikesantiago, B-cycle",
            ISO31661 = "CL",
            Country = "Chile", Id= "68"},
            #endregion

            #region FR
            new JcDecauxContract{Name = "Amiens",
            ServiceProvider="Vélam', JCDecaux",
            ISO31661 = "FR",
            Country = "France"},
            new JcDecauxContract{Name = "Besancon",
            ServiceProvider="VéloCité, JCDecaux",
            ISO31661 = "FR",
            Country = "France"},
            new JcDecauxContract{Name = "Paris",
            ServiceProvider="Vélib', JCDecaux",
            ISO31661 = "FR",
            Country = "France"},
            #endregion

            #region PL
            // les stations peuvent etre aussi récup depuis https://www.bikes-srm.pl/LocationsMap.aspx dans la variable js : var mapDataLocations
            new SzczecinContract{Name = "Szczecin",
            ServiceProvider = "Bike_S, BikeU, Smoove",
            ISO31661 = "PL",
            Country = "Poland"},
            #endregion

            #region US

            new BCycleContract{Name = "Ann Arbor, MI",
            ServiceProvider= "ArborBike, B-cycle",
            ISO31661 = "US",
            Country = "United States", Id= "76"},
            new BCycleContract{Name = "Austin, TX",
            ISO31661 = "US",
            Country= "United States", Id= "72"},
            new BCycleContract{Name = "Battle Creek, MI",
            ISO31661 = "US",
            Country= "United States", Id= "71"},
            new BCycleContract{Name = "Boulder, CO",
            ISO31661 = "US",
            Country= "United States", Id= "54"},
            new BCycleContract{Name = "Broward County, FL",
            ISO31661 = "US",
            Country= "United States", Id= "53"},
            new BCycleContract{Name = "Charlotte, NC",
            ISO31661 = "US",
            Country= "United States", Id= "61"},
            new BCycleContract{Name = "Cincinnati, OH",
            ServiceProvider= "Red Bike, B-cycle",
            ISO31661 = "US",
            Country= "United States", Id= "80"},
            new BCycleContract{Name = "Columbia County, GA",
            ISO31661 = "US",
            Country= "United States", Id= "74"},
            new BCycleContract{Name = "Milwaukee, WI",
            ServiceProvider= "Bublr Bikes, B-cycle",
            ISO31661 = "US",
            Country= "United States", Id= "70"},
            new BCycleContract{Name = "Dallas Fair Park, TX",
            ISO31661 = "US",
            Country= "United States", Id= "82"},
            new BCycleContract{Name = "Denver, CO",
            ISO31661 = "US",
            Country= "United States", Id= "36"},
            new BCycleContract{Name = "Des Moines, IA",
            ISO31661 = "US",
            Country= "United States", Id= "45"},
            new BCycleContract{Name = "Denver Federal Center, CO",
            ISO31661 = "US",
            Country= "United States", Id= "60"},
            new BCycleContract{Name = "Fargo, ND",
            ISO31661 = "US",
            Country= "United States", Id= "81"},
            new BCycleContract{Name = "Fort Worth, TX",
            ISO31661 = "US",
            Country= "United States", Id= "67"},
            new BCycleContract{Name = "Salt Lake City, UT",
            ServiceProvider = "GREENbike, B-cycle",
            ISO31661 = "US",
            Country= "United States", Id= "66"},
            new BCycleContract{Name = "Greenville, SC",
            ISO31661 = "US",
            Country= "United States", Id= "65"},
            new BCycleContract{Name = "South San Francisco, CA",
            ServiceProvider= "gRide, B-cycle",
            ISO31661 = "US",
            Country= "United States", Id= "47"},
            new BCycleContract{Name = "Kailua, Honolulu County, HI",
            ISO31661 = "US",
            Country= "United States", Id= "49"},
            new BCycleContract{Name = "Houston, TX",
            ISO31661 = "US",
            Country= "United States", Id= "59"},
            new BCycleContract{Name = "Indianapolis, IN",
            ServiceProvider= "Indianna Pacers Bikeshare, B-cycle",
            ISO31661 = "US",
            Country= "United States", Id= "75"},
            new BCycleContract{Name = "Kansas City, MO",
            ISO31661 = "US",
            Country= "United States", Id= "62"},
            new BCycleContract{Name = "Madison, WI",
            ISO31661 = "US",
            Country= "United States", Id= "55"},
            new BCycleContract{Name = "Nashville, TN",
            ISO31661 = "US",
            Country= "United States", Id= "64"},
            new BCycleContract{Name = "Omaha, NE",
            ServiceProvider= "Heartland, B-cycle",
            ISO31661 = "US",
            Country= "United States", Id= "56"},
            new BCycleContract{Name = "Rapid City, SD",
            ISO31661 = "US",
            Country= "United States", Id= "79"},
            new BCycleContract{Name = "San Antonio, TX",
            ISO31661 = "US",
            Country= "United States", Id= "48"},
            new BCycleContract{Name = "Savannah, GA",
            ServiceProvider= "CAT Bike, B-cycle",
            ISO31661 = "US",
            Country= "United States", Id= "73"},
            new BCycleContract{Name = "Spartanburg, SC",
            ISO31661 = "US",
            Country= "United States", Id= "57"},
            new BCycleContract{Name = "Whippany, NJ",
            ISO31661 = "US",
            Country= "United States", Id= "77"},

            #endregion 

            #region UK

            new BarclayBikes{Name = "London",
            ServiceProvider="Barclay",
            ISO31661 = "UK",
            Country = "United Kingdoms"},
            #endregion
        };
    }
}
