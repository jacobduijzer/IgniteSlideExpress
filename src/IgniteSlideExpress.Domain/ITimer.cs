namespace IgniteSlideExpress.Domain;

public interface ITimer
{
    void Start(double intervalDuration);
    void Stop();
    event EventHandler<EventArgs>? SheetTimeElapsed; 
}