namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition;

public class CommandResult
{
    public bool Matched { get; }
    public CommandType CommandType { get; }

    private CommandResult(bool matched, CommandType commandType)
    {
        Matched = matched;
        CommandType = commandType;
    }

    public static CommandResult CreateSuccessful(CommandType commandType) => new(true, commandType);

    public static CommandResult CreateFailed() => new(false, CommandType.Empty);
}
