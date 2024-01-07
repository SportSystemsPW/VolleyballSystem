using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbiterMAUI.Client.Services.Interfaces
{
    public interface ISignalrTextAnalyzerService
    {
        Task StartConnection();
        Task StopConnection();
        Task SendTextSentence(string text, int matchId);
    }
}
