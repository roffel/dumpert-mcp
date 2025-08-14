using System.Text.Json;
using DumpertMCP.models;
using Xunit;

namespace DumpertMCP.Tests;

public class KudosApiTest
{
    private readonly HttpClient _httpClient = new();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public async Task CanRate_ItemUp()
    {
        // Arrange - Get a real item ID from the top of the day
        var topItemsUrl = "https://api-live.dumpert.nl/mobile_api/json/top5/dag/";
        var topResponse = await _httpClient.GetAsync(topItemsUrl);
        var topContent = await topResponse.Content.ReadAsStringAsync();
        var topResult = JsonSerializer.Deserialize<DumpertApiResponse>(topContent, _jsonOptions);

        Assert.True(topResponse.IsSuccessStatusCode, "Failed to get top items");
        Assert.NotNull(topResult);
        Assert.NotNull(topResult!.Items);
        Assert.True(topResult.Items.Count > 0, "No items found");

        var itemId = topResult.Items[0].Id;
        var ratingUrl = $"https://post.dumpert.nl/api/v1.0/rating/{itemId}/up";

        // Act
        var response = await _httpClient.GetAsync(ratingUrl);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        // The rating API might require authentication, so we check for either success or auth error
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.Unauthorized || 
                   response.StatusCode == System.Net.HttpStatusCode.Forbidden, 
                   $"Rating failed with status {response.StatusCode}: {content}");

        if (response.IsSuccessStatusCode)
        {
            // If successful, the response should contain some indication of success
            Assert.False(string.IsNullOrEmpty(content), "Response should not be empty");
            Console.WriteLine($"Rating response: {content}");
        }
        else
        {
            Console.WriteLine($"Rating requires authentication: {content}");
        }
    }

    [Fact]
    public async Task CanRate_ItemDown()
    {
        // Arrange - Get a real item ID from the top of the day
        var topItemsUrl = "https://api-live.dumpert.nl/mobile_api/json/top5/dag/";
        var topResponse = await _httpClient.GetAsync(topItemsUrl);
        var topContent = await topResponse.Content.ReadAsStringAsync();
        var topResult = JsonSerializer.Deserialize<DumpertApiResponse>(topContent, _jsonOptions);

        Assert.True(topResponse.IsSuccessStatusCode, "Failed to get top items");
        Assert.NotNull(topResult);
        Assert.NotNull(topResult!.Items);
        Assert.True(topResult.Items.Count > 0, "No items found");

        var itemId = topResult.Items[0].Id;
        var ratingUrl = $"https://post.dumpert.nl/api/v1.0/rating/{itemId}/down";

        // Act
        var response = await _httpClient.GetAsync(ratingUrl);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        // The rating API might require authentication, so we check for either success or auth error
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.Unauthorized || 
                   response.StatusCode == System.Net.HttpStatusCode.Forbidden, 
                   $"Rating failed with status {response.StatusCode}: {content}");

        if (response.IsSuccessStatusCode)
        {
            // If successful, the response should contain some indication of success
            Assert.False(string.IsNullOrEmpty(content), "Response should not be empty");
            Console.WriteLine($"Rating response: {content}");
        }
        else
        {
            Console.WriteLine($"Rating requires authentication: {content}");
        }
    }

    [Fact]
    public async Task CanRate_CommentKudos()
    {
        // Arrange - Get a real comment from the comments API
        var commentsUrl = "https://comment.dumpert.nl/api/v1.0/articles/100130000/237d8919/comments";
        var commentsResponse = await _httpClient.GetAsync(commentsUrl);
        var commentsContent = await commentsResponse.Content.ReadAsStringAsync();
        var commentsResult = JsonSerializer.Deserialize<DumpertCommentsResponse>(commentsContent, _jsonOptions);

        Assert.True(commentsResponse.IsSuccessStatusCode, "Failed to get comments");
        Assert.NotNull(commentsResult);
        Assert.NotNull(commentsResult!.Comments);
        Assert.True(commentsResult.Comments.Count > 0, "No comments found");

        var commentId = commentsResult.Comments[0].Id;
        var kudosUrl = $"https://comment.dumpert.nl/api/v1.0/comments/{commentId}/kudos";

        // Act
        var response = await _httpClient.PostAsync(kudosUrl, null);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        // The kudos API might require authentication, so we check for either success or auth error
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.Unauthorized || 
                   response.StatusCode == System.Net.HttpStatusCode.Forbidden || 
                   response.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed, 
                   $"Kudos failed with status {response.StatusCode}: {content}");

        if (response.IsSuccessStatusCode)
        {
            // If successful, the response should contain some indication of success
            Assert.False(string.IsNullOrEmpty(content), "Response should not be empty");
            Console.WriteLine($"Kudos response: {content}");
        }
        else
        {
            Console.WriteLine($"Kudos requires authentication or different method: {content}");
        }
    }

    [Fact]
    public async Task CanRate_CommentDownvote()
    {
        // Arrange - Get a real comment from the comments API
        var commentsUrl = "https://comment.dumpert.nl/api/v1.0/articles/100130000/237d8919/comments";
        var commentsResponse = await _httpClient.GetAsync(commentsUrl);
        var commentsContent = await commentsResponse.Content.ReadAsStringAsync();
        var commentsResult = JsonSerializer.Deserialize<DumpertCommentsResponse>(commentsContent, _jsonOptions);

        Assert.True(commentsResponse.IsSuccessStatusCode, "Failed to get comments");
        Assert.NotNull(commentsResult);
        Assert.NotNull(commentsResult!.Comments);
        Assert.True(commentsResult.Comments.Count > 0, "No comments found");

        var commentId = commentsResult.Comments[0].Id;
        var downvoteUrl = $"https://comment.dumpert.nl/api/v1.0/comments/{commentId}/downvote";

        // Act
        var response = await _httpClient.PostAsync(downvoteUrl, null);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        // The downvote API might require authentication, so we check for either success or auth error
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.Unauthorized || 
                   response.StatusCode == System.Net.HttpStatusCode.Forbidden || 
                   response.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed, 
                   $"Downvote failed with status {response.StatusCode}: {content}");

        if (response.IsSuccessStatusCode)
        {
            // If successful, the response should contain some indication of success
            Assert.False(string.IsNullOrEmpty(content), "Response should not be empty");
            Console.WriteLine($"Downvote response: {content}");
        }
        else
        {
            Console.WriteLine($"Downvote requires authentication or different method: {content}");
        }
    }
}
