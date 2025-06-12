namespace DumpertMCP.models;

using System.Text.Json.Serialization;

public class DumpertComment
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("approved")]
    public bool Approved { get; set; }

    [JsonPropertyName("article_id")]
    public long ArticleId { get; set; }

    [JsonPropertyName("article_link")]
    public string ArticleLink { get; set; } = string.Empty;

    [JsonPropertyName("article_title")]
    public string ArticleTitle { get; set; } = string.Empty;

    [JsonPropertyName("author_is_newbie")]
    public bool AuthorIsNewbie { get; set; }

    [JsonPropertyName("author_username")]
    public string AuthorUsername { get; set; } = string.Empty;

    [JsonPropertyName("banned")]
    public bool Banned { get; set; }

    [JsonPropertyName("child_comments")]
    public List<DumpertComment> ChildComments { get; set; } = [];

    [JsonPropertyName("creation_datetime")]
    public DateTime CreationDatetime { get; set; }

    [JsonPropertyName("display_content")]
    public string DisplayContent { get; set; } = string.Empty;

    [JsonPropertyName("html_markup")]
    public string HtmlMarkup { get; set; } = string.Empty;

    [JsonPropertyName("is_author_premium_visible")]
    public bool IsAuthorPremiumVisible { get; set; }

    [JsonPropertyName("kudos_count")]
    public int KudosCount { get; set; }

    [JsonPropertyName("parent_id")]
    public int ParentId { get; set; }

    [JsonPropertyName("reference_id")]
    public int ReferenceId { get; set; }

    [JsonPropertyName("report_count")]
    public int ReportCount { get; set; }
}
