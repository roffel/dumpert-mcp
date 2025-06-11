namespace DumpertMCP.models;

public class DumpertSummary
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public int CommentCount { get; set; }
    public bool CanComment { get; set; }
    public DateTime ModeratedAt { get; set; }
}