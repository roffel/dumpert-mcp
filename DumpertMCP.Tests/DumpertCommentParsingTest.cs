namespace DumpertMCP.Tests;

using System.Text.Json;
using models;
using Xunit;

public class DumpertCommentsParsingTests
{
    private class DumpertDefaultTestHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _client = new();

        public HttpClient CreateClient(string name = "") => _client;
    }
    
    public class DumpertFetchDataLiveTests
    {
        [Fact]
        public async Task FetchData_WithLiveUrl_ParsesDumpertCommentsCorrectly()
        {
            // Arrange
            var httpClientFactory = new DumpertDefaultTestHttpClientFactory();
            var service = new DumpertService(httpClientFactory);
            const string url = "https://comments.dumpert.nl/api/v1.0/articles/100124827/a8b42271/comments/?includeitems=20";

            // Act
            var result = await service.FetchData<DumpertCommentsRoot>(url);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("success", result.Status);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.Comments.Count > 0);

            var firstComment = result.Data.Comments.First();
            Assert.False(string.IsNullOrWhiteSpace(firstComment.DisplayContent));
        }
    }
    
    [Fact]
    public void CanDeserialize_CommentsJson_IntoModel()
    {
        // Arrange
        var json = File.ReadAllText("sample-dumpert-comments.json"); // save your example JSON in this file

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Act
        var result = JsonSerializer.Deserialize<DumpertCommentsRoot>(json, options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("success", result!.Status);
        Assert.NotNull(result.Data);
        Assert.True(result.Data!.Comments.Count > 0);

        var firstComment = result.Data.Comments[0];
        Assert.Equal("daywalkr", firstComment.AuthorUsername);
        Assert.Contains("Marc-Junior", firstComment.DisplayContent);

        // Assert child comment of a specific comment
        var commentWithChildren = result.Data.Comments
            .FirstOrDefault(c => c.ChildComments.Count > 0);

        Assert.NotNull(commentWithChildren);
        Assert.True(commentWithChildren!.ChildComments.Count > 0);
    }
}