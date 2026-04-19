using Elp.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace Elp.UnitTests.Integration;

public class CertificatesControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CertificatesControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostCheckAuthorization_ShouldReturnOkAndTrue()
    {
        var requestDto = new
        {
            KrzpId = "11111111-1111-1111-1111-111111111111",
            Rid = "ABC-123-456"
        };

        var response = await _client.PostAsJsonAsync("/api/v2/posudky/ridicskeOpravneni/zalozeni/opravneni", requestDto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<bool>();

        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetById_GivenSeededId_ShouldReturnCorrectJsonAndETagHeader()
    {
        var seededId = "a1111111-1111-1111-1111-111111111111";

        var response = await _client.GetAsync($"/api/v2/posudky/ridicskeOpravneni/{seededId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PosudekRoDetailDto>();

        result.Should().NotBeNull();
        result!.Rid.Should().Be("ABC-123-456");
        result.StavPosudku.Kod.Should().Be("VYDANO");

        response.Headers.ETag.Should().NotBeNull();
        response.Headers.ETag!.Tag.Should().NotBeEmpty();
    }
}