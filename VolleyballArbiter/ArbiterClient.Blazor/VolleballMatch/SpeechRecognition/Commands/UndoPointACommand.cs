namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition.Commands;

public class UndoPointACommand : ICommand
{
    public CommandType Type => CommandType.UndoPointA;

    public bool IsMatch(IEnumerable<string> splittedText)
    {
        return splittedText.Contains("cofnij", StringComparer.OrdinalIgnoreCase)
            && splittedText.Contains("punkt", StringComparer.OrdinalIgnoreCase)
               && splittedText.Contains("a", StringComparer.OrdinalIgnoreCase);
    }
}
