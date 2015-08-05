using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PublicBikes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBikes.Serialization
{
    public class UnderlyingTypeConverter : JsonConverter
    {
        public override void WriteJson(
            JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {

            // Deserialize into a temporary JObject
            JObject obj = serializer.Deserialize<JObject>(reader);


            var test = obj.GetValue("Type", StringComparison.OrdinalIgnoreCase);

            var result = serializer.Deserialize<JObject>(reader);


            // Populate the ZoneProgramInput object with the contents
            //serializer.Populate(obj.CreateReader(), result);

            // Overwrite the "Value" property with the correct value based on the 
            // "Type" property.
            //result.Value =
            //    obj.GetValue("value", StringComparison.OrdinalIgnoreCase)
            //       .ToObject(result.Type, serializer);

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Contract);
        }

        public override bool CanWrite
        {
            get { return false; }
        }
    }
}