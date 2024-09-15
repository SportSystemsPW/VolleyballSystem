namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition.Commands;

public class UndoPointBCommand : ICommand
{
    public CommandType Type => CommandType.UndoPointB;

    public bool IsMatch(IEnumerable<string> splittedText)
    {
        return splittedText.Contains("cofnij", StringComparer.OrdinalIgnoreCase)
            && splittedText.Contains("punkt", StringComparer.OrdinalIgnoreCase)
               && splittedText.Contains("b", StringComparer.OrdinalIgnoreCase);
    }
}
