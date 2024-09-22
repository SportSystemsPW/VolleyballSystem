namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition.Commands;

public class BreakBCommand : ICommand
{
    public CommandType Type => CommandType.BreakB;

    public bool IsMatch(IEnumerable<string> splittedText)
    {
        return splittedText.ContainsAll("koniec", "przerwy", "technicznej");
    }
}