namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition;

public interface ICommandMatcher
{
    public CommandResult Match(string text);
}
