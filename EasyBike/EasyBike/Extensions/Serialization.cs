using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace EasyBike.Extensions
{
    public static class Serialization
    {
        public static T FromXmlString<T>(this string xmlString, string defaultNamespaceName)
        {
            using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
                return (mem.FromXmlStream<T>(defaultNamespaceName));
        }

        public static T FromXmlStream<T>(this Stream xmlStream, string defaultNamespaceName)
        {
            var c = new XmlSerializer(typeof(T), new XmlRootAttribute(defaultNamespaceName));
            var parsedObject = c.Deserialize(xmlStream);
            return ((T)parsedObject);
        }

        public static T FromJsonString<T>(this string jsonString, CultureInfo culture = null)
        {
            if (culture != null)
            {
                return JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings() { Culture = new System.Globalization.CultureInfo("fr-FR") });
            }
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
