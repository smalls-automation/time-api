using System.Globalization;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TimeApi.Api;

namespace TimeApi.Tests;

public class TimeEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TimeEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetTime_Returns200AndUtcTimestamp()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/time");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var utcValue = json.SubstringAfter("\"utc\":\"").SubstringBefore("\"");

        Assert.NotNull(utcValue);
        Assert.NotEmpty(utcValue);

        var parsed = DateTime.Parse(utcValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        Assert.Equal(DateTimeKind.Utc, parsed.Kind);

        // Allow a 5-second window to account for test execution time
        var diff = Math.Abs((DateTime.UtcNow - parsed).TotalSeconds);
        Assert.True(diff < 5, $"Expected UTC within 5 seconds, but difference was {diff} seconds");
    }

    [Fact]
    public async Task GetTime_ReturnsJsonContentType()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/time");

        // Assert
        var contentType = response.Content.Headers.ContentType?.MediaType;
        Assert.Equal("application/json", contentType);
    }

    [Fact]
    public async Task GetTime_ReturnsOnlyUtcProperty()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/time");

        // Assert
        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.True(root.TryGetProperty("utc", out _));
        Assert.Equal(1, root.GetPropertyCount());
    }

    [Fact]
    public async Task GetTime_ReturnsIso8601RoundtripFormat()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/time");

        // Assert
        var json = await response.Content.ReadAsStringAsync();
        var utcValue = json.SubstringAfter("\"utc\":\"").SubstringBefore("\"");

        // The "O" format produces ISO-8601 round-trip with Z suffix for UTC
        Assert.EndsWith("Z", utcValue, StringComparison.Ordinal);
        Assert.Matches(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z$", utcValue);
    }
}

internal static class StringExtensions
{
    public static string SubstringAfter(this string str, string after)
    {
        var index = str.IndexOf(after, StringComparison.Ordinal);
        return index >= 0 ? str[(index + after.Length)..] : string.Empty;
    }

    public static string SubstringBefore(this string str, string before)
    {
        var index = str.IndexOf(before, StringComparison.Ordinal);
        return index >= 0 ? str[..index] : str;
    }
}
