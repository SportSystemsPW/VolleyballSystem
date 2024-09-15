using Arbiter.Contracts;
using ArbiterClient.Blazor.VolleballMatch;

namespace ArbiterClient.Blazor.Match;

public class MatchState
{
    public int CurrentSet { get; private set; }
    public bool Active { get; private set; }
    public string TeamAName { get; private set; }
    public string TeamBName { get; private set; }
    public bool TeamAHasBall { get; private set; }
    public bool TeamBHasBall => !TeamAHasBall;

    private int[] _teamAScores;
    private int[] _teamBScores;

    private int _teamASetWins;
    private int _teamBSetWins;

    public static MatchState FromDto(MatchDto matchDto)
    {
        var teamAScores = new int[5];
        var teamBScores = new int[5];

        foreach (var item in matchDto.Score.Sets)
        {
            teamAScores[item.SetNumber - 1] = item.TeamAScore;
        }

        foreach (var item in matchDto.Score.Sets)
        {
            teamBScores[item.SetNumber - 1] = item.TeamBScore;
        }

        return new MatchState(
            matchDto.TeamA.Name,
            matchDto.TeamB.Name,
            teamAScores,
            teamBScores,
            matchDto.Score.Sets.Count(),
            false,
            matchDto.Score.TeamASetScore,
            matchDto.Score.TeamBSetScore);
    }

    private MatchState(string teamAName,
        string teamBName,
        int[] teamAScores,
        int[] teamBScores,
        int currentSet,
        bool active,
        int teamASetWins,
        int teamBSetWins)
    {
        TeamAName = teamAName;
        TeamBName = teamBName;
        CurrentSet = 1;
        Active = false;
        _teamAScores = teamAScores;
        _teamBScores = teamBScores;
        _teamASetWins = teamASetWins;
        _teamBSetWins = teamBSetWins;
    }

    public void Start()
    {
        if (Active)
        {
            throw new InvalidOperationException("Match is already active.");
        }

        Active = true;
        Console.WriteLine($"Match between {TeamAName} and {TeamBName} has started.");
    }

    public void End()
    {
        Active = false;
    }

    public int TeamAScore => _teamAScores[CurrentSet - 1];

    public int TeamBScore => _teamBScores[CurrentSet - 1];

    public int TeamASetScore => _teamASetWins;

    public int TeamBSetScore => _teamBSetWins;


    public void BallA()
    {
        TeamAHasBall = true;
    }

    public void BallB()
    {
        TeamAHasBall = false;
    }

    public MatchResult PointTeamA()
    {
        BallA();
        return PointTeamA(1);
    }

    public MatchResult PointTeamB()
    {
        BallB();
        return PointTeamB(1);
    }

    public MatchResult PointTeamA(int points)
    {
        if (!Active)
        {
            return MatchResult.Default();
        }

        _teamAScores[CurrentSet - 1] += points;

        return CheckSetWin();
    }

    public MatchResult PointTeamB(int points)
    {
        if (!Active)
        {
            return MatchResult.Default();
        }

        _teamBScores[CurrentSet - 1] += points;

        return CheckSetWin();
    }

    private MatchResult CheckSetWin()
    {
        if (_teamAScores[CurrentSet - 1] >= CommonConsts.PointsToWinSet && _teamAScores[CurrentSet - 1] - _teamBScores[CurrentSet - 1] >= 2)
        {
            _teamASetWins++;
            Console.WriteLine($"{TeamAName} wins set {CurrentSet}.");
            NextSet();
        }
        else if (_teamBScores[CurrentSet - 1] >= CommonConsts.PointsToWinSet && _teamBScores[CurrentSet - 1] - _teamAScores[CurrentSet - 1] >= 2)
        {
            _teamBSetWins++;
            Console.WriteLine($"{TeamBName} wins set {CurrentSet}.");
            NextSet();
        }

        if (_teamASetWins == CommonConsts.WinningSets)
        {
            return MatchResult.End(TeamAName);
        }

        if (_teamBSetWins == CommonConsts.WinningSets)
        {
            return MatchResult.End(TeamBName);
        }

        return MatchResult.Default();
    }

    private void NextSet()
    {
        if (_teamASetWins < CommonConsts.WinningSets && _teamBSetWins < CommonConsts.WinningSets)
        {
            CurrentSet++;
            Console.WriteLine($"Starting set {CurrentSet}.");
        }
    }
}

