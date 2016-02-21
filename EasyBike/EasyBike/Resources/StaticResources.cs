
using System.Collections.Generic;

namespace EasyBike.Resources
{
    public static class StaticResources
    {
        public static string FormatShareMessage()
        {
            var body = "Do you know EasyBike? Have a try!";
            body += "\r\nAndroid: " + StaticResources.AndroidStoreURL;
            body += "\r\n\r\nWindows Phone 10: " + StaticResources.WindowsPhone10StoreURL;
            body += "\r\n\r\nWindows Phone 8.1: " + StaticResources.WindowsPhone81StoreURL;
            body += "\r\n\r\nIPhone: " + StaticResources.IPhoneStoreURL;
            return body;
        }

        public static string WebSiteForStoresURLs = "https://github.com/ThePublicBikeGang/EasyBike/wiki/Store-links";
        public static string AndroidStoreURL = "https://play.google.com/store/apps/details?id=com.easybikeapp";
        public static string WindowsPhone10StoreURL = "https://www.microsoft.com/store/apps/9wzdncrdkng9";
        public static string WindowsPhone81StoreURL = "http://windowsphone.com/s?appid=191ef96d-e185-47d1-80a3-377ebfefa325";
        public static string IPhoneStoreURL = "Coming soon...";

        public static string TilesMapbox = "http://api.tiles.mapbox.com/v4/opentripplannerlow.jcagk4b3/{z}/{x}/{y}.png256?access_token=pk.eyJ1Ijoib3BlbnRyaXBwbGFubmVybG93IiwiYSI6ImRja1VOUjAifQ.1XBo8j8DGWj2EdIsxXGHdQ";
        public static string TilesMapboxRetina = "http://api.tiles.mapbox.com/v4/opentripplannerhigh.jcah6hla/{z}/{x}/{y}@2x.png256?access_token=pk.eyJ1Ijoib3BlbnRyaXBwbGFubmVyaGlnaCIsImEiOiIyT2xxdVRjIn0.1n9CkukOWpsIgExzdcWfJg";
        public static string TilesLyrk = "http://tiles.lyrk.org/lr/{z}/{x}/{y}?apikey=ea4f307439474ce99e29662232f80885";
        public static string TilesMaquest = "http://otile1.mqcdn.com/tiles/1.0.0/osm/{z}/{x}/{y}.png";
        public static string TilesMapnik = "http://a.tile.openstreetmap.org/{z}/{x}/{y}.png";
        public static string TilesCyclemap = "http://a.tile.opencyclemap.org/cycle/{z}/{x}/{y}.png";

        public static string TilesGoogleMapNormalName = "Google Map Normal";
        public static string TilesGoogleMapSatelliteName = "Google Map Satellite";
        public static string TilesGoogleMapHybridName = "Google Map Hybrid";
        public static string TilesGoogleMapTerrainName = "Google Map Terrain";
        public static string TilesOpenStreetMapName = "Open Street Map";
        public static string TilesOpenCycleMapName = "Open Cycle Map";
        public static string TilesMapQuestMapName = "Map Quest";

        public static LinkedList<TileContainer> TilesList = new LinkedList<TileContainer>();

        public static int TilesMaquestMaxZoom = 18;
        public static int TilesMapnikMaxZoom = 19;
        public static int TilesCyclemapMaxZoom = 18;
        public static int TilesLyrkMaxZoom = 18;

        static StaticResources()
        {
            TilesList.AddLast(new LinkedListNode<TileContainer>(new TileContainer
            {
                Name = TilesGoogleMapNormalName,
                GoogleMapLayer = true
            }));
            TilesList.AddLast(new LinkedListNode<TileContainer>(new TileContainer
            {
                Name = TilesOpenCycleMapName,
                TilesUrl = TilesCyclemap,
                MaxZoom = TilesCyclemapMaxZoom,
            }));
            TilesList.AddLast(new LinkedListNode<TileContainer>(new TileContainer
            {
                Name = TilesOpenStreetMapName,
                TilesUrl = TilesMapnik,
                MaxZoom = TilesMapnikMaxZoom,
            }));
            TilesList.AddLast(new LinkedListNode<TileContainer>(new TileContainer
            {
                Name = TilesGoogleMapHybridName,
                GoogleMapLayer = true
            }));
           
           
            //TilesList.AddLast(new LinkedListNode<TileContainer>(new TileContainer
            //{
            //    Name = TilesMapQuestMapName,
            //    TilesUrl = TilesMaquest,
            //    MaxZoom = TilesMaquestMaxZoom,
            //}));
            //TilesList.AddLast(new LinkedListNode<TileContainer>(new TileContainer
            //{
            //    Name = "Map Box",
            //    TilesUrl = TilesMapbox,
            //    MaxZoom = TilesMaquestMaxZoom,
            //}));
            //TilesList.AddLast(new LinkedListNode<TileContainer>(new TileContainer
            //{
            //    Name = "Map Box Retina",
            //    TilesUrl = TilesMapboxRetina,
            //    MaxZoom = TilesMaquestMaxZoom,
            //}));
        }
    }

    public class TileContainer
    {
        public string Name;
        public string TilesUrl;
        public int MaxZoom;
        public int TileSize = 256;
        public bool GoogleMapLayer = false;
    }


}
