using System.Text.Json.Serialization;

namespace DumpertMCP.models;

public class DumpertCommentData
{
    [JsonPropertyName("comment")]
    public DumpertComment Comment { get; set; } = new DumpertComment();
}