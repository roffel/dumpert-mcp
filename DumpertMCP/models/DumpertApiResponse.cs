using System.Text.Json.Serialization;

namespace DumpertMCP.models;

public class DumpertApiResponse
{
    [JsonPropertyName("items")]
    public List<DumpertItem>? Items { get; set; }
}