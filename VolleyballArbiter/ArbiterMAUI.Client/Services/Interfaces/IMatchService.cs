using ArbiterMAUI.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbiterMAUI.Client.Services.Interfaces
{
    public interface IMatchService
    {
        Task<IEnumerable<Match>> DownloadMatches(DateTime fromDate, DateTime toDate);
        Task<int> GetMatchStatus(int matchId);
        Task<int> GetSetStatus(int matchId, int setNumber);
        Task<bool> StartMatch(int matchId);
        Task<bool> FinishMatch(int matchId);
        Task<bool> StartSet(int matchId, int setNumber);
        Task<bool> FinishSet(int matchId, int setNumber);
    }
}
