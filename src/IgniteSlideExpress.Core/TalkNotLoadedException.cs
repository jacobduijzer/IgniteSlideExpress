namespace IgniteSlideExpress.Core;

public class TalkNotLoadedException : Exception
{
    public TalkNotLoadedException(string message) : base(message)
    {
    }
}