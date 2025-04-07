using System.Text.Json.Serialization;

namespace BDGCodingTask.Domain.Enums
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderType
    {
        Buy,
        Sell
    }
}
