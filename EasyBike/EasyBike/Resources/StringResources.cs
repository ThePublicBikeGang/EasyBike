
namespace EasyBike.Resources
{
    public static class StringResources
    {
        public static string FormatShareMessage()
        {
            var body = "Do you know EasyBike? Have a try!";
            body += "\r\nAndroid: " + StringResources.AndroidStoreURL;
            body += "\r\n\r\nWindows Phone 10: " + StringResources.WindowsPhone10StoreURL;
            body += "\r\n\r\nWindows Phone 8.1: " + StringResources.WindowsPhone81StoreURL;
            body += "\r\n\r\nIPhone: " + StringResources.IPhoneStoreURL;
            return body;
        }

        public static string WebSiteForStoresURLs = "https://github.com/ThePublicBikeGang/EasyBike/wiki/Store-links";
        public static string AndroidStoreURL = "https://play.google.com/store/apps/details?id=com.easybikeapp";
        public static string WindowsPhone10StoreURL = "https://www.microsoft.com/store/apps/9wzdncrdkng9";
        public static string WindowsPhone81StoreURL = "http://windowsphone.com/s?appid=191ef96d-e185-47d1-80a3-377ebfefa325";
        public static string IPhoneStoreURL = "Coming soon...";
    }
}
