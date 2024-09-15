namespace ArbiterClient.Blazor.VolleballMatch;

public class MatchResult
{
    public bool MatchEnded { get; }
    public string WinnerName { get; }

    private MatchResult(bool matchEnded, string winnerName)
    {
        MatchEnded = matchEnded;
        WinnerName = winnerName;
    }

    public static MatchResult End(string winnerName) => new(true, winnerName);

    public static MatchResult Default() => new(default, default);
}
