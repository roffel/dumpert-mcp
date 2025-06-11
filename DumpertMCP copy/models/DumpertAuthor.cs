using System.Text.Json.Serialization;

namespace DumpertMCP.models;

public class DumpertAuthor
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }= string.Empty;
    [JsonPropertyName("active")]
    public bool Active { get; set; }
    [JsonPropertyName("newbie")]
    public bool Newbie { get; set; }
    [JsonPropertyName("banned")]
    public bool Banned { get; set; }
    [JsonPropertyName("shadow_banned")]
    public bool ShadowBanned { get; set; }
    [JsonPropertyName("premium")]
    public bool Premium { get; set; }
    [JsonPropertyName("registered_at")]
    public DateTime RegisteredAt { get; set; }
    [JsonPropertyName("age")]
    public int Age { get; set; }
    [JsonPropertyName("gender")]
    public string Gender { get; set; }= string.Empty;
    [JsonPropertyName("commentary_end_ban_at")]
    public DumpertCommentaryEndBanAt? CommentaryEndBanAt { get; set; }
    [JsonPropertyName("commentary_state")]
    public string CommentaryState { get; set; } = string.Empty;
}