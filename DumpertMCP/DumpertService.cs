using System.Net.Http.Json;
using DumpertMCP.models;

namespace DumpertMCP;

public class DumpertService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();

    /// <summary>
    /// Fetches the top items (kudotoppers) of the day from the Dumpert API.
    /// </summary>
    /// <returns>A list of Dumpert items representing the kudotoppers of the day.</returns>
    public async Task<List<DumpertItem>> GetTopOfTheDay()
    {
        const string url = "https://api-live.dumpert.nl/mobile_api/json/top5/dag/";
        var result = await FetchData<DumpertApiResponse>(url);
        return result?.Items ?? [];
    }

    /// <summary>
    /// Fetches the top items of the week for a given year-week from the Dumpert API.
    /// </summary>
    /// <param name="yearWeek">The year-week string in the format YYYYWW.</param>
    /// <returns>A list of Dumpert items representing the top videos of the week.</returns>
    public async Task<List<DumpertItem>> GetTopOfTheWeek(string yearWeek)
    {
        var formatted = yearWeek.Replace("-", "");
        var url = $"https://api.dumpert.nl/mobile_api/json/top5/week/{formatted}";
        var result = await FetchData<DumpertApiResponse>(url);
        return result?.Items ?? new List<DumpertItem>();
    }

    /// <summary>
    /// Fetches the top items of the month for a given year-month from the Dumpert API.
    /// </summary>
    /// <param name="yearMonth">The year-month string in the format YYYYMM.</param>
    /// <returns>A list of Dumpert items representing the top items of the month.</returns>
    public async Task<List<DumpertItem>> GetTopOfTheMonth(string yearMonth)
    {
        var formatted = yearMonth.Replace("-", "");
        var url = $"https://api.dumpert.nl/mobile_api/json/top5/maand/{formatted}";
        var result = await FetchData<DumpertApiResponse>(url);
        return result?.Items ?? [];
    }

    /// <summary>
    /// Fetches the latest items from the Dumpert API for a given page number.
    /// </summary>
    /// <param name="page">The page number to fetch.</param>
    /// <returns>A list of Dumpert items representing the latest items.</returns>
    public async Task<List<DumpertItem>> GetLatest(int page)
    {
        var url = $"https://api.dumpert.nl/mobile_api/json/latest/{page}";
        var result = await FetchData<DumpertApiResponse>(url);
        return result?.Items ?? [];
    }

    /// <summary>
    /// Fetches classics from the Dumpert API for a given page number.
    /// </summary>
    /// <param name="page">The page number to fetch.</param>
    /// <returns>A list of Dumpert items representing classic items.</returns>
    public async Task<List<DumpertItem>> GetClassics(int page = 0)
    {
        var url = $"https://api.dumpert.nl/mobile_api/json/classics/{page}";
        var result = await FetchData<DumpertApiResponse>(url);
        return result?.Items ?? new List<DumpertItem>();
    }

    /// <summary>
    /// Fetches related items for a given Dumpert ID from the Dumpert API.
    /// </summary>
    /// <param name="dumpertId">The Dumpert ID of the item.</param>
    /// <returns>A list of Dumpert items representing related items.</returns>
    public async Task<List<DumpertItem>> GetRelated(string dumpertId)
    {
        var url = $"https://api.dumpert.nl/mobile_api/json/related/{dumpertId}";
        var result = await FetchData<DumpertApiResponse>(url);
        return result?.Items ?? [];
    }

    /// <summary>
    /// Rates an item up or down on the Dumpert platform.
    /// </summary>
    /// <param name="dumpertId">The Dumpert ID of the item.</param>
    /// <param name="upDown">The rating direction, either "up" or "down".</param>
    /// <returns>An object indicating the success of the operation and the response content.</returns>
    public async Task<object> Rate(string dumpertId, string upDown)
    {
        var url = $"https://post.dumpert.nl/api/v1.0/rating/{dumpertId}/{upDown}";
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        return new { Success = response.IsSuccessStatusCode, Response = content };
    }

    /// <summary>
    /// Fetches detailed information for a given Dumpert ID from the Dumpert API.
    /// </summary>
    /// <param name="dumpertId">The Dumpert ID of the item.</param>
    /// <returns>A Dumpert item representing the item information.</returns>
    public async Task<DumpertItem?> GetInfo(string dumpertId)
    {
        var url = $"https://api.dumpert.nl/mobile_api/json/info/{dumpertId}";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;
        var dumpertResponse = await response.Content.ReadFromJsonAsync<DumpertApiResponse>();
        return dumpertResponse?.Items?.FirstOrDefault();
    }

    /// <summary>
    /// Searches for item on the Dumpert platform using a search string and page number.
    /// </summary>
    /// <param name="searchString">The search string to use.</param>
    /// <param name="page">The page number to fetch.</param>
    /// <returns>A list of Dumpert items matching the search criteria.</returns>
    public async Task<List<DumpertItem>> Search(string searchString, int page)
    {
        var url = $"https://api.dumpert.nl/mobile_api/json/search/{searchString}/{page}";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return [];
        var dumpertResponse = await response.Content.ReadFromJsonAsync<DumpertApiResponse>();
        return dumpertResponse?.Items ?? [];
    }

    /// <summary>
    /// Fetches Dumpert TV videos from the Dumpert API.
    /// </summary>
    /// <returns>A list of Dumpert items representing Dumpert TV videos.</returns>
    public async Task<List<DumpertItem>> GetDumpertTv()
    {
        const string url = "https://api.dumpert.nl/mobile_api/json/dumperttv";
        var result = await FetchData<DumpertApiResponse>(url);
        return result?.Items ?? [];
    }

    /// <summary>
    /// Fetches trending items (hotshiz) from the Dumpert API.
    /// </summary>
    /// <returns>A list of Dumpert items representing trending items.</returns>
    public async Task<List<DumpertItem>> GetHotshiz()
    {
        const string url = "https://api.dumpert.nl/mobile_api/json/hotshiz";
        var result = await FetchData<DumpertApiResponse>(url);
        return result?.Items ?? [];
    }

    public async Task<T?> FetchData<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Failed to fetch data from {url}: {error}");
            return default;
        }

        try
        {
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing response from {url}: {ex.Message}");
            return default;
        }
    }

    /// <summary>
    /// Fetches comments for a given article (Dumpert ID) from the Dumpert API.
    /// </summary>
    /// <param name="dumpertId">The Dumpert ID of the article.</param>
    /// <param name="includeItems">The number of items to include in the response.</param>
    /// <returns>An object containing the comments response with authors, comments, and summary.</returns>
    public async Task<DumpertCommentsResponse?> GetCommentsForArticle(string dumpertId, int includeItems = 1)
    {
        var url = $"https://comment.dumpert.nl/api/v1.0/articles/{dumpertId.Replace("_","/")}/comments";
        return await FetchData<DumpertCommentsResponse>(url);
    }

    /// <summary>
    /// Fetches a comment by its ID from the Dumpert API.
    /// </summary>
    /// <param name="commentId">The ID of the comment.</param>
    /// <returns>An object containing the success status and response content.</returns>
    public async Task<object?> GetCommentById(string commentId)
    {
        var url = $"https://comments.dumpert.nl/api/v1.0/comments/{commentId}/";
        return await FetchData<DumpertSingleCommentRoot>(url);
    }

    /// <summary>
    /// Fetches soundboard data from the Dumpert API.
    /// </summary>
    /// <returns>A list of soundboard items containing name, URL, thumbnail, and duration.</returns>
    public async Task<List<SoundboardItem>> GetSoundboard()
    {
        const string url = "https://video-snippets.dumpert.nl/soundboard.json";
        var result = await FetchData<List<SoundboardItem>>(url);
        return result ?? [];
    }

    /// <summary>
    /// Fetches videomixer assets from the Dumpert API.
    /// </summary>
    /// <returns>An object containing the success status and response content.</returns>
    public async Task<object> GetVideomixer()
    {
        const string url = "https://video-snippets.dumpert.nl/videomixer.json";
        var result = await FetchData<object>(url);
        return result ?? new { Success = false, Response = "Failed to fetch videomixer assets." };
    }
}