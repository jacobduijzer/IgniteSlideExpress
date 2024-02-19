namespace IgniteSlideExpress.Domain;

public record Talk(Guid Id, string Title, string Speaker, int NumberOfSlides)
{
    public int CurrentSlideNumber { get; private set; } = 1;
    public string CurrentSlide => $"{CurrentSlideNumber}.jpg";


    public bool LastSlideShown => CurrentSlideNumber == NumberOfSlides;

    public void PreviousSlide()
    {
        if (CurrentSlideNumber > 0)
            CurrentSlideNumber--;
    }

    public void NextSlide()
    {
        if (LastSlideShown) return;
 
        CurrentSlideNumber++;
    }
}