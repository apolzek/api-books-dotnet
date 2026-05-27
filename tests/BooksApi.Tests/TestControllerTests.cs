using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BooksApi.Tests;

public class TestControllerTests : IClassFixture<TestAppFactory>
{
    private readonly HttpClient _client;

    public TestControllerTests(TestAppFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = true,
        });
    }

    [Fact]
    public async Task LoggingLevels_ReturnsExpectedString()
    {
        var response = await _client.GetAsync("/api/test/LoggingLevels");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("api-books logging-levels", body);
    }

    [Fact]
    public async Task GetAllHeaders_ReturnsDictionary()
    {
        _client.DefaultRequestHeaders.Add("X-Test-Header", "abc123");
        var response = await _client.GetAsync("/api/test/GetAllHeaders");
        response.EnsureSuccessStatusCode();
        var headers = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        Assert.NotNull(headers);
        Assert.Contains(headers!, kv => kv.Key.Equals("X-Test-Header", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task QueryString_EchoesParameter()
    {
        var response = await _client.GetAsync("/api/test/QueryString?querystring=hello");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        Assert.Equal("query testing: hello", body);
    }

    [Theory]
    [InlineData(2, 3, 5)]
    [InlineData(-1, 1, 0)]
    [InlineData(100, 200, 300)]
    public async Task SumNumbers_ReturnsCorrectSum(int a, int b, int expected)
    {
        var response = await _client.GetAsync($"/api/test/SumNumbers?num1={a}&num2={b}");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        Assert.Equal(expected.ToString(), body);
    }

    [Fact]
    public async Task Status_ReturnsRequestedStatusCode()
    {
        var response = await _client.GetAsync("/api/test/Status?response=404");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Status_Returns500ForServerError()
    {
        var response = await _client.GetAsync("/api/test/Status?response=500");
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task Delay_ReturnsOkAfterWaiting()
    {
        var response = await _client.GetAsync("/api/test/time/50");
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("50 milissegundos", body);
    }

    [Fact]
    public async Task Delay_RejectsNegativeValue()
    {
        var response = await _client.GetAsync("/api/test/time/-1");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delay_RejectsTooLargeValue()
    {
        var response = await _client.GetAsync("/api/test/time/30001");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task HealthCheck_ReturnsHealthy()
    {
        var response = await _client.GetAsync("/health");
        response.EnsureSuccessStatusCode();
    }
}
