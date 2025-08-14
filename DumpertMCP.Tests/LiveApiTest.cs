using System.Text.Json;
using DumpertMCP.models;
using Xunit;

namespace DumpertMCP.Tests;

public class LiveApiTest
{
    private readonly HttpClient _httpClient = new();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public async Task LiveApi_GetTopOfTheDay_ParsesCorrectly()
    {
        // Arrange
        var url = "https://api-live.dumpert.nl/mobile_api/json/top5/dag/";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DumpertApiResponse>(content, _jsonOptions);

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}: {content}");
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.True(result.Items!.Count > 0, "Expected at least one item in top of the day");
        
        var firstItem = result.Items[0];
        Assert.False(string.IsNullOrEmpty(firstItem.Id), "Item ID should not be null or empty");
        Assert.False(string.IsNullOrEmpty(firstItem.Title), "Item title should not be null or empty");
        Assert.NotNull(firstItem.Media);
    }

    [Fact]
    public async Task LiveApi_GetLatest_ParsesCorrectly()
    {
        // Arrange
        var url = "https://api.dumpert.nl/mobile_api/json/latest/0";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DumpertApiResponse>(content, _jsonOptions);

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}: {content}");
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.True(result.Items.Count > 0, "Expected at least one item in latest videos");
        
        var firstItem = result.Items[0];
        Assert.False(string.IsNullOrEmpty(firstItem.Id), "Item ID should not be null or empty");
        Assert.False(string.IsNullOrEmpty(firstItem.Title), "Item title should not be null or empty");
        Assert.NotNull(firstItem.Media);
    }

    [Fact]
    public async Task LiveApi_GetDumpertTv_ParsesCorrectly()
    {
        // Arrange
        var url = "https://api.dumpert.nl/mobile_api/json/dumperttv";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DumpertApiResponse>(content, _jsonOptions);

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}: {content}");
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.True(result.Items.Count > 0, "Expected at least one item in Dumpert TV");
        
        var firstItem = result.Items[0];
        Assert.False(string.IsNullOrEmpty(firstItem.Id), "Item ID should not be null or empty");
        Assert.False(string.IsNullOrEmpty(firstItem.Title), "Item title should not be null or empty");
    }

    [Fact]
    public async Task LiveApi_GetHotshiz_ParsesCorrectly()
    {
        // Arrange
        var url = "https://api.dumpert.nl/mobile_api/json/hotshiz";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DumpertApiResponse>(content, _jsonOptions);

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}: {content}");
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.True(result.Items.Count > 0, "Expected at least one item in hotshiz");
        
        var firstItem = result.Items[0];
        Assert.False(string.IsNullOrEmpty(firstItem.Id), "Item ID should not be null or empty");
        Assert.False(string.IsNullOrEmpty(firstItem.Title), "Item title should not be null or empty");
    }

    [Fact]
    public async Task LiveApi_GetCommentsForArticle_ParsesCorrectly()
    {
        // Arrange - Using a working article ID from the real API
        var url = "https://comment.dumpert.nl/api/v1.0/articles/100130000/237d8919/comments";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}: {content}");
        
        // Parse as JsonElement to inspect the actual structure
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(content);
        
        // Check if response has expected top-level fields
        Assert.True(jsonElement.TryGetProperty("authors", out var authorsElement), "Response should have 'authors' field");
        Assert.True(jsonElement.TryGetProperty("comments", out var commentsElement), "Response should have 'comments' field");
        Assert.True(jsonElement.TryGetProperty("summary", out var summaryElement), "Response should have 'summary' field");
        
        // Check if comments is an array
        Assert.Equal(JsonValueKind.Array, commentsElement.ValueKind);
        
        // Check if we have at least one comment
        var commentsArray = commentsElement.EnumerateArray().ToList();
        Assert.True(commentsArray.Count > 0, "Expected at least one comment");
        
        // Inspect first comment structure
        var firstComment = commentsArray[0];
        Assert.True(firstComment.TryGetProperty("id", out var idElement), "Comment should have 'id' field");
        Assert.True(firstComment.TryGetProperty("approved", out var approvedElement), "Comment should have 'approved' field");
        Assert.True(firstComment.TryGetProperty("creation_datetime", out var creationDatetimeElement), "Comment should have 'creation_datetime' field");
        Assert.True(firstComment.TryGetProperty("content", out var contentElement), "Comment should have 'content' field");
        Assert.True(firstComment.TryGetProperty("kudos_count", out var kudosCountElement), "Comment should have 'kudos_count' field");
        Assert.True(firstComment.TryGetProperty("reference_id", out var referenceIdElement), "Comment should have 'reference_id' field");
        Assert.True(firstComment.TryGetProperty("author", out var authorElement), "Comment should have 'author' field");
        
        // Check if authors is an array
        Assert.Equal(JsonValueKind.Array, authorsElement.ValueKind);
        
        // Check summary structure
        Assert.True(summaryElement.TryGetProperty("id", out var summaryIdElement), "Summary should have 'id' field");
        Assert.True(summaryElement.TryGetProperty("title", out var summaryTitleElement), "Summary should have 'title' field");
        Assert.True(summaryElement.TryGetProperty("comment_count", out var commentCountElement), "Summary should have 'comment_count' field");
        Assert.True(summaryElement.TryGetProperty("can_comment", out var canCommentElement), "Summary should have 'can_comment' field");
        Assert.True(summaryElement.TryGetProperty("moderated_at", out var moderatedAtElement), "Summary should have 'moderated_at' field");
    }

    [Fact]
    public async Task LiveApi_GetSoundboard_ParsesCorrectly()
    {
        // Arrange
        var url = "https://video-snippets.dumpert.nl/soundboard.json";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<SoundboardItem>>(content, _jsonOptions);

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}: {content}");
        Assert.NotNull(result);
        Assert.True(result!.Count > 0, "Expected at least one soundboard item");
        
        var firstItem = result[0];
        Assert.False(string.IsNullOrEmpty(firstItem.Name), "Soundboard item name should not be null or empty");
        Assert.False(string.IsNullOrEmpty(firstItem.Url), "Soundboard item URL should not be null or empty");
        Assert.True(firstItem.Duration > 0, "Soundboard item duration should be positive");
    }

    [Fact]
    public async Task LiveApi_GetVideomixer_ParsesCorrectly()
    {
        // Arrange
        var url = "https://video-snippets.dumpert.nl/videomixer.json";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}: {content}");
        Assert.False(string.IsNullOrEmpty(content), "Videomixer response should not be empty");
        
        // Try to parse as JSON to ensure it's valid
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(content);
        Assert.True(jsonElement.ValueKind == JsonValueKind.Object || jsonElement.ValueKind == JsonValueKind.Array, 
            "Videomixer response should be valid JSON object or array");
    }

    [Fact]
    public async Task LiveApi_GetTopOfTheWeek_ParsesCorrectly()
    {
        // Arrange - Using current year-week
        var currentYear = DateTime.Now.Year;
        var currentWeek = GetIso8601WeekOfYear(DateTime.Now);
        var yearWeek = $"{currentYear}{currentWeek:D2}";
        var url = $"https://api.dumpert.nl/mobile_api/json/top5/week/{yearWeek}";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DumpertApiResponse>(content, _jsonOptions);

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}: {content}");
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        // Note: This might be empty if there's no data for the current week, which is acceptable
        
        if (result.Items.Count > 0)
        {
            var firstItem = result.Items[0];
            Assert.False(string.IsNullOrEmpty(firstItem.Id), "Item ID should not be null or empty");
            Assert.False(string.IsNullOrEmpty(firstItem.Title), "Item title should not be null or empty");
        }
    }

    [Fact]
    public async Task LiveApi_GetTopOfTheMonth_ParsesCorrectly()
    {
        // Arrange - Using current year-month
        var currentYear = DateTime.Now.Year;
        var currentMonth = DateTime.Now.Month;
        var yearMonth = $"{currentYear}{currentMonth:D2}";
        var url = $"https://api.dumpert.nl/mobile_api/json/top5/maand/{yearMonth}";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DumpertApiResponse>(content, _jsonOptions);

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}: {content}");
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        // Note: This might be empty if there's no data for the current month, which is acceptable
        
        if (result.Items.Count > 0)
        {
            var firstItem = result.Items[0];
            Assert.False(string.IsNullOrEmpty(firstItem.Id), "Item ID should not be null or empty");
            Assert.False(string.IsNullOrEmpty(firstItem.Title), "Item title should not be null or empty");
        }
    }

    [Fact]
    public async Task LiveApi_GetClassics_ParsesCorrectly()
    {
        // Arrange
        var url = "https://api.dumpert.nl/mobile_api/json/classics/0";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DumpertApiResponse>(content, _jsonOptions);

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}: {content}");
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.True(result.Items.Count > 0, "Expected at least one classic item");
        
        var firstItem = result.Items[0];
        Assert.False(string.IsNullOrEmpty(firstItem.Id), "Item ID should not be null or empty");
        Assert.False(string.IsNullOrEmpty(firstItem.Title), "Item title should not be null or empty");
    }

    [Fact]
    public async Task LiveApi_Search_ParsesCorrectly()
    {
        // Arrange
        var searchTerm = "test";
        var url = $"https://api.dumpert.nl/mobile_api/json/search/{searchTerm}/0";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DumpertApiResponse>(content, _jsonOptions);

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}: {content}");
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        // Search results might be empty for "test", which is acceptable
        
        if (result.Items.Count > 0)
        {
            var firstItem = result.Items[0];
            Assert.False(string.IsNullOrEmpty(firstItem.Id), "Item ID should not be null or empty");
            Assert.False(string.IsNullOrEmpty(firstItem.Title), "Item title should not be null or empty");
        }
    }

    [Fact]
    public async Task LiveApi_GetRelated_ParsesCorrectly()
    {
        // Arrange - Using a known article ID from the sample data
        var dumpertId = "100124857_51862663";
        var url = $"https://api.dumpert.nl/mobile_api/json/related/{dumpertId}";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DumpertApiResponse>(content, _jsonOptions);

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}: {content}");
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        // Related items might be empty, which is acceptable
        
        if (result.Items.Count > 0)
        {
            var firstItem = result.Items[0];
            Assert.False(string.IsNullOrEmpty(firstItem.Id), "Item ID should not be null or empty");
            Assert.False(string.IsNullOrEmpty(firstItem.Title), "Item title should not be null or empty");
        }
    }

    [Fact]
    public async Task LiveApi_GetInfo_ParsesCorrectly()
    {
        // Arrange - Using a known article ID from the sample data
        var dumpertId = "100124857_51862663";
        var url = $"https://api.dumpert.nl/mobile_api/json/info/{dumpertId}";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DumpertApiResponse>(content, _jsonOptions);

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}: {content}");
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.True(result.Items.Count > 0, "Expected at least one item in info response");
        
        var firstItem = result.Items[0];
        Assert.False(string.IsNullOrEmpty(firstItem.Id), "Item ID should not be null or empty");
        Assert.False(string.IsNullOrEmpty(firstItem.Title), "Item title should not be null or empty");
        Assert.False(string.IsNullOrEmpty(firstItem.Description), "Item description should not be null or empty");
    }

    // Helper method to get ISO 8601 week number
    private static int GetIso8601WeekOfYear(DateTime time)
    {
        var day = (int)time.DayOfWeek;
        if (day == 0) day = 7;
        return (time.AddDays(4 - day).DayOfYear + 6) / 7;
    }
}
