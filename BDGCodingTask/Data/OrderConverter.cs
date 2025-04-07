using BDGCodingTask.Domain.Entities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BDGCodingTask.Data
{
    class OrderConverter : JsonConverter<List<Order>>
    {

        public override List<Order> ReadJson(JsonReader reader, Type objectType, List<Order> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            return array
                .Select(item => item["Order"].ToObject<Order>(serializer))
                .ToList();
        }

        public override void WriteJson(JsonWriter writer, List<Order> value, JsonSerializer serializer)
        {
            JArray.FromObject(value).WriteTo(writer);
        }
    }
}
