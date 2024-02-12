namespace IgniteSlideExpress.Domain;

public record Talk(Guid Id, string Title, string Speaker, int NumberOfPages)
{
    private int _currentImage;

    public bool LastPageShown { get; private set; }

    public string? NextImage()
    {
        if (!LastPageShown)
        {
            _currentImage++;
            if (_currentImage == NumberOfPages)
                LastPageShown = true;
        }
            
        return $"{_currentImage}.jpg";
    }
}