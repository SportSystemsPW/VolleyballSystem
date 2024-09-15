namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition.Commands;

public class BallBCommand : ICommand
{
    public CommandType Type => CommandType.BallB;

    public bool IsMatch(IEnumerable<string> splittedText)
    {
        return splittedText.Contains("piłka", StringComparer.OrdinalIgnoreCase) && splittedText.Contains("b", StringComparer.OrdinalIgnoreCase);
    }
}
