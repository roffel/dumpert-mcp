using System.Text.Json.Serialization;

namespace DumpertMCP.models;

public class DumpertCommentsRoot
{
    [JsonPropertyName("authors")]
    public List<DumpertAuthor> Authors { get; set; } = [];
    [JsonPropertyName("comments")]
    public List<DumpertComment> Comments { get; set; } = [];
    [JsonPropertyName("summary")]
    public DumpertSummary? Summary { get; set; }
}