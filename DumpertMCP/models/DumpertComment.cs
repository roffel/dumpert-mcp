using System.Text.Json.Serialization;

namespace DumpertMCP.models;

public class DumpertComment
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("approved")]
    public bool Approved { get; set; }
    [JsonPropertyName("creation_datetime")]
    public DateTime CreationDatetime { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
    [JsonPropertyName("kudos_count")]
    public int KudosCount { get; set; }
    [JsonPropertyName("reference_id")]
    public int ReferenceId { get; set; }
    [JsonPropertyName("author")]
    public int Author { get; set; }
    [JsonPropertyName("child_comments")]
    public List<DumpertComment> ChildComments { get; set; } = [];
}
