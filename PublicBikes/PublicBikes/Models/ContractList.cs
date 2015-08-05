using PublicBikes.Models.Contracts;
using PublicBikes.Models.Contracts.PL;
using System.Collections.Generic;

namespace PublicBikes.Models
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
            Pays = "France"},
            new JcDecauxContract{Name = "Besancon",
            ServiceProvider="VéloCité, JCDecaux",
            Pays = "France"},
            new JcDecauxContract{Name = "Paris",
            ServiceProvider="Vélib', JCDecaux",
            Pays = "France"},
            #endregion

            #region PL
            // les stations peuvent etre aussi récup depuis https://www.bikes-srm.pl/LocationsMap.aspx dans la variable js : var mapDataLocations
            new SzczecinContract{Name = "Szczecin",
            ServiceProvider = "Bike_S, BikeU, Smoove",
            PaysImage = "/PL.png",
            Pays = "Poland"},
            #endregion

            #region UK
            
            new BarclayBikes{Name = "London",
            ServiceProvider="Barclay",
            Pays = "Uk"},
            #endregion
        };
    }
}
