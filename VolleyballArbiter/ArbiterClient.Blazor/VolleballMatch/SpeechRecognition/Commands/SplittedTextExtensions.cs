namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition.Commands;

public static class SplittedTextExtensions
{
    public static bool ContainsAll(this IEnumerable<string> splittedText, params string[] textsToMatch)
    {
        foreach (var text in textsToMatch)
        {
            if(!splittedText.Contains(text, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        return true;
    }
}


