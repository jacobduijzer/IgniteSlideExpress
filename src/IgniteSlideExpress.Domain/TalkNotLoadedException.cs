namespace IgniteSlideExpress.Domain;

public class TalkNotLoadedException : Exception
{
    public TalkNotLoadedException(string message) : base(message)
    {
    }
}