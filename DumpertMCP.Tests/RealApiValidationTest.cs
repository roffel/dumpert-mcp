using System.Text.Json;
using DumpertMCP.models;
using Xunit;

namespace DumpertMCP.Tests;

public class RealApiValidationTest
{
    private readonly HttpClient _httpClient = new();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public async Task Validate_TopOfTheDay_ApiResponse_Structure()
    {
        // Arrange
        var url = "https://api-live.dumpert.nl/mobile_api/json/top5/dag/";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}");
        Assert.False(string.IsNullOrEmpty(content), "Response should not be empty");

        // Parse as JsonElement to inspect structure
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(content);
        
        // Check if response has expected top-level fields
        Assert.True(jsonElement.TryGetProperty("items", out var itemsElement), "Response should have 'items' field");
        Assert.True(jsonElement.TryGetProperty("success", out var successElement), "Response should have 'success' field");
        Assert.True(jsonElement.TryGetProperty("gentime", out var gentimeElement), "Response should have 'gentime' field");
        Assert.True(jsonElement.TryGetProperty("used_date", out var usedDateElement), "Response should have 'used_date' field");

        // Check if items is an array
        Assert.Equal(JsonValueKind.Array, itemsElement.ValueKind);

        // Check if we have at least one item
        var itemsArray = itemsElement.EnumerateArray().ToList();
        Assert.True(itemsArray.Count > 0, "Should have at least one item");

        // Inspect first item structure
        var firstItem = itemsArray[0];
        Assert.True(firstItem.TryGetProperty("id", out var idElement), "Item should have 'id' field");
        Assert.True(firstItem.TryGetProperty("title", out var titleElement), "Item should have 'title' field");
        Assert.True(firstItem.TryGetProperty("description", out var descriptionElement), "Item should have 'description' field");
        Assert.True(firstItem.TryGetProperty("date", out var dateElement), "Item should have 'date' field");
        Assert.True(firstItem.TryGetProperty("thumbnail", out var thumbnailElement), "Item should have 'thumbnail' field");
        Assert.True(firstItem.TryGetProperty("media_type", out var mediaTypeElement), "Item should have 'media_type' field");
        Assert.True(firstItem.TryGetProperty("media", out var mediaElement), "Item should have 'media' field");

        // Check additional fields that exist in real API but not in our model
        Assert.True(firstItem.TryGetProperty("stats", out var statsElement), "Item should have 'stats' field");
        Assert.True(firstItem.TryGetProperty("tags", out var tagsElement), "Item should have 'tags' field");
        Assert.True(firstItem.TryGetProperty("upload_id", out var uploadIdElement), "Item should have 'upload_id' field");
        Assert.True(firstItem.TryGetProperty("nopreroll", out var noprerollElement), "Item should have 'nopreroll' field");
        Assert.True(firstItem.TryGetProperty("nsfw", out var nsfwElement), "Item should have 'nsfw' field");
        Assert.True(firstItem.TryGetProperty("secret", out var secretElement), "Item should have 'secret' field");
        Assert.True(firstItem.TryGetProperty("resolutions", out var resolutionsElement), "Item should have 'resolutions' field");
        Assert.True(firstItem.TryGetProperty("scripts", out var scriptsElement), "Item should have 'scripts' field");
        Assert.True(firstItem.TryGetProperty("still", out var stillElement), "Item should have 'still' field");
        Assert.True(firstItem.TryGetProperty("stills", out var stillsElement), "Item should have 'stills' field");

        // Check media structure
        Assert.Equal(JsonValueKind.Array, mediaElement.ValueKind);
        var mediaArray = mediaElement.EnumerateArray().ToList();
        Assert.True(mediaArray.Count > 0, "Should have at least one media item");

        var firstMedia = mediaArray[0];
        Assert.True(firstMedia.TryGetProperty("description", out var mediaDescElement), "Media should have 'description' field");
        Assert.True(firstMedia.TryGetProperty("duration", out var mediaDurationElement), "Media should have 'duration' field");
        Assert.True(firstMedia.TryGetProperty("mediatype", out var mediaTypeElement2), "Media should have 'mediatype' field");
        Assert.True(firstMedia.TryGetProperty("variants", out var variantsElement), "Media should have 'variants' field");

        // Check variants structure
        Assert.Equal(JsonValueKind.Array, variantsElement.ValueKind);
        var variantsArray = variantsElement.EnumerateArray().ToList();
        Assert.True(variantsArray.Count > 0, "Should have at least one variant");

        var firstVariant = variantsArray[0];
        Assert.True(firstVariant.TryGetProperty("uri", out var uriElement), "Variant should have 'uri' field");
        Assert.True(firstVariant.TryGetProperty("version", out var versionElement), "Variant should have 'version' field");
    }

    [Fact]
    public async Task Validate_Soundboard_ApiResponse_Structure()
    {
        // Arrange
        var url = "https://video-snippets.dumpert.nl/soundboard.json";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}");
        Assert.False(string.IsNullOrEmpty(content), "Response should not be empty");

        // Try to deserialize with our model
        var result = JsonSerializer.Deserialize<List<SoundboardItem>>(content, _jsonOptions);

        // Assert
        Assert.NotNull(result);
        Assert.True(result!.Count > 0, "Should have at least one soundboard item");

        var firstItem = result[0];
        Assert.False(string.IsNullOrEmpty(firstItem.Name), "Name should not be empty");
        Assert.False(string.IsNullOrEmpty(firstItem.Url), "URL should not be empty");
        Assert.False(string.IsNullOrEmpty(firstItem.Thumbnail), "Thumbnail should not be empty");
        Assert.False(string.IsNullOrEmpty(firstItem.Video), "Video should not be empty");
        Assert.True(firstItem.Duration > 0, "Duration should be positive");
    }

    [Fact]
    public async Task Validate_Videomixer_ApiResponse_Structure()
    {
        // Arrange
        var url = "https://video-snippets.dumpert.nl/videomixer.json";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}");
        Assert.False(string.IsNullOrEmpty(content), "Response should not be empty");

        // Parse as JsonElement to inspect structure
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(content);
        
        // Check if response has expected top-level fields
        Assert.True(jsonElement.TryGetProperty("name", out var nameElement), "Response should have 'name' field");
        Assert.True(jsonElement.TryGetProperty("project_selection", out var projectSelectionElement), "Response should have 'project_selection' field");
        Assert.True(jsonElement.TryGetProperty("colors", out var colorsElement), "Response should have 'colors' field");
        Assert.True(jsonElement.TryGetProperty("theme", out var themeElement), "Response should have 'theme' field");
    }

    [Fact]
    public async Task Validate_Comments_ApiResponse_Structure()
    {
        // Arrange - Using the working article ID from the real API
        var url = "https://comment.dumpert.nl/api/v1.0/articles/100130000/237d8919/comments";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}");
        Assert.False(string.IsNullOrEmpty(content), "Response should not be empty");

        // Parse as JsonElement to inspect structure
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(content);
        
        // Check if response has expected top-level fields
        Assert.True(jsonElement.TryGetProperty("authors", out var authorsElement), "Response should have 'authors' field");
        Assert.True(jsonElement.TryGetProperty("comments", out var commentsElement), "Response should have 'comments' field");
        Assert.True(jsonElement.TryGetProperty("summary", out var summaryElement), "Response should have 'summary' field");

        // Check if comments is an array
        Assert.Equal(JsonValueKind.Array, commentsElement.ValueKind);

        var commentsArray = commentsElement.EnumerateArray().ToList();
        if (commentsArray.Count > 0)
        {
            var firstComment = commentsArray[0];
            Assert.True(firstComment.TryGetProperty("id", out var idElement), "Comment should have 'id' field");
            Assert.True(firstComment.TryGetProperty("approved", out var approvedElement), "Comment should have 'approved' field");
            Assert.True(firstComment.TryGetProperty("creation_datetime", out var creationDatetimeElement), "Comment should have 'creation_datetime' field");
            Assert.True(firstComment.TryGetProperty("content", out var contentElement), "Comment should have 'content' field");
            Assert.True(firstComment.TryGetProperty("kudos_count", out var kudosCountElement), "Comment should have 'kudos_count' field");
            Assert.True(firstComment.TryGetProperty("reference_id", out var referenceIdElement), "Comment should have 'reference_id' field");
            Assert.True(firstComment.TryGetProperty("author", out var authorElement), "Comment should have 'author' field");
        }

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
    public async Task Validate_Model_Compatibility_With_Real_Api()
    {
        // This test validates that our models can handle the real API responses
        // even if they don't capture all fields

        // Test DumpertApiResponse with real data
        var topOfDayUrl = "https://api-live.dumpert.nl/mobile_api/json/top5/dag/";
        var response = await _httpClient.GetAsync(topOfDayUrl);
        var content = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode, "API call should succeed");

        // Try to deserialize with our model (should work even if we don't capture all fields)
        var result = JsonSerializer.Deserialize<DumpertApiResponse>(content, _jsonOptions);

        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.True(result.Items.Count > 0, "Should have at least one item");

        var firstItem = result.Items[0];
        Assert.False(string.IsNullOrEmpty(firstItem.Id), "Item ID should not be empty");
        Assert.False(string.IsNullOrEmpty(firstItem.Title), "Item title should not be empty");
        Assert.NotNull(firstItem.Media);

        // Test SoundboardItem with real data
        var soundboardUrl = "https://video-snippets.dumpert.nl/soundboard.json";
        var soundboardResponse = await _httpClient.GetAsync(soundboardUrl);
        var soundboardContent = await soundboardResponse.Content.ReadAsStringAsync();

        Assert.True(soundboardResponse.IsSuccessStatusCode, "Soundboard API call should succeed");

        var soundboardResult = JsonSerializer.Deserialize<List<SoundboardItem>>(soundboardContent, _jsonOptions);

        Assert.NotNull(soundboardResult);
        Assert.True(soundboardResult!.Count > 0, "Should have at least one soundboard item");

        var firstSoundboardItem = soundboardResult[0];
        Assert.False(string.IsNullOrEmpty(firstSoundboardItem.Name), "Soundboard item name should not be empty");
        Assert.False(string.IsNullOrEmpty(firstSoundboardItem.Url), "Soundboard item URL should not be empty");
        Assert.True(firstSoundboardItem.Duration > 0, "Soundboard item duration should be positive");
    }

    [Fact]
    public async Task Validate_Api_Endpoints_Are_Accessible()
    {
        // Test that all the API endpoints we're trying to use are actually accessible

        var endpoints = new[]
        {
            "https://api-live.dumpert.nl/mobile_api/json/top5/dag/",
            "https://api.dumpert.nl/mobile_api/json/dumperttv",
            "https://api.dumpert.nl/mobile_api/json/hotshiz",
            "https://api.dumpert.nl/mobile_api/json/classics/0",
            "https://video-snippets.dumpert.nl/soundboard.json",
            "https://video-snippets.dumpert.nl/videomixer.json"
        };

        foreach (var endpoint in endpoints)
        {
            var response = await _httpClient.GetAsync(endpoint);
            Assert.True(response.IsSuccessStatusCode, $"Endpoint {endpoint} should be accessible");
            
            var content = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(content), $"Endpoint {endpoint} should return content");
        }
    }
}
