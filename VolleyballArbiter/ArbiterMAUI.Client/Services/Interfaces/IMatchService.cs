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
        Task<IEnumerable<Match>> DownloadMatches();
    }
}
