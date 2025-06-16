using System.Text.Json.Serialization;

namespace DumpertMCP.models;

public class DumpertSingleCommentRoot
{
    [JsonPropertyName("data")]
    public DumpertCommentData? Data { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    
    [JsonPropertyName("summary")]
    public DumpertSummary? Summary { get; set; }
}