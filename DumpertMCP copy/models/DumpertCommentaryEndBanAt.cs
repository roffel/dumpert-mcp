using System.Text.Json.Serialization;

namespace DumpertMCP.models;

public class DumpertCommentaryEndBanAt
{
    [JsonPropertyName("Time")]
    public DateTime Time { get; set; }
    [JsonPropertyName("Valid")]
    public bool Valid { get; set; }
}