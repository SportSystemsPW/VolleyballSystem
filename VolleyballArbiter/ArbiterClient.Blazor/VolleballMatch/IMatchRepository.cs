using Arbiter.Contracts;

namespace ArbiterClient.Blazor.VolleballMatch;

public interface IMatchRepository
{
    Task<MatchDto> GetMatch(int matchId);

    Task UpdateMatch(int matchId, MatchDto matchDto);
}
