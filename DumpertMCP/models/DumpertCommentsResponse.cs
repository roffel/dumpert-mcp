using System.Text.Json.Serialization;

namespace DumpertMCP.models;

public class DumpertCommentsResponse
{
    [JsonPropertyName("authors")]
    public List<DumpertAuthor> Authors { get; set; } = [];

    [JsonPropertyName("comments")]
    public List<DumpertComment> Comments { get; set; } = [];

    [JsonPropertyName("summary")]
    public DumpertCommentSummary Summary { get; set; } = new();
}


