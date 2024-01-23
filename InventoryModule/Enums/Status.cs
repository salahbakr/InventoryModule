using System.Text.Json.Serialization;

namespace InventoryModule.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        Approved,
        Pending,
        Canceled
    }
}
