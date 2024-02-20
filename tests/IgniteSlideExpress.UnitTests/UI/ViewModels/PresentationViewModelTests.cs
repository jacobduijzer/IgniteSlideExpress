using IgniteSlideExpress.Core;
using IgniteSlideExpress.UI.ViewModels;
using Moq;
using ITimer = System.Threading.ITimer;

namespace IgniteSlideExpress.UnitTests.UI.ViewModels;

public class PresentationViewModelTests
{
    private readonly Talk _testTalk = new Talk(Guid.NewGuid(), "Test Title", "Test Speaker", 2);
    
    [Fact]
    public async Task CanLoadTalk()
    {
        // ARRANGE
        PresentationViewModel viewModel = CreateDefaultViewModel();

        // ACT
        await viewModel.Load(_testTalk.Id);

        //ASSERT
        Assert.Equal(_testTalk, viewModel.Talk);
    }

    [Fact]
    public void ThrowsAnExceptionWhenPlayingWithoutLoadingATalk()
    {
        // ARRANGE
        PresentationViewModel viewModel = CreateDefaultViewModel();

        // ACT & ASSERT
        Assert.Throws<TalkNotLoadedException>(() => viewModel.Play());
    }
    
    [Fact]
    public async Task LoadingTalkEnablesPlayButton()
    {
        // ARRANGE
        PresentationViewModel viewModel = CreateDefaultViewModel();
        var fireCount = 0;
        viewModel.PropertyChanged += (s, e) => ++fireCount;

        // ACT
        await viewModel.Load(_testTalk.Id); 

        // ASSERT
        Assert.False(viewModel.PlayButtonDisabled);
        Assert.True(fireCount == 4);
    }

    [Fact]
    public async Task PlayTalkDisablesStartButton()
    {
        // ARRANGE
        PresentationViewModel viewModel = CreateDefaultViewModel();
        await viewModel.Load(_testTalk.Id);

        // ACT
        viewModel.Play();

        // ASSERT
        Assert.True(viewModel.PlayButtonDisabled);
    }
    
    [Fact]
    public async Task StopTalkEnablesStartButton()
    {
        // ARRANGE
        PresentationViewModel viewModel = CreateDefaultViewModel();
        await viewModel.Load(_testTalk.Id);
        viewModel.Play();
        
        // ACT
        viewModel.Stop();

        // ASSERT
        Assert.False(viewModel.PlayButtonDisabled);
    }
    
    private PresentationViewModel CreateDefaultViewModel()
    {
        var mockSessionRepository = new Mock<ISessionRepository>();
        mockSessionRepository.Setup(x => x.Get(_testTalk.Id)).ReturnsAsync(_testTalk);

        var mockTimer = new Mock<IgniteSlideExpress.Core.ITimer>();
        return new PresentationViewModel(mockSessionRepository.Object, mockTimer.Object);
    }
}