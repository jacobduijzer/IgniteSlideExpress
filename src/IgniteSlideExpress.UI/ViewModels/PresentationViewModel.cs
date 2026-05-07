using IgniteSlideExpress.Core;
using System.Diagnostics.CodeAnalysis;
using ITimer = IgniteSlideExpress.Core.ITimer;

namespace IgniteSlideExpress.UI.ViewModels;

public class PresentationViewModel : BaseViewModel, IDisposable
{
    private readonly int _maxDurationInMinutes = 5;
    private readonly IConfiguration _configuration;
    private readonly ISessionRepository _sessionRepository;
    private readonly ITimer _timer;

    private Talk? _talk;
    public Talk? Talk
    {
        get => _talk;
        private set => SetValue(ref _talk, value);
    }

    private string _currentSlide = string.Empty;
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

    public TimeSpan SlideDuration =>
        Talk is { NumberOfSlides: > 0 }
            ? TimeSpan.FromSeconds(TimeSpan.FromMinutes(_maxDurationInMinutes).TotalSeconds / Talk.NumberOfSlides)
            : TimeSpan.Zero;

    public TimeSpan PresentationDuration => TimeSpan.FromMinutes(_maxDurationInMinutes);
    
    public PresentationViewModel(IConfiguration configuration, ISessionRepository sessionRepository, ITimer timer)
    {
        _configuration = configuration;
        _sessionRepository = sessionRepository;
        _timer = timer;
        _timer.SheetTimeElapsed += TimerOnSheetTimeElapsed;

        if (int.TryParse(_configuration["DurationInMinutes"], out var duration))
            _maxDurationInMinutes = duration;
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
        if (Talk is null)
            throw new TalkNotLoadedException("Please add a talk before starting the presentation");
        
        _timer.Start(SlideDuration.TotalMilliseconds);
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
        EnsureTalkLoaded();
        var wasPlaying = PlayButtonDisabled;
        _timer.Stop();
        _talk!.PreviousSlide();
        CurrentSlide = GetFullSlidePath();

        if (wasPlaying)
            _timer.Start(SlideDuration.TotalMilliseconds);

        UpdateButtonsState();
    }
    
    public void NextSlide()
    {
        EnsureTalkLoaded();
        var wasPlaying = PlayButtonDisabled;
        _timer.Stop();
        _talk!.NextSlide();
        CurrentSlide = GetFullSlidePath();

        if (wasPlaying)
            _timer.Start(SlideDuration.TotalMilliseconds);

        UpdateButtonsState();
    }

    private void TimerOnSheetTimeElapsed(object? sender, EventArgs e)
    {
        EnsureTalkLoaded();

        if (_talk.LastSlideVisible)
        {
            Stop();
            UpdateButtonsState();
            return;
        }

        _talk!.NextSlide();
        CurrentSlide = GetFullSlidePath();
        
        UpdateButtonsState();
    }
    
    public void Dispose()
    {
        _timer.SheetTimeElapsed -= TimerOnSheetTimeElapsed;
    }

    private void UpdateButtonsState()
    {
        EnsureTalkLoaded();
        PreviousButtonDisabled = _talk!.FirstSlideVisible;
        NextButtonDisabled = _talk.LastSlideVisible; 
    }
    
    private string GetFullSlidePath()
    {
        EnsureTalkLoaded();
        return Path.Combine("/", "presentations", _talk!.Id.ToString(), _talk.CurrentSlide);
    }

    [MemberNotNull(nameof(_talk))]
    private void EnsureTalkLoaded()
    {
        if (_talk is null)
            throw new TalkNotLoadedException("Please add a talk before starting the presentation");
    }
}
