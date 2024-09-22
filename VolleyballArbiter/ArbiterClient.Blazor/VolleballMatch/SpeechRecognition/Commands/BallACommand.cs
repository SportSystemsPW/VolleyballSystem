namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition.Commands;

public class BallACommand : ICommand
{
    public CommandType Type => CommandType.BallA;

    public bool IsMatch(IEnumerable<string> splittedText)
    {
        return splittedText.ContainsAll("piłka", "a");
    }
}