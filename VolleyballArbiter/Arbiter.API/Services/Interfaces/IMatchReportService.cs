namespace Arbiter.API.Services.Interfaces
{
    public interface IMatchReportService
    {
        Task SaveAction(int matchId, string action, string arbiterSentence);
        Task ChangeScore(int matchId, string action);
    }
}
