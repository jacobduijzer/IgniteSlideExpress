using IgniteSlideExpress.Domain;
using Moq;
using ITimer = IgniteSlideExpress.Domain.ITimer;

namespace IgniteSlideExpress.UnitTests.Domain;

public class PresentationPlayerTests
{
   [Fact]
    public void ThrowsAnErrorWhenStartingWithoutTalk()
    {
        // ARRANGE
        PresentationPlayer player = new(new Infrastructure.Timer());

        // ACT & ARRANGE
        Assert.Throws<TalkNotAddedException>(() => player.Start());
    }

    [Fact]
    public void CanLoadTalk()
    {
        // ARRANGE
        PresentationPlayer player = new(new Infrastructure.Timer());
        Talk talk = new(Guid.NewGuid(), "Test Title", "Test Speaker", 0);

        // ACT
        player.Add(talk);

        //ASSERT
        Assert.Equal(talk, player.Talk);
    }

    [Theory]
    [InlineData("1.jpg, 2.jpg, 3.jpg, 4.jpg", 75)]
    [InlineData("1.jpg, 2.jpg, 3.jpg, 4.jpg, 5.jpg", 60)]
    [InlineData("1.jpg, 2.jpg, 3.jpg, 4.jpg, 5.jpg, 6.jpg, 7.jpg, 8.jpg, 9.jpg, 10.jpg", 30)]
    [InlineData(
        "1.jpg, 2.jpg, 3.jpg, 4.jpg, 5.jpg, 6.jpg, 7.jpg, 8.jpg, 9.jpg, 10.jpg, 11.jpg, 12.jpg, 13.jpg, 14.jpg, 15.jpg, 16.jpg, 17.jpg, 18.jpg, 19.jpg, 20.jpg", 15)]
    public void CanCalculateTimings(string images, int intervalTiming)
    {
        // ARRANGE
        PresentationPlayer player = new(new Infrastructure.Timer());
        Talk talk = new(Guid.NewGuid(), "Test Title", "Test Speaker", images.Split(",").Count());

        // ACT
        player.Add(talk);

        //ASSERT
        Assert.Equal(intervalTiming, player.IntervalTiming);
    }

    [Fact]
    public void StartPresentationWillPresentFirstImage()
    {
        // ARRANGE
        PresentationPlayer player = new(new Infrastructure.Timer());
        Talk talk = new(Guid.NewGuid(), "Test Title", "Test Speaker", 2);
        player.Add(talk);
        
        // ACT
        player.Start();

        //ASSERT
        Assert.Equal("1.jpg", player.CurrentImage);
    }

    [Fact]
    public void StartPresentationMoveToTheNextImageWhenTimerElapses()
    {
        // ARRANGE
        var mockTimer = new Mock<ITimer>();
        mockTimer.Setup(i => i.Start(It.IsAny<double>())).Raises(i => i.SheetTimeElapsed += null, EventArgs.Empty);

        PresentationPlayer player = new(mockTimer.Object);
        Talk talk = new(Guid.NewGuid(), "Test Title", "Test Speaker", 2);
        player.Add(talk);

        // ACT
        var raisedEvent = Assert.RaisesAny<EventArgs>(
            h => player.SheetTimeElapsed += h,
            h => player.SheetTimeElapsed -= h,
            () => player.Start());

        // ASSERT
        Assert.NotNull(raisedEvent);
        Assert.Equal(player, raisedEvent.Sender);
        Assert.Equal("2.jpg", player.CurrentImage);
    }

    [Fact]
    public void StartingWillSubscribeToTimer()
    {
        // ARRANGE
        var mockTimer = new Mock<ITimer>();
        mockTimer.Setup(i => i.Start(It.IsAny<double>())).Raises(i => i.SheetTimeElapsed += null, EventArgs.Empty);
        PresentationPlayer player = new(mockTimer.Object);
        Talk talk = new(Guid.NewGuid(), "Test Title", "Test Speaker", 2);
        player.Add(talk);
        
        // ACT
        player.Start();

        //ASSERT
        mockTimer.VerifyAdd(m => m.SheetTimeElapsed += It.IsAny<EventHandler<EventArgs>>(), Times.Exactly(1));
    }

    [Fact]
    public void StoppingWillUnsubscribeFromTimer()
    {
        // ARRANGE
        var mockTimer = new Mock<ITimer>();
        
        PresentationPlayer player = new(mockTimer.Object);
        Talk talk = new(Guid.NewGuid(), "Test Title", "Test Speaker", 2);
        player.Add(talk);
        player.Start();
        
        // ACT
        player.Stop();

        //ASSERT
        mockTimer.VerifyRemove(m => m.SheetTimeElapsed -= It.IsAny<EventHandler<EventArgs>>(), Times.Exactly(1));
    } 
}