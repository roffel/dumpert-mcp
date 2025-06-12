using System.Text.Json.Serialization;

namespace DumpertMCP.models;

public class DumpertCommentsRoot
{
    [JsonPropertyName("data")]
    public DumpertCommentsData? Data { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    
    [JsonPropertyName("summary")]
    public DumpertSummary? Summary { get; set; }
}