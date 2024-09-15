namespace Arbiter.Contracts;

public class MatchScoreDto
{
    public int TeamASetScore { get; set; }

    public int TeamBSetScore { get; set; }

    public IEnumerable<SetScoreDto> Sets { get; set; }
}

