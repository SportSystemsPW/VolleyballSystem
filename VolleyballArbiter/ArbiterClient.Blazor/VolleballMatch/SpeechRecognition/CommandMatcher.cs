namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition;

public class CommandMatcher : ICommandMatcher
{
    private readonly IEnumerable<ICommand> _commands;

    public CommandMatcher(IEnumerable<ICommand> commands)
    {
        _commands = commands;
    }

    public CommandResult Match(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return CommandResult.CreateFailed();
        }

        var splittedText = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (!splittedText.Any())
        {
            return CommandResult.CreateFailed();
        }

        var match = _commands.FirstOrDefault(command => command.IsMatch(splittedText));

        return match != null ? CommandResult.CreateSuccessful(match.Type) : CommandResult.CreateFailed();
    }
}
