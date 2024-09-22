using Arbiter.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace ArbiterClient.Blazor.VolleballMatch;

public class MockMatchRepository : IMatchRepository
{
    private readonly IMemoryCache _memoryCache;

    public MockMatchRepository(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _memoryCache.Set(1, CreateMock());
    }

    public Task<MatchDto> GetMatch(int matchId)
    {
        if (_memoryCache.TryGetValue(matchId, out var matchDto))
        {
            return Task.FromResult(matchDto as MatchDto);
        }

        return Task.FromResult(CreateMock());
    }

    public Task UpdateMatch(int matchId, MatchDto matchDto)
    {
        _memoryCache.Set(matchId, matchDto);

        return Task.CompletedTask;
    }

    private MatchDto CreateMock()
    {
        return new MatchDto
        {
            TeamA = new TeamDto
            {
                Id = 1,
                Name = "Skra Bełchatów"
            },
            TeamB = new TeamDto
            {
                Id = 2,
                Name = "Jastrzębski Węgiel"
            },
            Finished = false,
            Score = new MatchScoreDto
            {
                TeamASetScore = 0,
                TeamBSetScore = 0,
                Sets = []
            }
        };
    }
}

