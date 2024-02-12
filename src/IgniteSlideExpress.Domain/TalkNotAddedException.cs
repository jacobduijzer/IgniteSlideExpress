namespace IgniteSlideExpress.Domain;

public class TalkNotAddedException : Exception
{
    public TalkNotAddedException(string message) : base(message)
    {
    }
}