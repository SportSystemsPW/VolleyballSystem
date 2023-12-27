namespace TreningOrganizer.API
{
    public class TrainerNotAuthorizedException : Exception
    {
        public TrainerNotAuthorizedException(string message) : base(message) { }

    }
}
