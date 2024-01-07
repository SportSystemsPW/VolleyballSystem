namespace Arbiter.API.Services.Interfaces
{
    public interface ITextAnalyzerService
    {
        Task AnalyzeSentence(int matchId, string sentence);
    }
}
