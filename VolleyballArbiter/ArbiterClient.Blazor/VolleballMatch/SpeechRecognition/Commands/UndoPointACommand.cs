namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition.Commands;

public class UndoPointACommand : ICommand
{
    public CommandType Type => CommandType.UndoPointA;

    public bool IsMatch(IEnumerable<string> splittedText)
    {
        return splittedText.ContainsAll("cofnij", "punkt", "a");
    }
}
