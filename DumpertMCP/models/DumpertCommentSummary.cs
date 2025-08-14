using System.Text.Json.Serialization;

namespace DumpertMCP.models;

public class DumpertCommentSummary
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("link")]
    public string Link { get; set; } = string.Empty;

    [JsonPropertyName("comment_count")]
    public int CommentCount { get; set; }

    [JsonPropertyName("can_comment")]
    public bool CanComment { get; set; }

    [JsonPropertyName("moderated_at")]
    public DateTime ModeratedAt { get; set; }
}


