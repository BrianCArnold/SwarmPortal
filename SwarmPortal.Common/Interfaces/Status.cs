
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[JsonConverter(typeof(StringEnumConverter))]
public enum Status
{
    Offline = 0,
    Degraded = -1,
    Online = 1,
    Unknown = 99
}