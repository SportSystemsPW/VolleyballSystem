namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition;

public interface ICommand
{
    CommandType Type { get; }

    bool IsMatch(IEnumerable<string> splittedText);
}
