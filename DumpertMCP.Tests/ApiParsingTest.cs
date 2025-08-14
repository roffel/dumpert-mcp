using System.Text.Json;
using DumpertMCP.models;
using Xunit;

namespace DumpertMCP.Tests;

public class ApiParsingTest
{
    private class DumpertDefaultTestHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _client = new();

        public HttpClient CreateClient(string name = "") => _client;
    }

    [Fact]
    public void CanDeserialize_DumpertApiResponse_WithItems()
    {
        // Arrange
        var json = @"{
            ""items"": [
                {
                    ""id"": ""100124857_51862663"",
                    ""title"": ""Test Video"",
                    ""description"": ""Test Description"",
                    ""date"": ""2025-01-01"",
                    ""thumbnail"": ""https://example.com/thumb.jpg"",
                    ""media_type"": ""video"",
                    ""media"": [
                        {
                            ""url"": ""https://example.com/video.mp4"",
                            ""type"": ""video/mp4""
                        }
                    ]
                }
            ]
        }";

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Act
        var result = JsonSerializer.Deserialize<DumpertApiResponse>(json, options);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.Single(result.Items);
        
        var item = result.Items[0];
        Assert.Equal("100124857_51862663", item.Id);
        Assert.Equal("Test Video", item.Title);
        Assert.Equal("Test Description", item.Description);
        Assert.Equal("2025-01-01", item.Date);
        Assert.Equal("https://example.com/thumb.jpg", item.Thumbnail);
        Assert.Equal("video", item.MediaType);
        Assert.NotNull(item.Media);
        Assert.Single(item.Media);
        Assert.Equal("Test video description", item.Media[0].Description);
        Assert.Equal(30, item.Media[0].Duration);
    }

    [Fact]
    public void CanDeserialize_DumpertCommentsRoot_WithComments()
    {
        // Arrange
        var json = @"{
            ""data"": {
                ""comments"": [
                    {
                        ""id"": 253085847,
                        ""approved"": true,
                        ""article_id"": 100124857,
                        ""article_link"": ""https://www.dumpert.nl/item/100124857_51862663/"",
                        ""article_title"": ""Test Article"",
                        ""author_is_newbie"": false,
                        ""author_username"": ""testuser"",
                        ""banned"": false,
                        ""child_comments"": [],
                        ""creation_datetime"": ""2025-06-12T12:48:55Z"",
                        ""display_content"": ""Test comment content"",
                        ""html_markup"": ""<div>Test comment content</div>"",
                        ""is_author_premium_visible"": false,
                        ""kudos_count"": 5,
                        ""parent_id"": 0,
                        ""reference_id"": 0,
                        ""report_count"": 0
                    }
                ]
            },
            ""status"": ""success"",
            ""summary"": {
                ""can_comment"": true,
                ""comment_count"": 1,
                ""get_rate_limit"": ""400/minute"",
                ""moderated_at"": ""2025-06-12T11:31:09.078312+00:00""
            }
        }";

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
        Assert.NotNull(result.Data!.Comments);
        Assert.Single(result.Data.Comments);
        
        var comment = result.Data.Comments[0];
        Assert.Equal(253085847, comment.Id);
        Assert.True(comment.Approved);
        Assert.Equal("Test comment content", comment.Content);
        Assert.Equal(1, comment.Author);
        Assert.Empty(comment.ChildComments);
        Assert.Equal(new DateTime(2025, 6, 12, 12, 48, 55, DateTimeKind.Utc), comment.CreationDatetime);
        Assert.Equal("Test comment content", comment.DisplayContent);
        Assert.Equal(5, comment.KudosCount);
        Assert.Equal(0, comment.ReferenceId);
    }

    [Fact]
    public void CanDeserialize_DumpertCommentsRoot_WithNestedComments()
    {
        // Arrange
        var json = @"{
            ""data"": {
                ""comments"": [
                    {
                        ""id"": 253084451,
                        ""approved"": true,
                        ""article_id"": 100124857,
                        ""article_link"": ""https://www.dumpert.nl/item/100124857_51862663/"",
                        ""article_title"": ""Test Article"",
                        ""author_is_newbie"": false,
                        ""author_username"": ""parentuser"",
                        ""banned"": false,
                        ""child_comments"": [
                            {
                                ""id"": 253085469,
                                ""approved"": true,
                                ""article_id"": 100124857,
                                ""article_link"": ""https://www.dumpert.nl/item/100124857_51862663/"",
                                ""article_title"": ""Test Article"",
                                ""author_is_newbie"": false,
                                ""author_username"": ""childuser"",
                                ""banned"": false,
                                ""child_comments"": [],
                                ""creation_datetime"": ""2025-06-12T11:31:09Z"",
                                ""display_content"": ""Child comment"",
                                ""html_markup"": ""<div>Child comment</div>"",
                                ""is_author_premium_visible"": false,
                                ""kudos_count"": 1,
                                ""parent_id"": 253084451,
                                ""reference_id"": 0,
                                ""report_count"": 0
                            }
                        ],
                        ""creation_datetime"": ""2025-06-12T07:42:05Z"",
                        ""display_content"": ""Parent comment"",
                        ""html_markup"": ""<div>Parent comment</div>"",
                        ""is_author_premium_visible"": false,
                        ""kudos_count"": 28,
                        ""parent_id"": 0,
                        ""reference_id"": 0,
                        ""report_count"": 0
                    }
                ]
            },
            ""status"": ""success""
        }";

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
        Assert.NotNull(result.Data!.Comments);
        Assert.Single(result.Data.Comments);
        
        var parentComment = result.Data.Comments[0];
        Assert.Equal(253084451, parentComment.Id);
        Assert.Equal(1, parentComment.Author);
        Assert.Single(parentComment.ChildComments);
        
        var childComment = parentComment.ChildComments[0];
        Assert.Equal(253085469, childComment.Id);
        Assert.Equal(2, childComment.Author);
        Assert.Equal("Child comment", childComment.Content);
    }

    [Fact]
    public void CanDeserialize_DumpertSingleCommentRoot()
    {
        // Arrange
        var json = @"{
            ""data"": {
                ""id"": 253085847,
                ""approved"": true,
                ""article_id"": 100124857,
                ""article_link"": ""https://www.dumpert.nl/item/100124857_51862663/"",
                ""article_title"": ""Test Article"",
                ""author_is_newbie"": false,
                ""author_username"": ""testuser"",
                ""banned"": false,
                ""child_comments"": [],
                ""creation_datetime"": ""2025-06-12T12:48:55Z"",
                ""display_content"": ""Test comment content"",
                ""html_markup"": ""<div>Test comment content</div>"",
                ""is_author_premium_visible"": false,
                ""kudos_count"": 5,
                ""parent_id"": 0,
                ""reference_id"": 0,
                ""report_count"": 0
            },
            ""status"": ""success""
        }";

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Act
        var result = JsonSerializer.Deserialize<DumpertSingleCommentRoot>(json, options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("success", result!.Status);
        Assert.NotNull(result.Data);
        Assert.Equal(253085847, result.Data!.Comment.Id);
        Assert.Equal("testuser", result.Data.Comment.AuthorUsername);
        Assert.Equal("Test comment content", result.Data.Comment.DisplayContent);
    }

    [Fact]
    public void CanDeserialize_SoundboardItems()
    {
        // Arrange
        var json = @"[
            {
                ""name"": ""Test Sound"",
                ""url"": ""https://example.com/sound.mp3"",
                ""thumbnail"": ""https://example.com/thumb.jpg"",
                ""video"": ""https://example.com/video.mp4"",
                ""duration"": 5
            },
            {
                ""name"": ""Another Sound"",
                ""url"": ""https://example.com/another.mp3"",
                ""thumbnail"": ""https://example.com/another-thumb.jpg"",
                ""video"": ""https://example.com/another-video.mp4"",
                ""duration"": 3
            }
        ]";

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Act
        var result = JsonSerializer.Deserialize<List<SoundboardItem>>(json, options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result!.Count);
        
        var firstItem = result[0];
        Assert.Equal("Test Sound", firstItem.Name);
        Assert.Equal("https://example.com/sound.mp3", firstItem.Url);
        Assert.Equal("https://example.com/thumb.jpg", firstItem.Thumbnail);
        Assert.Equal("https://example.com/video.mp4", firstItem.Video);
        Assert.Equal(5, firstItem.Duration);
        
        var secondItem = result[1];
        Assert.Equal("Another Sound", secondItem.Name);
        Assert.Equal("https://example.com/another.mp3", secondItem.Url);
        Assert.Equal("https://example.com/another-thumb.jpg", secondItem.Thumbnail);
        Assert.Equal("https://example.com/another-video.mp4", secondItem.Video);
        Assert.Equal(3, secondItem.Duration);
    }

    [Fact]
    public void CanDeserialize_DumpertMedia()
    {
        // Arrange
        var json = @"{
            ""description"": ""Test video description"",
            ""duration"": 30,
            ""mediatype"": ""video"",
            ""variants"": [
                {
                    ""uri"": ""https://example.com/video.mp4"",
                    ""version"": ""1.0""
                }
            ]
        }";

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Act
        var result = JsonSerializer.Deserialize<DumpertMedia>(json, options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test video description", result!.Description);
        Assert.Equal(30, result.Duration);
        Assert.Equal("video", result.Mediatype);
        Assert.NotNull(result.Variants);
        Assert.Single(result.Variants);
        Assert.Equal("https://example.com/video.mp4", result.Variants[0].Uri);
        Assert.Equal("1.0", result.Variants[0].Version);
    }

    [Fact]
    public void CanDeserialize_DumpertSummary()
    {
        // Arrange
        var json = @"{
            ""can_comment"": true,
            ""comment_count"": 37,
            ""get_rate_limit"": ""400/minute"",
            ""moderated_at"": ""2025-06-12T11:31:09.078312+00:00""
        }";

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Act
        var result = JsonSerializer.Deserialize<DumpertSummary>(json, options);

        // Assert
        Assert.NotNull(result);
        Assert.True(result!.CanComment);
        Assert.Equal(37, result.CommentCount);
        Assert.Equal("400/minute", result.GetRateLimit);
        Assert.Equal(new DateTime(2025, 6, 12, 11, 31, 9, DateTimeKind.Utc), result.ModeratedAt);
    }

    [Fact]
    public void CanDeserialize_EmptyApiResponse()
    {
        // Arrange
        var json = @"{
            ""items"": []
        }";

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Act
        var result = JsonSerializer.Deserialize<DumpertApiResponse>(json, options);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.Empty(result.Items);
    }

    [Fact]
    public void CanDeserialize_EmptyCommentsResponse()
    {
        // Arrange
        var json = @"{
            ""data"": {
                ""comments"": []
            },
            ""status"": ""success""
        }";

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
        Assert.NotNull(result.Data!.Comments);
        Assert.Empty(result.Data.Comments);
    }
}
