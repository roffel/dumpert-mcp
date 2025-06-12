using System.Text.Json.Serialization;

namespace DumpertMCP.models;

public class DumpertCommentsData
{
    [JsonPropertyName("comments")]
    public List<DumpertComment> Comments { get; set; } = [];
}