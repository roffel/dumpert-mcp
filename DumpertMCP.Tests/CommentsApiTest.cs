using System.Text.Json;
using DumpertMCP.models;
using Xunit;

namespace DumpertMCP.Tests;

public class CommentsApiTest
{
    private readonly HttpClient _httpClient = new();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public async Task CanDeserialize_RealCommentsApiResponse()
    {
        // Arrange
        var url = "https://comment.dumpert.nl/api/v1.0/articles/100130000/237d8919/comments";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DumpertCommentsResponse>(content, _jsonOptions);

        // Assert
        Assert.True(response.IsSuccessStatusCode, $"API call failed with status {response.StatusCode}");
        Assert.NotNull(result);
        Assert.NotNull(result!.Authors);
        Assert.NotNull(result.Comments);
        Assert.NotNull(result.Summary);

        // Check that we have data
        Assert.True(result.Authors.Count > 0, "Should have at least one author");
        Assert.True(result.Comments.Count > 0, "Should have at least one comment");

        // Check summary
        Assert.Equal(100130000, result.Summary.Id);
        Assert.Equal("Afplakken 2.0", result.Summary.Title);
        Assert.True(result.Summary.CommentCount > 0);
        Assert.True(result.Summary.CanComment);

        // Check first comment
        var firstComment = result.Comments[0];
        Assert.True(firstComment.Id > 0);
        Assert.True(firstComment.Approved);
        Assert.False(string.IsNullOrEmpty(firstComment.Content));
        Assert.True(firstComment.Author > 0);
        Assert.NotNull(firstComment.ChildComments);

        // Check first author
        var firstAuthor = result.Authors[0];
        Assert.True(firstAuthor.Id > 0);
        Assert.False(string.IsNullOrEmpty(firstAuthor.Username));
        Assert.NotEqual(default(DateTime), firstAuthor.RegisteredAt);
    }

    [Fact]
    public async Task CanDeserialize_CommentsWithChildComments()
    {
        // Arrange
        var url = "https://comment.dumpert.nl/api/v1.0/articles/100130000/237d8919/comments";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DumpertCommentsResponse>(content, _jsonOptions);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result!.Comments);

        // Find a comment with child comments
        var commentWithChildren = result.Comments.FirstOrDefault(c => c.ChildComments.Count > 0);
        if (commentWithChildren != null)
        {
            Assert.True(commentWithChildren.ChildComments.Count > 0);
            var childComment = commentWithChildren.ChildComments[0];
            Assert.True(childComment.Id > 0);
            Assert.False(string.IsNullOrEmpty(childComment.Content));
        }
    }

    [Fact]
    public async Task CanMap_AuthorToComment()
    {
        // Arrange
        var url = "https://comment.dumpert.nl/api/v1.0/articles/100130000/237d8919/comments";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DumpertCommentsResponse>(content, _jsonOptions);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result!.Authors);
        Assert.NotNull(result.Comments);

        // Check that we can map authors to comments
        var firstComment = result.Comments[0];
        var author = result.Authors.FirstOrDefault(a => a.Id == firstComment.Author);
        
        Assert.NotNull(author);
        Assert.Equal(firstComment.Author, author!.Id);
        Assert.False(string.IsNullOrEmpty(author.Username));
    }

    [Fact]
    public async Task CanHandle_BackwardCompatibility()
    {
        // Arrange
        var url = "https://comment.dumpert.nl/api/v1.0/articles/100130000/237d8919/comments";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DumpertCommentsResponse>(content, _jsonOptions);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result!.Comments);

        var firstComment = result.Comments[0];
        
        // Test backward compatibility properties
        Assert.Equal(firstComment.Content, firstComment.DisplayContent);
        Assert.False(string.IsNullOrEmpty(firstComment.DisplayContent));
    }

    [Fact]
    public async Task CanHandle_EmptyCommentsResponse()
    {
        // Arrange - Use a different article ID that might have no comments
        var url = "https://comment.dumpert.nl/api/v1.0/articles/100000000/00000000/comments";

        // Act
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        // Even if the article doesn't exist or has no comments, we should get a valid response structure
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NotFound);
        
        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<DumpertCommentsResponse>(content, _jsonOptions);
            Assert.NotNull(result);
            Assert.NotNull(result!.Authors);
            Assert.NotNull(result.Comments);
            Assert.NotNull(result.Summary);
        }
    }
}
