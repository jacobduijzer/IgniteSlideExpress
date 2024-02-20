using IgniteSlideExpress.Domain;

namespace IgniteSlideExpress.UI.ViewModels;

public class PresentationViewModel : BaseViewModel, IDisposable
{
    private const int MaxDurationInMinutes = 5;
    private readonly ISessionRepository _sessionRepository;
    private readonly Domain.ITimer _timer;

    private Talk _talk;
    public Talk Talk
    {
        get => _talk;
        private set => SetValue(ref _talk, value);
    }

    private string _currentSlide;
    public string CurrentSlide
    {
        get => _currentSlide;
        private set => SetValue(ref _currentSlide, value);
    }

    private bool _playButtonDisabled = true;
    public bool PlayButtonDisabled
    {
        get => _playButtonDisabled; 
        private set => SetValue(ref _playButtonDisabled, value);
    }

    public bool StopButtonDisabled => !PlayButtonDisabled;

    private bool _previousButtonDisabled = true;

    public bool PreviousButtonDisabled
    {
        get => _previousButtonDisabled;
        private set => SetValue(ref _previousButtonDisabled, value);
    }

    private bool _nextButtonDisabled = true;
    public bool NextButtonDisabled
    {
        get => _nextButtonDisabled;
        private set => SetValue(ref _nextButtonDisabled, value);
    }
    
    public PresentationViewModel(ISessionRepository sessionRepository, Domain.ITimer timer)
    {
        _sessionRepository = sessionRepository;
        _timer = timer;
        _timer.SheetTimeElapsed += TimerOnSheetTimeElapsed;
    }

    public async Task Load(Guid talkId)
    {
        Talk = await _sessionRepository.Get(talkId);
        CurrentSlide = GetFullSlidePath();
        PlayButtonDisabled = false;
        NextButtonDisabled = false;
    }

    public void Play()
    {
        if(Talk == null)
            throw new TalkNotLoadedException("Please add a talk before starting the presentation");
        
        var intervalTiming = TimeSpan.FromSeconds(TimeSpan.FromMinutes(MaxDurationInMinutes).TotalSeconds / Talk!.NumberOfSlides).TotalMilliseconds;
        _timer.Start(intervalTiming);
        PlayButtonDisabled = true;
        UpdateButtonsState();
    }

    public void Stop()
    {
        _timer.Stop();
        PlayButtonDisabled = false; 
    }

    public void PreviousSlide()
    {
        Stop();
        _talk.PreviousSlide();
        CurrentSlide = GetFullSlidePath();

        UpdateButtonsState();
    }
    
    public void NextSlide()
    {
        Stop();
        _talk.NextSlide();
        CurrentSlide = GetFullSlidePath();

        UpdateButtonsState();
    }

    private void TimerOnSheetTimeElapsed(object? sender, EventArgs e)
    {
        _talk.NextSlide();
        CurrentSlide = GetFullSlidePath();

        if (_talk.LastSlideVisible)
            Stop();
        
        UpdateButtonsState();
    }
    
    public void Dispose()
    {
        _timer.SheetTimeElapsed -= TimerOnSheetTimeElapsed;
    }

    private void UpdateButtonsState()
    {
        PreviousButtonDisabled = _talk.FirstSlideVisible;
        NextButtonDisabled = _talk.LastSlideVisible; 
    }
    
    private string GetFullSlidePath() => Path.Combine("/", "presentations", Talk.Id.ToString(), Talk.CurrentSlide); 
}
