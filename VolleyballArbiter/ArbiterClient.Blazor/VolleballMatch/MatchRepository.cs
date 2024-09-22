using Arbiter.Contracts;
using System.Net.Http.Json;
using System.Text.Json;

namespace ArbiterClient.Blazor.VolleballMatch;

public class MatchRepository : IMatchRepository
{
    private readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly IHttpClientFactory _httpClientFactory;

    private const string HttpClientName = "arbiter-api";

    public MatchRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<MatchDto> GetMatch(int matchId)
    {
        var result = await _httpClientFactory.CreateClient(HttpClientName).GetAsync($"matches/{matchId}");

        result.EnsureSuccessStatusCode();

        var content = await result.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<MatchDto>(content, JsonSerializerOptions);
    }

    public async Task UpdateMatch(int matchId, MatchDto matchDto)
    {
        var result = await _httpClientFactory.CreateClient(HttpClientName).PutAsJsonAsync($"matches/{matchId}", matchDto);

        result.EnsureSuccessStatusCode();
    }
}

