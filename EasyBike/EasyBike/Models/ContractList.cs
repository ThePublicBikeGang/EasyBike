using EasyBike.Models.Contracts;
using EasyBike.Models.Contracts.ES;
using EasyBike.Models.Contracts.FR;
using EasyBike.Models.Contracts.PL;
using EasyBike.Models.Contracts.UK;
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
              #region AE
          new NextBikeContract{Name = "Al Sharjah",
               ISO31661 = "AE",
               Country = "United Arab Emirates", Id= "233"},
               new NextBikeContract{Name = "Dubai",
               ISO31661 = "AE",
               Country = "United Arab Emirates", Id= "219"},
            #endregion

               
          #region AT
            // Ended ?
          //new NextBikeContract{Name = "Haag",
          //     ISO31661 = "AT",
          //     Country = "Austria", Id= "167"},
               new NextBikeContract{Name = "Hollabrunn",
               ISO31661 = "AT",
               Country = "Austria", Id= "212"},
               new NextBikeContract{Name = "Innsbruck",
               ISO31661 = "AT",
               Country = "Austria", Id= "199"},
               new NextBikeContract{Name = "Krems Ost",
               ISO31661 = "AT",
               Country = "Austria", Id= "164"},
               new NextBikeContract{Name = "Laa an der Thaya",
               ISO31661 = "AT",
               Country = "Austria", Id= "184"},
               new NextBikeContract{Name = "Marchfeld",
               ISO31661 = "AT",
               Country = "Austria", Id= "170"},
               new NextBikeContract{Name = "Mistelbach",
               ISO31661 = "AT",
               Country = "Austria", Id= "169"},
               new NextBikeContract{Name = "Mödling",
               ISO31661 = "AT",
               Country = "Austria", Id= "64"},
               new NextBikeContract{Name = "Neunkirchen",
               ISO31661 = "AT",
               Country = "Austria", Id= "163"},
               new NextBikeContract{Name = "NeusiedlerSee",
               ISO31661 = "AT",
               Country = "Austria", Id= "23"},
               new NextBikeContract{Name = "Oberes Ybbstal",
               ISO31661 = "AT",
               Country = "Austria", Id= "181"},
               new NextBikeContract{Name = "ÖBB-Bahnhöfe",
               ISO31661 = "AT",
               Country = "Austria", Id= "150"},
               new NextBikeContract{Name = "Piestingtal",
               ISO31661 = "AT",
               Country = "Austria", Id= "168"},
               new NextBikeContract{Name = "Römerland",
               ISO31661 = "AT",
               Country = "Austria", Id= "149"},
               new NextBikeContract{Name = "Sankt Pölten",
               TechnicalName= "St.Pölten",
               ISO31661 = "AT",
               Country = "Austria", Id= "57"},
               new NextBikeContract{Name = "Südheide",
               ISO31661 = "AT",
               Country = "Austria", Id= "185"},
               new NextBikeContract{Name = "Triestingtal",
               ISO31661 = "AT",
               Country = "Austria", Id= "144"},
               new NextBikeContract{Name = "Tulln an der Donau",
               TechnicalName= "Tulln",
               ISO31661 = "AT",
               Country = "Austria", Id= "143"},
               new NextBikeContract{Name = "Tullnerfeld West",
               ISO31661 = "AT",
               Country = "Austria", Id= "196"},
               new NextBikeContract{Name = "Thermenregion",
               ISO31661 = "AT",
               Country = "Austria", Id= "146"},
               new NextBikeContract{Name = "Traisen-Gölsental",
               ISO31661 = "AT",
               Country = "Austria", Id= "180"},
               new NextBikeContract{Name = "Unteres Traisental",
               ISO31661 = "AT",
               Country = "Austria", Id= "165"},
               new NextBikeContract{Name = "Wachau",
               ISO31661 = "AT",
               Country = "Austria", Id= "142"},
               new NextBikeContract{Name = "10 vor Wien",
               TechnicalName= "10vorWien",
               ISO31661 = "AT",
               Country = "Austria", Id= "174"},
               new NextBikeContract{Name = "WienerWald",
               ISO31661 = "AT",
               Country = "Austria", Id= "213"},
               new NextBikeContract{Name = "Wiener Neustadt",
               TechnicalName= "Wr.Neustadt",
               ISO31661 = "AT",
               Country = "Austria", Id= "156"},
               new NextBikeContract{Name = "Wieselburg",
               ISO31661 = "AT",
               Country = "Austria", Id= "151"},
          #endregion

                 #region AZ
               // Ended
          //new NextBikeContract{Name = "Baku",
          //     ISO31661 = "AZ",
          //     Country = "Azerbaijan", Id= "205"},
          #endregion

              #region BE
          new JcDecauxContract{Name = "Bruxelles-Capitale",
               ISO31661 = "BE",
               Country = "Belgium"},
               new JcDecauxContract{Name = "Namur",
               ISO31661 = "BE",
               Country = "Belgium"},
          #endregion

          

               #region BG
          new NextBikeContract{Name = "Dobrich",
               ISO31661 = "BG",
               Country = "Bulgaria", Id= "215"},
          #endregion

            #region CH
                  new NextBikeContract{Name = "Luzern",
            ISO31661 = "CH",
            Country = "Switzerland", Id= "126"},

                    new NextBikeContract{Name = "Sursee",
            ISO31661 = "CH",
            Country = "Switzerland", Id= "88"},
#endregion


            #region CL
            new BCycleContract{Name = "Santiago",
            ServiceProvider= "Bikesantiago, B-cycle",
            ISO31661 = "CL",
            Country = "Chile", Id= "68"},
            #endregion


                #region CY
          new NextBikeContract{Name = "Limassol",
               ISO31661 = "CY",
               Country = "Cyprus", Id= "190"},
          #endregion
            #region DE
            
          new NextBikeContract{Name = "Augsburg",
               ISO31661 = "DE",
               Country = "Germany", Id= "178"},
               new NextBikeContract{Name = "Berlin",
               ISO31661 = "DE",
               Country = "Germany", Id= "20"},
               new NextBikeContract{Name = "Bielefeld",
               ISO31661 = "DE",
               Country = "Germany", Id= "16"},
               new NextBikeContract{Name = "Bietigheim-Bissingen",
               ServiceProvider = "E-Bike Station, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "226"},
               new NextBikeContract{Name = "Bochum",
               ServiceProvider = "metropolradruhr, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "130"},
               new NextBikeContract{Name = "Bottrop",
               ServiceProvider = "metropolradruhr, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "131"},
               new NextBikeContract{Name = "Burghausen",
               ISO31661 = "DE",
               Country = "Germany", Id= "201"},
               new NextBikeContract{Name = "Dortmund",
               ServiceProvider = "metropolradruhr, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "129"},
               new NextBikeContract{Name = "Duisburg",
               ServiceProvider = "metropolradruhr, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "132"},
               new NextBikeContract{Name = "Düsseldorf",
               ISO31661 = "DE",
               Country = "Germany", Id= "50"},
               new NextBikeContract{Name = "Dresden",
               ServiceProvider = "SZ-bike, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "2"},
               new NextBikeContract{Name = "Essen",
               ServiceProvider = "metropolradruhr, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "133"},
               new NextBikeContract{Name = "Flensburg",
               ISO31661 = "DE",
               Country = "Germany", Id= "147"},
               new NextBikeContract{Name = "Frankfurt",
               ISO31661 = "DE",
               Country = "Germany", Id= "8"},
               new NextBikeContract{Name = "Gelsenkirchen",
               ServiceProvider = "metropolradruhr, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "134"},
               new NextBikeContract{Name = "Gütersloh",
               ISO31661 = "DE",
               Country = "Germany", Id= "160"},
               new NextBikeContract{Name = "Hannover",
               ISO31661 = "DE",
               Country = "Germany", Id= "87"},
               new NextBikeContract{Name = "Hamburg",
               ISO31661 = "DE",
               Country = "Germany", Id= "43"},
               new NextBikeContract{Name = "Hamm",
               ServiceProvider = "metropolradruhr, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "135"},
               new NextBikeContract{Name = "Herne",
               ServiceProvider = "metropolradruhr, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "136"},
               new NextBikeContract{Name = "Karlsruhe",
               ServiceProvider = "Fächerrad, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "21"},
               new NextBikeContract{Name = "Leipzig",
               ISO31661 = "DE",
               Country = "Germany", Id= "1"},
               new NextBikeContract{Name = "Magdeburg",
               ISO31661 = "DE",
               Country = "Germany", Id= "42"},
               new NextBikeContract{Name = "Mannheim",
               ISO31661 = "DE",
               Country = "Germany", Id= "195"},
               new NextBikeContract{Name = "Mülheim an der Ruhr",
               ServiceProvider = "metropolradruhr, NextBike",
               TechnicalName= "Mülheim a.d.R.",
               ISO31661 = "DE",
               Country = "Germany", Id= "137"},
               new NextBikeContract{Name = "München",
               ISO31661 = "DE",
               Country = "Germany", Id= "139"},
               new NextBikeContract{Name = "Norderstedt",
               ISO31661 = "DE",
               Country = "Germany", Id= "177"},
               new NextBikeContract{Name = "Nürnberg",
               ServiceProvider = "NorisBike, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "6"},
               new NextBikeContract{Name = "Oberhausen",
               ServiceProvider = "metropolradruhr, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "138"},
               new NextBikeContract{Name = "Offenbach am Main",
               ISO31661 = "DE",
               Country = "Germany", Id= "32"},
               new NextBikeContract{Name = "Offenburg",
               ISO31661 = "DE",
               Country = "Germany", Id= "155"},
               new NextBikeContract{Name = "Postdam",
               ISO31661 = "DE",
               Country = "Germany", Id= "158"},
               new NextBikeContract{Name = "Quickborn",
               ISO31661 = "DE",
               Country = "Germany", Id= "256"},
               new NextBikeContract{Name = "Schwieberdingen",
               ServiceProvider = "E-Bike Station, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "253"},
               new NextBikeContract{Name = "Tübingen",
               ISO31661 = "DE",
               Country = "Germany", Id= "101"},
               new NextBikeContract{Name = "Usedom", //8 Stations côté Polonais Uznam,PL
               ServiceProvider = "UsedomRad, NextBike",
               ISO31661 = "DE",
               Country = "Germany", Id= "176"},
#endregion

            #region ES
            new BicimadContract{Name = "Madrid",
            ISO31661 = "ES",
            Country = "Spain"},
              new JcDecauxContract{Name = "Santander",
               ISO31661 = "ES",
               Country = "Spain"},
               new JcDecauxContract{Name = "Seville",
               ISO31661 = "ES",
               Country = "Spain"},
               new JcDecauxContract{Name = "Valence",
               ISO31661 = "ES",
               Country = "Spain"},


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
               new JcDecauxContract{Name = "Cergy-Pontoise",
               ServiceProvider="VélO2, JCDecaux",
               ISO31661 = "FR",
               Country = "France"},
               new JcDecauxContract{Name = "Creteil",
               ServiceProvider="Cristolib', JCDecaux",
               ISO31661 = "FR",
               Country = "France"},
                new JcDecauxContract{Name = "Lyon",
               ServiceProvider="Vélo'V, JCDecaux",
               ISO31661 = "FR",
               Country = "France"},
               new JcDecauxContract{Name = "Marseille",
               ServiceProvider="Le vélo, JCDecaux",
               ISO31661 = "FR",
               Country = "France"},
               new JcDecauxContract{Name = "Mulhouse",
               ServiceProvider="Vélocité, JCDecaux",
               ISO31661 = "FR",
               Country = "France"},
               new JcDecauxContract{Name = "Nancy",
               ServiceProvider="VélOstan', JCDecaux",
               ISO31661 = "FR",
               Country = "France"},
               new JcDecauxContract{Name = "Nantes",
               ServiceProvider="Bicloo', JCDecaux",
               ISO31661 = "FR",
               Country = "France"},
               new JcDecauxContract{Name = "Paris",
               ServiceProvider="Vélib', JCDecaux",
               ISO31661 = "FR",
               Country = "France"},
               new JcDecauxContract{Name = "Rouen",
               ServiceProvider="Cy'clic', JCDecaux",
               ISO31661 = "FR",
               Country = "France"},
                new JcDecauxContract{Name = "Toulouse",
               ServiceProvider="VélôToulouse', SMTC, JCDecaux", // SMTC = Syndicat Mixte des Transports en Commun
               ISO31661 = "FR",
               Country = "France"},

                 new SmooveContract{Name = "Grenoble",
               ServiceProvider="Métrovélo, SMTC, Smoove", // SMTC = Syndicat Mixte des Transports en Commun
                StationsUrl = "http://vms.metrovelo.fr/vcstations.xml",
               ISO31661 = "FR",
               Country = "France"},
               new SmooveContract{Name = "Avignon",
               ServiceProvider="Vélopop', Smoove",
                StationsUrl = "http://www.velopop.fr/vcstations.xml", // 1 station sans la et lg
               ISO31661 = "FR",
               Country = "France"},
               new SmooveContract{Name = "Belfort",
               ServiceProvider="Optymo, SMTC, Smoove",
                StationsUrl = "http://cli-velo-belfort.gir.fr/vcstations.xml", // 2 Station sans id, la, lg et une qui n'est pas indiqué sur la carte du site
               ISO31661 = "FR",
               Country = "France"},
               new SmooveContract{Name = "Chalon-sur-Saône",
               ServiceProvider="Réflex, Transdev, Smoove", // http://en.wikipedia.org/wiki/Transdev
                StationsUrl = " http://www.reflex-grandchalon.fr/vcstations.xml",
               ISO31661 = "FR",
               Country = "France"},
               new SmooveContract{Name = "Clermont-Ferrand",
               ServiceProvider="C.Vélo, SMTC, Smoove",
                StationsUrl = "http://www.c-velo.fr/vcstations.xml",
               ISO31661 = "FR",
               Country = "France"},
               new SmooveContract{Name = "Lorient",
               ServiceProvider="Vélo an oriant, Smoove",
                StationsUrl = "http://www.lorient-velo.fr/vcstations.xml",
               ISO31661 = "FR",
               Country = "France"},
               new SmooveContract{Name = "Montpellier",
               ServiceProvider="Vélomagg', Smoove",
                StationsUrl = "http://cli-velo-montpellier.gir.fr/vcstations.xml",
               ISO31661 = "FR",
               Country = "France"},
               new SmooveContract{Name = "Saint-Étienne",
               ServiceProvider="Vélivert, Smoove",
                StationsUrl = "http://www.velivert.fr/vcstations.xml",
               ISO31661 = "FR",
               Country = "France"},
               new SmooveContract{Name = "Valence",
               ServiceProvider="Libélo, Transdev, Smoove",
                StationsUrl = "http://www.velo-libelo.fr/vcstations.xml",
               ISO31661 = "FR",
               Country = "France"},

          new SmooveContract{Name = "Strasbourg",
               ServiceProvider="Vélhop', Smoove",
                StationsUrl = "http://www.velhop.strasbourg.eu/vcstations.xml",
               ISO31661 = "FR",
               Country = "France"},

              new VeloPlusContract{Name = "Orléans",
               ISO31661 = "FR",
               Country = "France"},

            #endregion

            #region HR
          new NextBikeContract{Name = "Šibenik",
              ISO31661 = "HR",
               Country = "Croatia", Id= "248"},
               new NextBikeContract{Name = "Zagreb",
              ISO31661 = "HR",
               Country = "Croatia", Id= "220"},
          #endregion

            #region HU
          new NextBikeContract{Name = "Budapest",
               ISO31661 = "HU",
               ServiceProvider= "Bubi, NextBike",
               StationsUrl = "https://nextbike.net/maps/nextbike-live.xml?&domains=mb",
               Country = "Hungary"},
          #endregion

           #region JP
          new JcDecauxContract{Name = "Toyama",
               ISO31661 = "JP",
               Country = "Japan"},
          #endregion

           #region LT
          new JcDecauxContract{Name = "Vilnius",
               ISO31661 = "LT",
               Country = "Lithuania"},
          #endregion

          #region LU
          new JcDecauxContract{Name = "Luxembourg",
               ISO31661 = "LU",
               Country = "Luxembourg"},
          #endregion




               #region LV
          new NextBikeContract{Name = "Jurmala",
               ISO31661 = "LV",
               Country = "Latvia", Id= "140"},
               new NextBikeContract{Name = "Riga",
               ISO31661 = "LV",
               Country = "Latvia", Id= "128"},
          #endregion

          #region NO
          new JcDecauxContract{Name = "Lillestrom",
               ISO31661 = "NO",
               Country = "Norway"},
          #endregion
            
          #region NZ
          new NextBikeContract{Name = "Auckland",
               ISO31661 = "NZ",
               Country = "New Zealand", Id= "34"},
               new NextBikeContract{Name = "Christchurch",
               ISO31661 = "NZ",
               Country = "New Zealand", Id= "193"},
          #endregion

            #region PL
            // les stations peuvent etre aussi récup depuis https://www.bikes-srm.pl/LocationsMap.aspx dans la variable js : var mapDataLocations
            new SzczecinContract{Name = "Szczecin",
            ServiceProvider = "Bike_S, BikeU, Smoove",
            ISO31661 = "PL",
            Country = "Poland"},
               new NextBikeContract{Name = "Bemowo",
               ISO31661 = "PL",
               Country = "Poland", Id= "197"},
               new NextBikeContract{Name = "Białystok",
               ISO31661 = "PL",
               Country = "Poland", Id= "245"},
               new NextBikeContract{Name = "Grodzisk Mazowiecki",
               ServiceProvider = "Grodziski Rower Miejski, NextBike",
               ISO31661 = "PL",
               Country = "Poland", Id= "255"},
               new NextBikeContract{Name = "Konstancin Jeziorna",
               ISO31661 = "PL",
               Country = "Poland", Id= "247"},
               new NextBikeContract{Name = "Kraków",
               ISO31661 = "PL",
               Country = "Poland", Id= "232"},
               new NextBikeContract{Name = "Lublin",
               ServiceProvider = "Lubelski Rower Miejski, NextBike",
               ISO31661 = "PL",
               Country = "Poland", Id= "251"},
               new NextBikeContract{Name = "Opole",
               ISO31661 = "PL",
               Country = "Poland", Id= "202"},
               new NextBikeContract{Name = "Poznan",
               ISO31661 = "PL",
               Country = "Poland", Id= "192"},
               new NextBikeContract{Name = "Sopot",
               ISO31661 = "PL",
               Country = "Poland", Id= "227"},
               new NextBikeContract{Name = "Warszawa", // Biggest one 199 stations
               ServiceProvider = "Veturilo, NextBike",
               ISO31661 = "PL",
               Country = "Poland", Id= "210"},
               new NextBikeContract{Name = "Wrocław",
               ISO31661 = "PL",
               Country = "Poland", Id= "148"},
               new NextBikeContract{Name = "Wroclaw", // Other Stations near & in
               TechnicalName= "WROCŁAW 61",
               ISO31661 = "PL",
               Country = "Poland", Id= "187"},

            #endregion


                   #region RU
          new JcDecauxContract{Name = "Kazan",
               ISO31661 = "RU",
               Country = "Russia"},
          #endregion

          #region SE
          new JcDecauxContract{Name = "Goteborg",
               ISO31661 = "SE",
               Country = "Sweden"},
               new JcDecauxContract{Name = "Stockholm",
               ISO31661 = "SE",
               Country = "Sweden"},
          #endregion

          #region SI
          new JcDecauxContract{Name = "Ljubljana",
               ISO31661 = "SI",
               Country = "Slovenia"},
          #endregion

                    #region TR
          new NextBikeContract{Name = "Konya",
               ISO31661 = "TR",
               Country = "Turkey", Id= "183"},
               new NextBikeContract{Name = "Seferihisar",
               ISO31661 = "TR",
               Country = "Turkey", Id= "249"},
          #endregion


            #region UK

                        new NextBikeContract{Name = "Belfast",
               ISO31661 = "GB",
               ServiceProvider = "Coca-Cola Zero Belfast Bikes, NextBike",
               Country = "United Kingdom", Id= "238"},

          new NextBikeContract{Name = "Bath",
               ISO31661 = "GB",
               Country = "United Kingdom", Id= "236"},
               new NextBikeContract{Name = "Glasgow",
               ISO31661 = "GB",
               Country = "United Kingdom", Id= "237"},
          new TflContract{Name = "London",
            ISO31661 = "GB",
            Country = "United Kingdom"},
            #endregion
    
          new NextBikeContract{Name = "Stirling",
               ISO31661 = "GB",
               Country = "United Kingdom", Id= "243"},

            
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


            new NextBikeContract{Name = "Hoboken, NJ",
               ServiceProvider= "Hudson Bike Share, NextBike",
              ISO31661 = "US",
               Country = "United States", Id= "258"},


            #endregion 

        };
    }
}
