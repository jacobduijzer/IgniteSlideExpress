using IgniteSlideExpress.Domain;

namespace IgniteSlideExpress.UnitTests.Domain;

public class TalkTests
{
    [Fact]
    public void CanStepThroughImages()
    {
        // ARRANGE
        var images = new List<string?> { "1.jpg", "2.jpg", "3.jpg", "4.jpg", "5.jpg", "6.jpg", "7.jpg", "8.jpg", "9.jpg", "10.jpg" };
        Talk talk = new (Guid.NewGuid(), "Title", "Speaker", images);

        // ACT
        var image1 = talk.NextImage();
        var image2 = talk.NextImage();
        var image3 = talk.NextImage();

        //ASSERT
        Assert.Equal("1.jpg", image1);
        Assert.Equal("2.jpg", image2);
        Assert.Equal("3.jpg", image3);
    } 
}