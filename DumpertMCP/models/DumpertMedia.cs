using System.Text.Json.Serialization;

namespace DumpertMCP.models;

public class DumpertMedia
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("mediatype")]
    public string? Mediatype { get; set; }

    [JsonPropertyName("variants")]
    public List<DumpertVariant>? Variants { get; set; }
}