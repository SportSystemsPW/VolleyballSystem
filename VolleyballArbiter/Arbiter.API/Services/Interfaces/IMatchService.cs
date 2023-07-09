using Arbiter.API.Data.Models;

namespace Arbiter.API.Services.Interfaces
{
    public interface IMatchService
    {
        IEnumerable<Match> GetMatches();
    }
}
