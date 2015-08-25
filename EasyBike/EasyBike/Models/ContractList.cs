using EasyBike.Models.Contracts;
using EasyBike.Models.Contracts.PL;
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

            #region UK
            
            new BarclayBikes{Name = "London",
            ServiceProvider="Barclay",
            ISO31661 = "UK",
            Country = "United Kingdoms"},
            #endregion
        };
    }
}
