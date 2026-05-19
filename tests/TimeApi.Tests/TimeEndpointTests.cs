using System.Globalization;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TimeApi.Tests;

public class TimeEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TimeEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetTime_Returns200Ok()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/time");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
    public async Task GetTime_ReturnsUtcTimeProperty()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/time");

        // Assert
        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.True(root.TryGetProperty("utc_time", out _));
    }

    [Fact]
    public async Task GetTime_ReturnsOnlyUtcTimeProperty()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/time");

        // Assert
        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.Equal(1, root.GetPropertyCount());
        Assert.True(root.TryGetProperty("utc_time", out _));
    }

    [Fact]
    public async Task GetTime_ReturnsValidIso8601UtcTimestamp()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/time");

        // Assert
        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        var utcValue = doc.RootElement.GetProperty("utc_time").GetString()!;

        Assert.NotEmpty(utcValue);
        Assert.EndsWith("Z", utcValue, StringComparison.Ordinal);

        // Verify it parses as a valid ISO 8601 round-trip format
        var parsed = DateTime.Parse(utcValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        Assert.Equal(DateTimeKind.Utc, parsed.Kind);
    }

    [Fact]
    public async Task GetTime_ReturnsTimestampWithinFiveSecondsOfNow()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/time");

        // Assert
        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        var utcValue = doc.RootElement.GetProperty("utc_time").GetString()!;

        var parsed = DateTime.Parse(utcValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        var diff = Math.Abs((DateTime.UtcNow - parsed).TotalSeconds);
        Assert.True(diff < 5, $"Expected UTC within 5 seconds, but difference was {diff} seconds");
    }
}
