using IgniteSlideExpress.Domain;

namespace IgniteSlideExpress.UnitTests.Domain;

public class TalkTests
{
    [Fact]
    public void CanStepForwardThroughSlides()
    {
        // ARRANGE
        var images = new List<string?> { "1.jpg", "2.jpg", "3.jpg", "4.jpg", "5.jpg", "6.jpg", "7.jpg", "8.jpg", "9.jpg", "10.jpg" };
        Talk talk = new (Guid.NewGuid(), "Title", "Speaker", images.Count());

        // ACT
        var slide1 = talk.CurrentSlide; 
        talk.NextSlide();
        var slide2 = talk.CurrentSlide;
        talk.NextSlide();
        var slide3 = talk.CurrentSlide;
        talk.NextSlide();
        var slide4 = talk.CurrentSlide;

        // ASSERT
        Assert.Equal("1.jpg", slide1);
        Assert.Equal("2.jpg", slide2);
        Assert.Equal("3.jpg", slide3);
        Assert.Equal("4.jpg", slide4);
    }
    
    [Fact]
    public void CanStepBackwardThroughSlides()
    {
        // ARRANGE
        var slides = new List<string?> { "1.jpg", "2.jpg", "3.jpg", "4.jpg", "5.jpg", "6.jpg", "7.jpg", "8.jpg", "9.jpg", "10.jpg" };
        Talk talk = new (Guid.NewGuid(), "Title", "Speaker", slides.Count);

        // ACT
        talk.NextSlide();
        talk.NextSlide();
        talk.NextSlide();
        talk.PreviousSlide();
        var slide = talk.CurrentSlide;

        // ASSERT
        Assert.Equal("3.jpg", slide);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public void SetsLastImageShowWhenAllPagesAreShown(int numberOfPages)
    {
        // ARRANGE
        Talk talk = new (Guid.NewGuid(), "Title", "Speaker", numberOfPages);
        
        // ACT
        for(var i = 0; i < numberOfPages; i++)
            talk.NextSlide();
        
        // ASSERT
        Assert.True(talk.LastSlideVisible);
    }

    [Theory]
    [InlineData(2, 1, 2)]
    [InlineData(5, 3, 4)]
    [InlineData(10, 6, 7)]
    [InlineData(10, 10, 10)]
    public void ShowsCurrentSlideNumber(int totalNumberOfSlides, int steps, int expectedCurrentSlide)
    {
        // ARRANGE
        Talk talk = new (Guid.NewGuid(), "Title", "Speaker", totalNumberOfSlides);

        // ACT
        for (var i = 0; i < steps; i++)
            talk.NextSlide();

        //ASSERT
        Assert.Equal(expectedCurrentSlide, talk.CurrentSlideNumber);
    }
}