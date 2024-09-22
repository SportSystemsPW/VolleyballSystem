namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition.Commands;

public class PointACommand : ICommand
{
    public CommandType Type => CommandType.PointA;

    public bool IsMatch(IEnumerable<string> splittedText)
    {
        return splittedText.ContainsAll("punkt", "a");
    }
}
