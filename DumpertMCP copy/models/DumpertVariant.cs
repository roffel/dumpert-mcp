using System.Text.Json.Serialization;

namespace DumpertMCP.models;

public class DumpertVariant
{
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }
}