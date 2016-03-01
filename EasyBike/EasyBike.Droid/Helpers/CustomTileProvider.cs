using System;
using Android.Gms.Maps.Model;
using Java.Net;

namespace EasyBike.Droid.Helpers
{
    public class CustomUrlTileProvider : UrlTileProvider
    {

        private string baseUrl;

        public CustomUrlTileProvider(int width, int height, String url) : base(width, height)
        {
            this.baseUrl = url;
        }

        public override URL GetTileUrl(int x, int y, int zoom)
        {
            try
            {
                return new URL(baseUrl.Replace("{zoomlevel}", "" + zoom).Replace("{x}", "" + x)
                        .Replace("{y}", "" + y));
            }
            catch (MalformedURLException e)
            {

            }
            return null;
        }
    }
}