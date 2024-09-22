namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition.Commands;

public class BreakACommand : ICommand
{
    public CommandType Type => CommandType.BreakA;

    public bool IsMatch(IEnumerable<string> splittedText)
    {
        return splittedText.ContainsAll("przerwa", "a");
    }
}
