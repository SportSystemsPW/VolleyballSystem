namespace Arbiter.Contracts;

public class MatchDto
{
    public TeamDto TeamA { get; set; }
    public TeamDto TeamB { get; set; }
    public MatchScoreDto Score { get; set; }
    public bool Finished { get; set; }
}

