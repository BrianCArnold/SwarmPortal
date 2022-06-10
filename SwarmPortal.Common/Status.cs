using System.Text.Json.Serialization;

[JsonConverterAttribute(typeof(JsonStringEnumConverter))]
public enum Status
{
    Online,
    Offline
}