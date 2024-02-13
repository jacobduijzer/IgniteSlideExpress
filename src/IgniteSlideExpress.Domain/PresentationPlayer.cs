namespace IgniteSlideExpress.Domain;

public class PresentationPlayer
{
    private readonly ITimer _timer;
    private const int MaxDurationInMinutes = 5;
    
    public event EventHandler<EventArgs>? SheetTimeElapsed;

    public Talk? Talk { get; private set; }

    public string CurrentImage { get; private set; } = string.Empty;
    
    public PresentationPlayer(ITimer timer)
    {
        _timer = timer;
    }

    public int IntervalTiming =>
        (int)TimeSpan.FromMinutes(MaxDurationInMinutes).TotalSeconds / Talk!.NumberOfSlides;

    public void Add(Talk talk) => Talk = talk;

    public void Start()
    {
        if (Talk == null)
            throw new TalkNotAddedException("Please add a talk before starting the presentation");
        
        CurrentImage = Talk!.NextImage();
        _timer.SheetTimeElapsed += TimerOnSheetTimeElapsed;
        _timer.Start(TimeSpan.FromSeconds(IntervalTiming).TotalMilliseconds);
    }

    public void Stop()
    {
        _timer.Stop();
        _timer.SheetTimeElapsed -= TimerOnSheetTimeElapsed;
    }
    
    private void TimerOnSheetTimeElapsed(object? sender, EventArgs e)
    {
        CurrentImage = Talk!.NextImage();
        SheetTimeElapsed?.Invoke(this, e);
        
        if (Talk!.LastSlideShown)
            Stop();
    } 
}