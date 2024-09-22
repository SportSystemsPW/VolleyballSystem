using Arbiter.Contracts;
using ArbiterClient.Blazor.VolleballMatch;
using static ArbiterClient.Blazor.Components.SetsResult;

namespace ArbiterClient.Blazor.Match;

public class MatchState
{
    public int CurrentSet { get; private set; }
    public bool Active { get; private set; }
    public string TeamAName { get; private set; }
    public string TeamBName { get; private set; }
    public bool TeamAHasBall { get; private set; }
    public bool TeamBHasBall => !TeamAHasBall;
    public int[] TeamAScores { get; private set; }
    public int[] TeamBScores { get; private set; }

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
        TeamAScores = teamAScores;
        TeamBScores = teamBScores;
        _teamASetWins = teamASetWins;
        _teamBSetWins = teamBSetWins;
    }

    public IList<TeamScoreEntry> TeamScores =>
        [
            new TeamScoreEntry
            {
                TeamName = TeamAName,
                SetsScore = TeamASetScore,
                Set1Score = TeamAScores[0],
                Set2Score = TeamAScores[1],
                Set3Score = TeamAScores[2],
                Set4Score = TeamAScores[3],
                Set5Score = TeamAScores[4],
            },
            new TeamScoreEntry
            {
                TeamName = TeamBName,
                SetsScore = TeamBSetScore,
                Set1Score = TeamBScores[0],
                Set2Score = TeamBScores[1],
                Set3Score = TeamBScores[2],
                Set4Score = TeamBScores[3],
                Set5Score = TeamBScores[4],
            }
        ];

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

    public int TeamAScore => TeamAScores[CurrentSet - 1];

    public int TeamBScore => TeamBScores[CurrentSet - 1];

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

        TeamAScores[CurrentSet - 1] += points;

        return CheckSetWin();
    }

    public MatchResult PointTeamB(int points)
    {
        if (!Active)
        {
            return MatchResult.Default();
        }

        TeamBScores[CurrentSet - 1] += points;

        return CheckSetWin();
    }

    private MatchResult CheckSetWin()
    {
        if (TeamAScores[CurrentSet - 1] >= CommonConsts.PointsToWinSet && TeamAScores[CurrentSet - 1] - TeamBScores[CurrentSet - 1] >= 2)
        {
            _teamASetWins++;
            Console.WriteLine($"{TeamAName} wins set {CurrentSet}.");
            NextSet();
        }
        else if (TeamBScores[CurrentSet - 1] >= CommonConsts.PointsToWinSet && TeamBScores[CurrentSet - 1] - TeamAScores[CurrentSet - 1] >= 2)
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

