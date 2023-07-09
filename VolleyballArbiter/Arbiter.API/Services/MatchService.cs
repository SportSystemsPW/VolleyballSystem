using Arbiter.API.Data;
using Arbiter.API.Data.Models;
using Arbiter.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Arbiter.API.Services
{
    public class MatchService : IMatchService
    {
        private readonly DatabaseContext _dbContext;

        public MatchService(DatabaseContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public IEnumerable<Match> GetMatches()
        {
            return _dbContext.Matches.Where(x => x.Schedule > DateTime.Now.AddDays(-90))
                .Include(x => x.HomeTeam)
                .Include(x => x.GuestTeam)
                .Include(x => x.League);
        }
    }
}
