﻿using EasyBike.Models.Contracts;
using EasyBike.Models.Contracts.CH;
using EasyBike.Models.Contracts.CN;
using EasyBike.Models.Contracts.DE;
using EasyBike.Models.Contracts.ES;
using EasyBike.Models.Contracts.FR;
using EasyBike.Models.Contracts.GR;
using EasyBike.Models.Contracts.PL;
using EasyBike.Models.Contracts.UK;
using EasyBike.Models.Contracts.US;
using System.Collections.Generic;

namespace EasyBike.Models
{
    public static class ContractList
    {
        public static List<Country> Countries
        {
            get
            {
                return countries;
            }

            set
            {
                countries = value;
            }
        }

        private static List<Country> countries = new List<Country>()
        {
            #region Australia
            new Country
            {
                Name = "Australia",
                ISO31661 = "AU",
                Contracts = new List<Contract>()
                {
                   new BixxiContract {Name = "Melbourne", ServiceProvider="Melbourne Bike Share, Alta Bicycle Share, Bixi"},
                }
            },            
            #endregion

            #region Austria
            new Country
            {
                Name = "Austria",
                ISO31661 = "AT",
                Contracts = new List<Contract>()
                {
                // Todo inform that it is not working
                //new NextBikeContract{Name = "Haag", Id= "167"},
               new NextBikeContract{Name = "Hollabrunn", Id= "212"},
               new NextBikeContract{Name = "Innsbruck", Id= "199"},
               new NextBikeContract{Name = "Krems Ost", Id= "164"},
               new NextBikeContract{Name = "Laa an der Thaya", Id= "184"},
               new NextBikeContract{Name = "Marchfeld", Id= "170"},
                 // Todo inform that it is not working
               //new NextBikeContract{Name = "Mistelbach", Id= "169"},
               new NextBikeContract{Name = "Mödling", Id= "64"},

               // Todo inform that it is not working
               // new NextBikeContract{Name = "Neunkirchen", Id= "163"},
               new NextBikeContract{Name = "NeusiedlerSee", Id= "23"},
               new NextBikeContract{Name = "Oberes Ybbstal", Id= "181"},
               new NextBikeContract{Name = "ÖBB-Bahnhöfe", Id= "150"},
               new NextBikeContract{Name = "Piestingtal", Id= "168"},
               new NextBikeContract{Name = "Römerland", Id= "149"},
               new NextBikeContract{Name = "Sankt Pölten", TechnicalName= "St.Pölten", Id= "57"},
               new NextBikeContract{Name = "Serfaus", Id= "286"},
               new NextBikeContract{Name = "Südheide", Id= "185"},
               new NextBikeContract{Name = "Triestingtal", Id= "144"},
               new NextBikeContract{Name = "Tulln an der Donau", TechnicalName= "Tulln", Id= "143"},
               new NextBikeContract{Name = "Tullnerfeld West", Id= "196"},
               new NextBikeContract{Name = "Thermenregion", Id= "146"},
               new NextBikeContract{Name = "Traisen-Gölsental", Id= "180"},
               new NextBikeContract{Name = "Unteres Traisental", Id= "165"},
               new NextBikeContract{Name = "Wachau", Id= "142"},
               new NextBikeContract{Name = "10 vor Wien", TechnicalName= "10vorWien", Id= "174"},
               new NextBikeContract{Name = "WienerWald", Id= "213"},
               new NextBikeContract{Name = "Wiener Neustadt", TechnicalName= "Wr.Neustadt", Id= "156"},

               // Todo inform that it is not working
               // new NextBikeContract{Name = "Wieselburg", Id= "151"},
                }
            },            
            #endregion
         
            //#region AZ
            //new Country
            //{
            //    Name = "Azerbaijan",
            //    ISO31661 = "AZ",
            //    Contracts = new List<Contract>()
            //    {
            //    // Todo inform that it is not working
            //    //new NextBikeContract{Name = "Baku", Id= "205"},
            //    }
            //},            
            //#endregion

            #region Belguim
            new Country
            {
                Name = "Belguim",
                ISO31661 = "BE",
                Contracts = new List<Contract>()
                {
                    new JcDecauxContract{Name = "Bruxelles-Capitale" },
                    new JcDecauxContract{Name = "Namur" },
                }
            },            
            #endregion

            #region Bulgaria
            new Country
            {
                Name = "Bulgaria",
                ISO31661 = "BG",
                Contracts = new List<Contract>()
                {
                    new NextBikeContract{Name = "Dobrich", Id= "215"},
                }
            },            
            #endregion

            #region Canada
            new Country
            {
                Name = "Canada",
                ISO31661 = "CA",
                Contracts = new List<Contract>()
                {
                    new CapitalBikeShareContract{Name = "Montréal", StationsUrl = "https://montreal.bixi.com/data/bikeStations.xml", ServiceProvider = "Bixi Montreal, Bixi"},
                    new DivyBikeContract{Name = "Toronto, ON", StationsUrl = "http://www.bikesharetoronto.com/stations/json", ServiceProvider = "Bike Share Toronto, Alta Bicycle Share, Bixi"},
                }
            },            
            #endregion
    
            #region Chile
            new Country
            {
                Name = "Chile",
                ISO31661 = "CL",
                Contracts = new List<Contract>()
                {
                    new BCycleContract{Name = "Santiago",ServiceProvider= "Bikesantiago, B-cycle",Id= "68"}
                }
            },            
            #endregion

            #region China
            new Country
            {
                Name = "China",
                ISO31661 = "CN",
                Contracts = new List<Contract>()
                {
                    // liste des cartes chinoises : http://www.publicbike.net/en/c/param-qual.aspx?param=17
                    new PublicBicycleContract{Name = "Anqiu", StationsUrl="http://218.93.33.59:85/map/wfmap/aqibikestation.asp", AvailabilityUrl = "http://218.93.33.59:85/map/wfmap/ibikegif.asp?id={0}&flag={1}"},
                    new PublicBicycleContract{Name = "Bin Zhou", StationsUrl="http://map.crsud.cn/bz/map/ibikestation.asp", AvailabilityUrl = "http://map.crsud.cn/bz/map/ibikegif.asp?id={0}&flag={1}"},

                    // down
                    //new PublicBicycleContract{Name = "Changshu", StationsUrl="http://www.csbike01.com/csmap/ibikestation.asp", AvailabilityUrl = "http://www.csbike01.com/csmap/ibikestation.asp?id={0}&flag={1}"},


                    /// http://www.bike0555.com/index.asp    
                    new DangtuContract{Name = "Dangtu", AvailabilityUrl = "http://218.93.33.59:85/map/maanshanmap/ibikegif.asp?id={0}&flag={1}"},
                    new PublicBicycleContract{Name = "Fuyang", StationsUrl="http://218.93.33.59:85/map/fuyangmap/ibikestation.asp", AvailabilityUrl = "http://218.93.33.59:85/map/fuyangmap/ibikegif.asp?id={0}&flag={1}"},
                    new PublicBicycleContract{Name = "Guilin", StationsUrl="http://218.93.33.59:85/map/guilinmap/ibikestation.asp", AvailabilityUrl = "http://218.93.33.59:85/map/guilinmap/ibikegif.asp?id={0}&flag={1}"},
                  ////////  // ws.uibike.com	/map.php?location=127.5347550,50.2511620&city=%E9%BB%91%E6%B2%B3%E5%B8%82	4,185		text/html;charset=utf-8	chrome:6420			GET	

                  //////////ApiUrl = "http://www.heihebike.com/hhmap/ibikestation.asp",
                  //////////http://ws.uibike.com/wx.station.php?myloc=127.5347550,50.2511620&e=1&k=74f609d5ae49cefb0c99a90ea6326a5b&d=2

                    new PublicBicycle2Contract{Name = "Heihe", StationsUrl="http://www.heihebike.com/hhmap/ibikestation.asp", AvailabilityUrl = "http://www.heihebike.com/hhmap/ibikegif.asp?id={0}&flag={1}"},
                    new PublicBicycle2Contract{Name = "HeZe", StationsUrl="http://map.crsud.cn/hz/map/ibikestation.asp"},
                    new PublicBicycleContract{Name = "Huaian", StationsUrl = "http://218.93.33.59:85/map/huaianmap/ibikestation.asp", AvailabilityUrl = "http://218.93.33.59:85/map/huaianmap/ibikegif.asp?id={0}&flag={1}"},
                    new PublicBicycleContract{Name = "Huaibei", StationsUrl = "http://218.93.33.59:85/map/suiximap/ibikestation.asp", AvailabilityUrl = "http://218.93.33.59:85/map/suiximap/ibikegif.asp?id={0}&flag={1}"},
                    new HuiminOperateContract{Name = "Huizhou", StationsUrl = "http://hz.2773456.com/zdfb/sz_station.php"},
                    new HuiminOperateContract{Name = "Huizhou (Zhong Kai district)", StationsUrl = "http://zk.2773456.com/zdfb/sz_station.php"},
                    new HuiminOperateContract{Name = "Huizhou (Huiyang district)", StationsUrl = "http://hy.2773456.com/zdfb/sz_station.php"},
                    new HuiminOperateContract{Name = "Huizhou (Longgang district)", StationsUrl = "http://sz.2773456.com/zdfb/sz_station.php"},
                    new HuiminOperateContract{Name = "Huizhou (Luohu district)", StationsUrl = "http://www.lhggzxc.com/zdfb/sz_station.php"},
                    new PublicBicycle2Contract{Name = "Longwan", StationsUrl = "http://218.93.33.59:85/map/wzmap/ibikestation.asp"},
                    ///ApiUrl = "http://ws.uibike.com/wx.station.php?myloc=116.3480570,39.7324840&e=1&k=74f609d5ae49cefb0c99a90ea6326a5b&d=2",
                    new PublicBicycle2Contract{Name = "Daxing", StationsUrl = "http://www.1km0g.com/api/ibikeJSInterface.asp"},
                    new PublicBicycleContract{Name = "Siyang", StationsUrl = "http://218.93.33.59:85/map/siyangmap/ibikestation.asp", AvailabilityUrl = "http://218.93.33.59:85/map/siyangmap/ibikegif.asp?id={0}&flag={1}"},
                    new PublicBicycleContract{Name = "Suzhou", StationsUrl = "http://218.93.33.59:85/map/szmap/ibikestation.asp", AvailabilityUrl = "http://218.93.33.59:85/map/szmap/ibikegif.asp?id={0}&flag={1}"},
                    new PublicBicycleContract{Name = "Taizhou", StationsUrl = "http://www.zjtzpb.com/tzmap/ibikestation.asp", AvailabilityUrl = "http://www.zjtzpb.com/tzmap/ibikegif.asp?id={0}&flag={1}"},
                    new ShanghaiContract{Name = "Shanghai and districts", ServiceProvider="Shangai Forever Bicycle Rental (no availability)"},
                    new PublicBicycleContract{Name = "Weifang", StationsUrl = "http://218.93.33.59:85/map/wfmap/ibikestation.asp", AvailabilityUrl = "http://218.93.33.59:85/map/wfmap/ibikegif.asp?id={0}&flag={1}"},
                    new PublicBicycleContract{Name = "Shenmu", StationsUrl = "http://www.bike912.com/smmap/ibikestation.asp",AvailabilityUrl = "http://www.bike912.com/smmap/ibikegif.asp?id={0}&flag={1}"},
                    new PublicBicycleContract{Name = "Yangzhong", StationsUrl ="http://218.93.33.59:85/map/zjmap/ibikestation.asp",  AvailabilityUrl = "http://218.93.33.59:85/map/zjmap/ibikegif.asp?id={0}&flag={1}"},
                    new PublicBicycleContract{Name = "Yichun", StationsUrl ="http://218.93.33.59:85/map/yichunmap/ibikestation.asp",AvailabilityUrl = "http://218.93.33.59:85/map/yichunmap/ibikegif.asp?id={0}&flag={1}"},
                    new PublicBicycle2Contract{Name = "Zhongshan", StationsUrl="http://www.zsbicycle.com/zsbicycle/zsmap/ibikestation.asp", AvailabilityUrl = "http://www.zsbicycle.com/zsbicycle/zsmap/ibikestation.asp?id={0}&flag={1}"},
                }
            },            
            #endregion

            #region Croatia
            new Country
            {
                Name = "Croatia",
                ISO31661 = "HR",
                Contracts = new List<Contract>()
                {
                    new NextBikeContract{Name = "Gospić",Id= "291"},
                    new NextBikeContract{Name = "Karlovac",Id= "305"},
                    new NextBikeContract{Name = "Šibenik",Id= "248"},
                    new NextBikeContract{Name = "Zagreb",Id= "220"},
                }
            },
            #endregion 

            #region Cyprus
            new Country
            {
                Name = "Cyprus",
                ISO31661 = "CY",
                Contracts = new List<Contract>()
                {
                    new NextBikeContract{Name = "Limassol", Id= "190"}
                }
            },            
            #endregion

            #region Finland
            new Country
            {
                Name = "Finland",
                ISO31661 = "FI",
                Contracts = new List<Contract>()
                {
                    new NextBikeContract{Name = "Helsinki",Id= "311"},
                }
            },
            #endregion 

            #region France
            new Country
            {
                Name = "France",
                ISO31661 = "FR",
                Contracts = new List<Contract>()
                {
                    new JcDecauxContract{Name = "Amiens",ServiceProvider="Vélam', JCDecaux"},
                    new JcDecauxContract{Name = "Besancon",ServiceProvider="VéloCité, JCDecaux"},
                    new VeoliaContract{Name = "Calais", ServiceProvider=" (Vel-in, Veloway Systems, Veolia)", StationsUrl="http://www.vel-in.fr/cartoV2/libProxyCarto.asp"},
                    new JcDecauxContract{Name = "Cergy-Pontoise",ServiceProvider="VélO2, JCDecaux"},
                    new JcDecauxContract{Name = "Creteil",ServiceProvider="Cristolib', JCDecaux"},
                    new JcDecauxContract{Name = "Lyon",ServiceProvider="Vélo'V, JCDecaux"},
                    new JcDecauxContract{Name = "Marseille", ServiceProvider="Le vélo, JCDecaux"},
                    new JcDecauxContract{Name = "Mulhouse", ServiceProvider="Vélocité, JCDecaux"},
                    new JcDecauxContract{Name = "Nancy", ServiceProvider="VélOstan', JCDecaux"},
                    new JcDecauxContract{Name = "Nantes", ServiceProvider="Bicloo', JCDecaux"},
                    new VeoliaContract{Name = "Nice", ServiceProvider=" (Vélobleu, Veloway Systems, Veolia)", StationsUrl="https://www.velobleu.org/cartoV2/libProxyCarto.asp"},
                    new JcDecauxContract{Name = "Paris", ServiceProvider="Vélib', JCDecaux"},
                    new JcDecauxContract{Name = "Rouen", ServiceProvider="Cy'clic', JCDecaux"},
                    // SMTC = Syndicat Mixte des Transports en Commun
                    new JcDecauxContract{Name = "Toulouse", ServiceProvider="VélôToulouse', SMTC, JCDecaux", },
                    new GrenobleContract{Name = "Grenoble", ServiceProvider="Métrovélo, Smoove (No station availability...)"},
                    // 1 station sans la et lg"
                    new SmooveContract{Name = "Avignon", ServiceProvider="Vélopop', Smoove", StationsUrl = "http://www.velopop.fr/vcstations.xml" },
                    // 2 Station sans id, la, lg et une qui n'est pas indiqué sur la carte du site
                    new SmooveContract{Name = "Belfort", ServiceProvider="Optymo, SMTC, Smoove", StationsUrl = "http://cli-velo-belfort.gir.fr/vcstations.xml"},
                    // http://en.wikipedia.org/wiki/Transdev
                    new SmooveContract{Name = "Chalon-sur-Saône",ServiceProvider="Réflex, Transdev, Smoove", StationsUrl = " http://www.reflex-grandchalon.fr/vcstations.xml"},
                    new SmooveContract{Name = "Clermont-Ferrand", ServiceProvider="C.Vélo, SMTC, Smoove", StationsUrl = "http://www.c-velo.fr/vcstations.xml"},
                    new SmooveContract{Name = "Lorient", ServiceProvider="Vélo an oriant, Smoove", StationsUrl = "http://www.lorient-velo.fr/vcstations.xml"},
                    new SmooveContract{Name = "Montpellier", ServiceProvider="Vélomagg', Smoove", StationsUrl = "http://cli-velo-montpellier.gir.fr/vcstations.xml"},
                    new SmooveContract{Name = "Saint-Étienne", ServiceProvider="Vélivert, Smoove", StationsUrl = "http://www.velivert.fr/vcstations.xml"},
                    new SmooveContract{Name = "Valence", ServiceProvider="Libélo, Transdev, Smoove", StationsUrl = "http://www.velo-libelo.fr/vcstations.xml"},
                    new SmooveContract{Name = "Strasbourg", ServiceProvider="Vélhop', Smoove", StationsUrl = "http://www.velhop.strasbourg.eu/vcstations.xml"},
                    // Check this
                    new VeloPlusContract{Name = "Orléans"},

                    new VeoliaContract{Name = "Vannes", ServiceProvider=" (Vélocéa, Veloway Systems, Veolia)", StationsUrl="http://www.velocea.fr/cartoV2/libProxyCarto.asp"},
                }
            },
            #endregion 

            #region Germany
            new Country
            {
                Name = "Germany",
                ISO31661 = "DE",
                Contracts = new List<Contract>()
                {
                    new MVGContract{Name = "Mainz"},
                    // 0 stations...
                    // new CallABikeContract{Name = "Aachen",Id="100006"},
                    new CallABikeContract{Name = "Aschaffenburg",Id="18"},
                    //new CallABikeContract{Name = "Augsburg",       // Ce trouve en plein océan pacifique...
                    //PaysImage = paysImagesRootPath+ "/DE.png",    // DANS LA FUCKING WATER T ENTEND !!!},
                    new CallABikeContract{Name = "Baden-Baden", Id="100039"},
                    new CallABikeContract{Name = "Bamberg",Id="33600"},
                    new CallABikeContract{Name = "Berlin",Id="2"},
                    new CallABikeContract{Name = "Bielefeld",Id="100010"},
                    new CallABikeContract{Name = "Bonn",Id="100016"},
                    new CallABikeContract{Name = "Braunschweig",Id="51"},
                    new CallABikeContract{Name = "Bremen",Id="100004"},
                    new CallABikeContract{Name = "Darmstadt",Id="19"},
                    new CallABikeContract{Name = "Düsseldorf",Id="58"},
                    new CallABikeContract{Name = "Erlangen",Id="100017"},
                    new CallABikeContract{Name = "Frankfurt am Main",Id="13"},
                    new CallABikeContract{Name = "Frankfurt Flughafen",Id="100002"},
                    new CallABikeContract{Name = "Freiburg im Breisgau",Id="2000"},
                    new CallABikeContract{Name = "Fulda",Id="100011"},
                    new CallABikeContract{Name = "Gersthofen",Id="1000071"},
                    new CallABikeContract{Name = "Göttingen",Id="33200"},
                    new CallABikeContract{Name = "Gütersloh",Id="139"},
                    new CallABikeContract{Name = "Halle(Saale)",Id="1"},
                    new CallABikeContract{Name = "Hamburg", ServiceProvider = "StadtRAD Hamburg, Call a Bike (no dock availabilty :/)", Id="75"},
                    new CallABikeContract{Name = "Hanau",Id="141"},
                    new CallABikeContract{Name = "Hannover",Id="2600"},
                    new CallABikeContract{Name = "Heidelberg",Id="34201"},
                    new CallABikeContract{Name = "Hennef(Sieg)",Id="1000139"},
                    new CallABikeContract{Name = "Ingolstadt",Id="28"},
                    new CallABikeContract{Name = "Kaiserslautern",Id="100007"},
                    new CallABikeContract{Name = "Karlsruhe",Id="34000"},
                    new CallABikeContract{Name = "Kassel", ServiceProvider = "Konrad, Call a Bike (no dock availabilty :/)", Id="34100"},
                    //new CallABikeContract{Name = "Kiel",          // Encore une ville sous l'océan
                    //Id="100022",
                    new CallABikeContract{Name = "Köln", Id="33800"},
                    new CallABikeContract{Name = "Lübeck",Id="100023",},
                    new CallABikeContract{Name = "Lüneburg",ServiceProvider = "StadtRAD Lüneburg, Call a Bike (no dock availabilty :/)",Id="165"},
                    new CallABikeContract{Name = "Magdeburg",Id="34300"},
                    new CallABikeContract{Name = "Mainz",Id="21"},
                    new CallABikeContract{Name = "Mannheim",Id="34200"},
                    new CallABikeContract{Name = "Marburg",Id="2500"},
                    new CallABikeContract{Name = "München",Id="90"},
                    new CallABikeContract{Name = "Oberhausen",Id="57"},
                    new CallABikeContract{Name = "Oldenburg(Oldb)",Id="100014"},
                    new CallABikeContract{Name = "Rostock",Id="8"},
                    new CallABikeContract{Name = "Rüsselsheim",Id="299"},
                    new CallABikeContract{Name = "Saarbrücken",Id="100015"},
                    new CallABikeContract{Name = "Stuttgart",Id="33900"},
                    new CallABikeContract{Name = "Troisdorf",Id="30503"},
                    new CallABikeContract{Name = "Warnemünde",Id="247"},
                    new CallABikeContract{Name = "Weimar",Id="10"},
                    new CallABikeContract{Name = "Wiesbaden",Id="24"},
                    new CallABikeContract{Name = "Wolfsburg",Id="100009"},
                    new CallABikeContract{Name = "Würzburg",Id="100012"},
                    new NextBikeContract{Name = "Augsburg",Id= "178"},
                    new NextBikeContract{Name = "Berlin",Id= "20"},
                    new NextBikeContract{Name = "Bielefeld",Id= "16"},
                    new NextBikeContract{Name = "Bietigheim-Bissingen",ServiceProvider = "E-Bike Station, NextBike",Id= "226"},
                    new NextBikeContract{Name = "Bochum",ServiceProvider = "metropolradruhr, NextBike",Id= "130"},
                    new NextBikeContract{Name = "Bottrop",ServiceProvider = "metropolradruhr, NextBike",Id= "131"},
                    // Todo inform that it is not working
                    // new NextBikeContract{Name = "Burghausen",Id= "201"},
                    new NextBikeContract{Name = "Dortmund",ServiceProvider = "metropolradruhr, NextBike",Id= "129"},
                    new NextBikeContract{Name = "Duisburg",ServiceProvider = "metropolradruhr, NextBike",Id= "132"},
                    new NextBikeContract{Name = "Düsseldorf",Id= "50"},
                    new NextBikeContract{Name = "Dresden",ServiceProvider = "SZ-bike, NextBike",Id= "2"},
                    new NextBikeContract{Name = "Essen",ServiceProvider = "metropolradruhr, NextBike",Id= "133"},
                    new NextBikeContract{Name = "Flensburg",Id= "147"},
                    new NextBikeContract{Name = "Frankfurt",Id= "8"},
                    new NextBikeContract{Name = "Friedrichshafen",Id= "24"},
                    new NextBikeContract{Name = "Gelsenkirchen",ServiceProvider = "metropolradruhr, NextBike",Id= "134"},
                    new NextBikeContract{Name = "Gütersloh",Id= "160"},
                    new NextBikeContract{Name = "Hannover",Id= "87"},
                    new NextBikeContract{Name = "Hamburg",Id= "43"},
                    new NextBikeContract{Name = "Hamm",ServiceProvider = "metropolradruhr, NextBike",Id= "135"},
                    new NextBikeContract{Name = "Hanau", Id= "301"},
                    new NextBikeContract{Name = "Heidelberg",Id= "194"},
                    new NextBikeContract{Name = "Herrenberg",Id= "288"},
                    new NextBikeContract{Name = "Herne",ServiceProvider = "metropolradruhr, NextBike",Id= "136"},
                    new NextBikeContract{Name = "Holzgerlingen",Id= "292"},
                    new NextBikeContract{Name = "Karlsruhe",ServiceProvider = "Fächerrad, NextBike",Id= "21"},
                    new NextBikeContract{Name = "Köln",Id= "14"},
                    new NextBikeContract{Name = "Leipzig",Id= "1"},
                    new NextBikeContract{Name = "Ludwigsburg",Id= "290"},
                    new NextBikeContract{Name = "Ludwigshafen",Id= "266"},
                    new NextBikeContract{Name = "Magdeburg",Id= "42"},
                    new NextBikeContract{Name = "Mannheim",Id= "195"},
                    new NextBikeContract{Name = "Mülheim an der Ruhr",ServiceProvider = "metropolradruhr, NextBike",TechnicalName= "Mülheim a.d.R.",Id = "137"},
                    new NextBikeContract{Name = "München",Id= "139"},
                    new NextBikeContract{Name = "Norderstedt",Id= "177"},
                    new NextBikeContract{Name = "Nürnberg",ServiceProvider = "NorisBike, NextBike",Id= "6"},
                    new NextBikeContract{Name = "Oberhausen",ServiceProvider = "metropolradruhr, NextBike",Id= "138"},
                    new NextBikeContract{Name = "Offenbach am Main",Id= "32"},
                    new NextBikeContract{Name = "Offenburg",Id= "155"},
                    new NextBikeContract{Name = "Postdam",Id= "158"},
                    new NextBikeContract{Name = "Quickborn",Id= "256"},
                    new NextBikeContract{Name = "Schwieberdingen",ServiceProvider = "E-Bike Station, NextBike",Id= "253"},
                    new NextBikeContract{Name = "Speyer",Id= "278"},
                    new NextBikeContract{Name = "Tübingen",Id= "101"},
                    //8 Stations côté Polonais Uznam,PL
                    new NextBikeContract{Name = "Usedom",ServiceProvider = "UsedomRad, NextBike",Id= "176"},
                    new NextBikeContract{Name = "Vaihingen an der Enz",Id= "287"},
                    new NextBikeContract{Name = "Waiblingen",Id= "267"},
                    new NextBikeContract{Name = "Würzburg",Id= "281"},
                }
            },            
            #endregion
           
            #region Greece
            new Country
            {
                Name = "Greece",
                ISO31661 = "GR",
                Contracts = new List<Contract>()
                {
                    //Check trello at ToDo maybe a possiblity for make a list of each citie
                    new EasyBikeContract{Name = "All cities (from Easy Bike)"},
                }
            },
            #endregion 
         
            #region Hungary
            new Country
            {
                Name = "Hungary",
                ISO31661 = "HU",
                Contracts = new List<Contract>()
                {
                     new NextBikeContract{Name = "Budapest",ServiceProvider= "Bubi, NextBike",StationsUrl = "https://nextbike.net/maps/nextbike-live.xml?&domains=mb"},
                }
            },
            #endregion 

            #region Japan
            new Country
            {
                Name = "Japan",
                ISO31661 = "JP",
                Contracts = new List<Contract>()
                {
                    new JcDecauxContract{Name = "Toyama"},
                }
            },
            #endregion 

            #region Latvia
            new Country
            {
                Name = "Latvia",
                ISO31661 = "LV",
                Contracts = new List<Contract>()
                {
                    new NextBikeContract{Name = "Jurmala", Id= "140"},
                    new NextBikeContract{Name = "Riga", Id= "128"},
                }
            },
            #endregion 

            #region Lithuania
            new Country
            {
                Name = "Lithuania",
                ISO31661 = "LT",
                Contracts = new List<Contract>()
                {
                    new JcDecauxContract{Name = "Vilnius"},
                }
            },
            #endregion 

            #region Luxembourg
            new Country
            {
                Name = "Luxembourg",
                ISO31661 = "LU",
                Contracts = new List<Contract>()
                {
                    new JcDecauxContract{Name = "Luxembourg"},
                }
            },
            #endregion 

            #region New Zealand
            new Country
            {
                Name = "New Zealand",
                ISO31661 = "NZ",
                Contracts = new List<Contract>()
                {
                    new NextBikeContract{Name = "Auckland", Id= "34"},
                    new NextBikeContract{Name = "Cambridge", Id= "261"},
                    new NextBikeContract{Name = "Christchurch", Id= "193"},
                }
            },
            #endregion   

            #region Norway
            new Country
            {
                Name = "Norway",
                ISO31661 = "NO",
                Contracts = new List<Contract>()
                {
                      new JcDecauxContract{Name = "Lillestrom"},
                }
            },
            #endregion 
       
            #region Poland
            new Country
            {
                Name = "Poland",
                ISO31661 = "PL",
                Contracts = new List<Contract>()
                {
                    // les stations peuvent etre aussi récup depuis https://www.bikes-srm.pl/LocationsMap.aspx dans la variable js : var mapDataLocations
                    new SzczecinContract{Name = "Szczecin", ServiceProvider = "Bike_S, BikeU, Smoove"},
                    // Todo investigate
                    new NextBikeContract{Name = "Bemowo", Id= "197"},
                    new NextBikeContract{Name = "Białystok", Id= "245"},
                    new NextBikeContract{Name = "Grodzisk Mazowiecki", ServiceProvider = "Grodziski Rower Miejski, NextBike", Id= "255"},
                    new NextBikeContract{Name = "Juchnowiec Kościelny", ServiceProvider="ROWER GMINNY Poland, NextBike", Id= "300"},
                    new NextBikeContract{Name = "Konstancin Jeziorna", Id= "247"},
                    new NextBikeContract{Name = "Katowice", Id= "282"},
                    new NextBikeContract{Name = "Kraków", Id= "232"},
                    new NextBikeContract{Name = "Lublin", ServiceProvider = "Lubelski Rower Miejski, NextBike", Id= "251"},
                    new NextBikeContract{Name = "Opole", Id= "202"},
                    new NextBikeContract{Name = "Poznań", Id= "192"},
                    new NextBikeContract{Name = "Sopot", Id= "227"},
                    // Biggest one 199 stations
                    new NextBikeContract{Name = "Warszawa", ServiceProvider = "Veturilo, NextBike", Id= "210"},
                    new NextBikeContract{Name = "Wrocław", Id= "148"},
                }
            },
            #endregion 

            #region Russia
            new Country
            {
                Name = "Russia",
                ISO31661 = "RU",
                Contracts = new List<Contract>()
                {
                     new JcDecauxContract{Name = "Kazan"},
                }
            },
            #endregion 

            #region Saudi Arabia
            new Country
            {
                Name = "Saudi Arabia",
                ISO31661 = "SA",
                Contracts = new List<Contract>()
                {
                    new NextBikeContract{Name = "King Abdullah Economic City", Id= "264"}
                }
            },            
            #endregion

			#region Slovakia
			new Country
            {
                Name = "Slovakia",
                ISO31661 = "SK",
                Contracts = new List<Contract>()
                {
                    new OpenSourceBikeShareContract{Name = "Bratislava"},
                }
            },
			#endregion 

            #region Slovenia
            new Country
            {
                Name = "Slovenia",
                ISO31661 = "SI",
                Contracts = new List<Contract>()
                {
                    new JcDecauxContract{Name = "Ljubljana"},
                }
            },
            #endregion 

            #region Spain
            new Country
            {
                Name = "Spain",
                ISO31661 = "ES",
                Contracts = new List<Contract>()
                {
                    new BarceloneContract{Name = "Barcelona"},
                    new BicimadContract{Name = "Madrid"},
                    new JcDecauxContract{Name = "Santander"},
                    new JcDecauxContract{Name = "Seville"},
                    new JcDecauxContract{Name = "Valence"},
                }
            },            
            #endregion

            #region Sweden
            new Country
            {
                Name = "Sweden",
                ISO31661 = "SE",
                Contracts = new List<Contract>()
                {
                    new JcDecauxContract{Name = "Goteborg"},
                    new JcDecauxContract{Name = "Stockholm"},
                }
            },
            #endregion 

            #region Switzerland
            new Country
            {
                Name = "Switzerland",
                ISO31661 = "CH",
                Contracts = new List<Contract>()
                {
                    new PubliBikeContract{Name = "Aigle",ServiceProvider="Chablais, PubliBike",},
                    new PubliBikeContract{Name = "Avenches",ServiceProvider="Les Lacs-Romont, PubliBike"},
                    new PubliBikeContract{Name = "Basel"},
                    new PubliBikeContract{Name = "Bern"},
                    new PubliBikeContract{Name = "Brig"},
                    new PubliBikeContract{Name = "Bulle"},
                    new PubliBikeContract{Name = "Chavannes-près-Renens", ServiceProvider="Lausanne-Morges, PubliBike"},
                    new PubliBikeContract{Name = "Cheyres", ServiceProvider="Les Lacs-Romont, PubliBike"},
                    new PubliBikeContract{Name = "Delémont"},
                    new PubliBikeContract{Name = "Divonne-les-Bains", ServiceProvider="La Côte, PubliBike" },
                    new PubliBikeContract{Name = "Ecublens", ServiceProvider="Lausanne-Morges, PubliBike", TechnicalName= "Ecublens PL4"},
                    new PubliBikeContract{Name = "Estavayer-le-Lac", ServiceProvider="Les Lacs-Romont, PubliBike"},
                    new PubliBikeContract{Name = "Frauenfeld"},
                    new PubliBikeContract{Name = "Fribourg", ServiceProvider="Agglo Fribourg, PubliBike"},
                    new PubliBikeContract{Name = "Gland", ServiceProvider="La Côte, PubliBike"},
                    new PubliBikeContract{Name = "Granges-Paccot",ServiceProvider="Agglo Fribourg, PubliBike"},
                    new PubliBikeContract{Name = "Kreuzlingen"},
                    new PubliBikeContract{Name = "La Tour-de-Peilz", ServiceProvider="Riviera, PubliBike"},
                    new PubliBikeContract{Name = "La Tour-de-Trême", ServiceProvider="Bulle, PubliBike"},
                    new PubliBikeContract{Name = "Lausanne",ServiceProvider="Lausanne-Morges, PubliBike"},
                    new PubliBikeContract{Name = "Lugano"},
                    new PubliBikeContract{Name = "Luzern"},
                    new PubliBikeContract{Name = "Marly",ServiceProvider="Agglo Fribourg, PubliBike"},
                    new PubliBikeContract{Name = "Melide",ServiceProvider="Lugano, PubliBike"},
                    new PubliBikeContract{Name = "Monthey",ServiceProvider="Chablais, PubliBike"},
                    new PubliBikeContract{Name = "Morcote",ServiceProvider="Lugano, PubliBike"},
                    new PubliBikeContract{Name = "Morges", ServiceProvider="Lausanne-Morges, PubliBike"},
                    new PubliBikeContract{Name = "Murten Morat",ServiceProvider="Les Lacs-Romont, PubliBike",TechnicalName= "Murten/Morat"},
                    new PubliBikeContract{Name = "Nyon",ServiceProvider="La Côte, PubliBike"},
                    new PubliBikeContract{Name = "Payerne",ServiceProvider="Les Lacs-Romont, PubliBike"},
                    new PubliBikeContract{Name = "Pazzallo",ServiceProvider="Lugano, PubliBike"},
                    new PubliBikeContract{Name = "Prangins",ServiceProvider="La Côte, PubliBike"},
                    new PubliBikeContract{Name = "Préverenges",ServiceProvider="Lausanne-Morges, PubliBike"},
                    new PubliBikeContract{Name = "Prilly",ServiceProvider="Lausanne-Morges, PubliBike"},
                    new PubliBikeContract{Name = "Rapperswil"},
                    new PubliBikeContract{Name = "Renens",ServiceProvider="Lausanne-Morges, PubliBike"},
                    new PubliBikeContract{Name = "Romont",ServiceProvider="Les Lacs-Romont, PubliBike"},
                    new PubliBikeContract{Name = "Sion"},
                    // not working atm
                    // new PubliBikeContract{Name = "Solothurn"},
                    new PubliBikeContract{Name = "Tesserete",ServiceProvider="Lugano, PubliBike"},
                    new PubliBikeContract{Name = "Tolochenaz",ServiceProvider="Lausanne-Morges, PubliBike"},
                    new PubliBikeContract{Name = "Vevey",ServiceProvider="Riviera, PubliBike"},
                    new PubliBikeContract{Name = "Villars-sur-Glâne",ServiceProvider="Agglo Fribourg, PubliBike"},
                    // not working atm
                    // new PubliBikeContract{Name = "Winterthur"},
                    new PubliBikeContract{Name = "Yverdon-les-Bains"},
                    new PubliBikeContract{Name = "Zürich"},
                    new NextBikeContract{Name = "Luzern",Id= "126"},
                    new NextBikeContract{Name = "Sursee",Id= "88"},


                }
            },            
            #endregion

            #region Taiwan
            new Country
            {
                Name = "Taiwan",
                ISO31661 = "TW",
                Contracts = new List<Contract>()
                {
                    new CBikeContract{Name = "Kaohsiung"},
                }
            },
            #endregion 

            #region Turkey
            new Country
            {
                Name = "Turkey",
                ISO31661 = "TR",
                Contracts = new List<Contract>()
                {
                    new NextBikeContract{Name = "Konya", Id= "183"},
                    new NextBikeContract{Name = "Seferihisar", Id= "249"},
                }
            },
            #endregion 
        
            #region Ukraine
            new Country
            {
                Name = "Ukraine",
                ISO31661 = "UA",
                Contracts = new List<Contract>()
                {
                    new NextBikeContract{Name = "Lviv", Id= "280"},
                }
            },
            #endregion 

            #region United Arab Emirates
            new Country
            {
                Name = "United Arab Emirates",
                ISO31661 = "AE",
                Contracts = new List<Contract>()
                {
                    new NextBikeContract{Name = "Al Sharjah", Id= "233"},
                    new NextBikeContract{Name = "Dubai", Id= "219"},
                }
            },            
            #endregion

            #region United Kingdom
            new Country
            {
                Name = "United Kingdom",
                ISO31661 = "GB",
                Contracts = new List<Contract>()
                {
                    new NextBikeContract{Name = "Belfast", ServiceProvider = "Coca-Cola Zero Belfast Bikes, NextBike", Id= "238"},
                    new NextBikeContract{Name = "Bath", Id= "236"},
                    new NextBikeContract{Name = "Glasgow", Id= "237"},
                    new TflContract{Name = "London"},
                    // https://www.cycleconnect.co.uk/
                    new OxonbikesContract{Name = "Northampton", ServiceProvider="Cycle CoNNect", StationsUrl="https://www.cycleconnect.co.uk/"},
                    new CitycardCyclesContract{Name = "Nottingham"},
                    new OxonbikesContract{Name = "Oxford"},
                    new NextBikeContract{Name = "Stirling", Id= "243"},
                }
            },
            #endregion 

            #region United States
            new Country
            {
                Name = "United States",
                ISO31661 = "US",
                Contracts = new List<Contract>()
                {
                  
                    //https://secure.niceridemn.org/data2/bikeStations.xml
                    new BixxiMinneapolisContract{Name = "Minneapolis, MN", StationsUrl = "https://secure.niceridemn.org/data2/stations.json",ServiceProvider = "Nice Ride Minnesota, Alta Bicycle Share, Bixi"},
                    new BixxiMinneapolisContract{Name = "Seattle, WA", StationsUrl = "https://secure.prontocycleshare.com/data/stations.json",ServiceProvider = "Pronto Cycle Share, Alta Bicycle Share, Bixi"},
                    new CapitalBikeShareContract{Name = "Boston, MA", StationsUrl= "http://www.thehubway.com/data/stations/bikeStations.xml",ServiceProvider = "Hubway, Alta Bicycle Share, Bixi"},
                    new CapitalBikeShareContract{Name = "Washington, D.C. area", TechnicalName= "Washington", ServiceProvider = "Capital BikeShare, Alta Bicycle Share, Bixi"},

                    new BCycleContract{Name = "Ann Arbor, MI", ServiceProvider= "ArborBike, B-cycle", Id= "76"},
                    new BCycleContract{Name = "Austin, TX", Id= "72"},
                    new BCycleContract{Name = "Battle Creek, MI", Id= "71"},
                    new BCycleContract{Name = "Boulder, CO", Id= "54"},
                    new BCycleContract{Name = "Broward County, FL", Id= "53"},
                    new BCycleContract{Name = "Charlotte, NC", Id= "61"},
                    new DivyBikeContract{Name = "Chattanooga, TM", StationsUrl = "http://www.bikechattanooga.com/stations/json", ServiceProvider = "Bike Chattanooga, Alta Bicycle Share, Bixi"},
                    new DivyBikeContract{Name = "Chicago, IL", TechnicalName= "Chicago", ServiceProvider = "Divvy, Alta Bicycle Share, Bixi"},
                    new BCycleContract{Name = "Cincinnati, OH", ServiceProvider= "Red Bike, B-cycle", Id= "80"},
                    new BCycleContract{Name = "Clarksville, TN", Id= "86"},
                    new BCycleContract{Name = "Columbia County, GA", Id= "74"},
                    new DivyBikeContract{Name = "Columbus, OH", StationsUrl = "http://cogobikeshare.com/stations/json", ServiceProvider = "CoGo Bike Share, Alta Bicycle Share, Bixi"},
                    new BCycleContract{Name = "Dallas Fair Park, TX", Id= "82"},
                    new BCycleContract{Name = "Denver, CO", Id= "36"},
                    new BCycleContract{Name = "Des Moines, IA", Id= "45"},
                    new BCycleContract{Name = "Denver Federal Center, CO", Id= "60"},
                    new BCycleContract{Name = "El Paso, TX", Id= "84"},
                    // https://www.facebook.com/GreatRidesBikeshare
                    new BCycleContract{Name = "Fargo, ND", ServiceProvider="Great Rides Bike Share", Id= "81"},
                    new BCycleContract{Name = "Fort Worth, TX", Id= "67"},
                    // https://www.facebook.com/LinkDayton
                    new BCycleContract{Name = "Dayton, OH, ", ServiceProvider = "Link Dayton Bike Share, B-cycle", Id= "83"},
                    new BCycleContract{Name = "Salt Lake City, UT", ServiceProvider = "GREENbike, B-cycle", Id= "66"},
                    new BCycleContract{Name = "Greenville, SC", Id= "65"},
                    new BCycleContract{Name = "South San Francisco, CA", ServiceProvider= "gRide, B-cycle", Id= "47"},
                    new BCycleContract{Name = "Kailua, Honolulu County, HI", Id= "49"},
                    new BCycleContract{Name = "Houston, TX", Id= "59"},
                    new BCycleContract{Name = "Indianapolis, IN", ServiceProvider= "Indianna Pacers Bikeshare, B-cycle", Id= "75"},
                    new BCycleContract{Name = "Kansas City, MO", Id= "62"},
                    new BCycleContract{Name = "Madison, WI", Id= "55"},
                    new BCycleContract{Name = "Milwaukee, WI", ServiceProvider= "Bublr Bikes, B-cycle", Id= "70"},
                    new BCycleContract{Name = "Nashville, TN", Id= "64"},
                    new DivyBikeContract{Name = "New York City, NY", TechnicalName= "New York", ServiceProvider= "Citi Bike, Alta Bicycle Share, Bixi", StationsUrl= "http://www.citibikenyc.com/stations/json"},
                    new BCycleContract{Name = "Omaha, NE", ServiceProvider= "Heartland, B-cycle", Id= "56"},

                    // Philadelphia have his data as well from the next service // new BCycleContract{Name = "Philadelphia, PA",ServiceProvider= "Philly Indego, B-cycle", Id= "3"},
                    new PhiladelphiaContract{Name = "Philadelphia, PA", ServiceProvider= "Philly Indego, B-cycle", StationsUrl = "https://api.phila.gov/bike-share-stations/v1"},

                    new BCycleContract{Name = "Rapid City, SD", Id= "79"},
                    new BCycleContract{Name = "San Antonio, TX", Id= "48"},
                    new DivyBikeContract{Name = "San Francisco Bay Area, CA", StationsUrl = "http://www.bayareabikeshare.com/stations/json", ServiceProvider = "Bay Area Bike Share, Alta Bicycle Share, Bixi"},
                    new BCycleContract{Name = "Savannah, GA", ServiceProvider= "CAT Bike, B-cycle", Id= "73"},
                    new BCycleContract{Name = "Spartanburg, SC", Id= "57"},
                    new BCycleContract{Name = "Whippany, NJ", Id= "77"},
                    new NextBikeContract{Name = "Hoboken, NJ", ServiceProvider= "Hudson Bike Share, NextBike", Id= "258"},
                    new NextBikeContract{Name = "Kent State University", ServiceProvider= "Flashfleet Kent State University, NextBike", Id= "306"},
                    new NextBikeContract{Name = "Pittsburgh", ServiceProvider= "Healthy Ride Pittsburgh, NextBike", Id= "254"},
                    new NextBikeContract{Name = "West Palm Beach Florida", ServiceProvider= "Skybike West Palm Beach, NextBike", Id= "283"},
                }
            },
            #endregion 
        };
    }
}


