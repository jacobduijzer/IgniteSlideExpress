namespace IgniteSlideExpress.Domain;

public record Talk(Guid Id, string Title, string Speaker, int NumberOfSlides)
{
    public int CurrentSlideNumber { get; private set; } = 1;
    public string CurrentSlide => $"{CurrentSlideNumber}.jpg";

    public bool FirstSlideVisible => CurrentSlideNumber == 1;
    public bool LastSlideVisible => CurrentSlideNumber == NumberOfSlides;

    public void PreviousSlide()
    {
        if (FirstSlideVisible) return;
        
        if (CurrentSlideNumber > 1)
            CurrentSlideNumber--;
    }

    public void NextSlide()
    {
        if (LastSlideVisible) return;
 
        CurrentSlideNumber++;
    }
}