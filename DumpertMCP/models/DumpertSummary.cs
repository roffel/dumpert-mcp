using System.Text.Json.Serialization;

namespace DumpertMCP.models;

public class DumpertSummary
{
    [JsonPropertyName("can_comment")]
    public bool CanComment { get; set; }

    [JsonPropertyName("comment_count")]
    public int CommentCount { get; set; }

    [JsonPropertyName("get_rate_limit")]
    public string GetRateLimit { get; set; } = string.Empty;

    [JsonPropertyName("moderated_at")]
    public DateTime ModeratedAt { get; set; }
}
