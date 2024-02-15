namespace IgniteSlideExpress.Domain;

public record Talk(Guid Id, string Title, string Speaker, int NumberOfSlides)
{
    private int _currentSlide;

    public bool LastSlideShown { get; private set; }

    public string NextImage()
    {
        if (!LastSlideShown)
        {
            _currentSlide++;
            if (_currentSlide == NumberOfSlides)
                LastSlideShown = true;
        }
            
        return $"{_currentSlide}.jpg";
    }
}