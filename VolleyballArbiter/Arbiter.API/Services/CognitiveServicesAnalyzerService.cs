using Arbiter.API.Hubs;
using Arbiter.API.Services.Interfaces;
using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using static Arbiter.API.AppStrings.AppStrings;

namespace Arbiter.API.Services
{
    public class CognitiveServicesAnalyzerService : ITextAnalyzerService
    {
        private readonly IMatchReportService _matchReportService;
        private readonly IHubContext<MatchReportTextAnalyzerHub> _hubContext;
        private readonly IConfiguration _configuration;

        public CognitiveServicesAnalyzerService(IMatchReportService matchReportService, IHubContext<MatchReportTextAnalyzerHub> hubContext, IConfiguration configuration)
        {
            _matchReportService = matchReportService;
            _hubContext = hubContext;
            _configuration = configuration;
        }

        public async Task AnalyzeSentence(int matchId, string sentence)
        {
            var client = new TextAnalyticsClient(new Uri(_configuration["CognitiveServices-TextAnalyst-Endpoint"]!), new AzureKeyCredential(_configuration["CognitiveServices-TextAnalyst-Key"]!));
            var projectName = _configuration["CognitiveServices-TextAnalyst-ProjectName"]!;
            var deploymentName = _configuration["CognitiveServices-TextAnalyst-DeploymentName"]!;

            ClassifyDocumentOperation operation = await client.SingleLabelClassifyAsync(WaitUntil.Completed, new List<string> { sentence }, projectName, deploymentName);

            await foreach (ClassifyDocumentResultCollection documentsInPage in operation.Value)
            {
                foreach (ClassifyDocumentResult documentResult in documentsInPage)
                {
                    foreach (ClassificationCategory classification in documentResult.ClassificationCategories)
                    {
                        if(classification.Category == Actions.HOME_TEAM_POINT)
                        {
                            await _matchReportService.SaveAction(matchId, Actions.HOME_TEAM_POINT, sentence);
                            await _matchReportService.ChangeScore(matchId, Actions.HOME_TEAM_POINT);
                            await _hubContext.Clients.All.SendAsync("NewPoint", matchId, Actions.HOME_TEAM_POINT);
                        }
                        if (classification.Category == Actions.GUEST_TEAM_POINT)
                        {
                            await _matchReportService.SaveAction(matchId, Actions.GUEST_TEAM_POINT, sentence);
                            await _matchReportService.ChangeScore(matchId, Actions.GUEST_TEAM_POINT);
                            await _hubContext.Clients.All.SendAsync("NewPoint", matchId, Actions.GUEST_TEAM_POINT);
                        }
                    }
                }
            }
        }
    }
}
