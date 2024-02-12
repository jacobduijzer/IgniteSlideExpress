namespace IgniteSlideExpress.Domain;

public record Talk(Guid Id, string Title, string Speaker, List<string?> Images)
{
    private string? _currentImage;
    public string? NextImage()
    {
        _currentImage = _currentImage == null 
            ? Images.First() 
            : Images
                .SkipWhile(item => item != _currentImage)
                .Skip(1)
                .FirstOrDefault();

        return _currentImage;
    }
}