namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition.Commands;

public class PointBCommand : ICommand
{
    public CommandType Type => CommandType.PointB;

    public bool IsMatch(IEnumerable<string> splittedText)
    {
        return splittedText.ContainsAll("punkt", "b");
    }
}
