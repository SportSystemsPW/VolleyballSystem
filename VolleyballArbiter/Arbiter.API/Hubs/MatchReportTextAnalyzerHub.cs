using Arbiter.API.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Arbiter.API.Hubs
{
    public class MatchReportTextAnalyzerHub : Hub
    {
        public MatchReportTextAnalyzerHub()
        {
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
