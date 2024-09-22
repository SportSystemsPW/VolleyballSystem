namespace ArbiterClient.Blazor.VolleballMatch.SpeechRecognition;

public class CommandType
{
    public static readonly CommandType Empty = new(string.Empty);

    public static readonly CommandType PointA = new("PointA");
    public static readonly CommandType PointB = new("PointB");

    public static readonly CommandType UndoPointA = new("UndoPointA");
    public static readonly CommandType UndoPointB = new("UndoPointB");

    public static readonly CommandType BallA = new("BallA");
    public static readonly CommandType BallB = new("BallB");

    public static readonly CommandType BreakA = new("BreakA");
    public static readonly CommandType BreakB = new("BreakB");

    private CommandType(string name)
    {
        Name = name;
    }

    public string Name { get; private set; } = string.Empty;

    public override string ToString() => Name;

    public override bool Equals(object obj)
    {
        if (obj is not CommandType commandType)
        {
            return false;
        }

        return commandType.Name.Equals(Name);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
