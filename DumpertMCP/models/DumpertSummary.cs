namespace DumpertMCP.models;

public class DumpertSummary
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Link { get; set; }
    public int CommentCount { get; set; }
    public bool CanComment { get; set; }
    public DateTime ModeratedAt { get; set; }
}