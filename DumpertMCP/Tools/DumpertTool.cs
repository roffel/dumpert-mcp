using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace DumpertMCP.Tools;

[McpServerToolType]
public static class DumpertTool
{
    [McpServerTool, Description("Get a list of 'dagtoppers' or top of the day.")]
    public static async Task<string> GetTopOfTheDay(DumpertService dumpertService)
    {
        var topOfTheDay = await dumpertService.GetTopOfTheDay();
        return JsonSerializer.Serialize(topOfTheDay);
    }

    [McpServerTool, Description("Get the top 5 videos of the given week.")]
    public static async Task<string> GetTopOfTheWeek(DumpertService dumpertService, string yearWeek)
    {
        var result = await dumpertService.GetTopOfTheWeek(yearWeek);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get the top 5 videos of the given month.")]
    public static async Task<string> GetTopOfTheMonth(DumpertService dumpertService, string yearMonth)
    {
        var result = await dumpertService.GetTopOfTheMonth(yearMonth);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get the latest uploaded videos (paginated).")]
    public static async Task<string> GetLatest(DumpertService dumpertService, int page)
    {
        var result = await dumpertService.GetLatest(page);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get the latest classic videos (paginated).")]
    public static async Task<string> GetClassics(DumpertService dumpertService, int page)
    {
        var result = await dumpertService.GetClassics(page);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get related videos for a given DumpertID.")]
    public static async Task<string> GetRelated(DumpertService dumpertService, string dumpertId)
    {
        var result = await dumpertService.GetRelated(dumpertId);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Rate a video up or down.")]
    public static async Task<string> Rate(DumpertService dumpertService, string dumpertId, string upDown)
    {
        var result = await dumpertService.Rate(dumpertId, upDown);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get info for a video by DumpertID.")]
    public static async Task<string> GetInfo(DumpertService dumpertService, string dumpertId)
    {
        var result = await dumpertService.GetInfo(dumpertId);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Search for videos by string (paginated).")]
    public static async Task<string> Search(DumpertService dumpertService, string searchString, int page)
    {
        var result = await dumpertService.Search(searchString, page);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get the latest Dumpert TV videos.")]
    public static async Task<string> GetDumpertTv(DumpertService dumpertService)
    {
        var result = await dumpertService.GetDumpertTv();
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get the latest trending (hotshiz) videos.")]
    public static async Task<string> GetHotshiz(DumpertService dumpertService)
    {
        var result = await dumpertService.GetHotshiz();
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get all comments for an article (DumpertID).")]
    public static async Task<string> GetCommentsForArticle(DumpertService dumpertService, string dumpertId, int includeItems)
    {
        var result = await dumpertService.GetCommentsForArticle(dumpertId, includeItems);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get a comment by CommentID.")]
    public static async Task<string> GetCommentById(DumpertService dumpertService, string commentId)
    {
        var result = await dumpertService.GetCommentById(commentId);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get all soundboard data.")]
    public static async Task<string> GetSoundboard(DumpertService dumpertService)
    {
        var result = await dumpertService.GetSoundboard();
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get all videomixer assets.")]
    public static async Task<string> GetVideomixer(DumpertService dumpertService)
    {
        var result = await dumpertService.GetVideomixer();
        return JsonSerializer.Serialize(result);
    }
}
