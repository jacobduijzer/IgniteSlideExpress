using IgniteSlideExpress.Core;
using IgniteSlideExpress.UI;
using IgniteSlideExpress.UI.ViewModels;
using Moq;

namespace IgniteSlideExpress.UnitTests.UI.ViewModels;

public class SessionViewModelTests
{
    [Fact]
    public async Task DeleteTalkReloadsSession()
    {
        var talk = new Talk(Guid.NewGuid(), "Title", "Speaker", 20);
        var initialSession = new Session();
        initialSession.Add(talk);
        var updatedSession = new Session();
        var loadedSessions = new Queue<Session>([initialSession, updatedSession]);

        var sessionRepository = new Mock<ISessionRepository>();
        sessionRepository.Setup(x => x.Load()).ReturnsAsync(loadedSessions.Dequeue);

        var viewModel = new SessionViewModel(
            Mock.Of<IPdf2Slides>(),
            new UploadFileProcessor(),
            sessionRepository.Object);
        await viewModel.LoadSession();
        var propertyChangedCount = 0;
        viewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(SessionViewModel.Session))
                propertyChangedCount++;
        };

        await viewModel.DeleteTalk(talk.Id);

        sessionRepository.Verify(x => x.Delete(talk.Id), Times.Once);
        Assert.Same(updatedSession, viewModel.Session);
        Assert.Equal(1, propertyChangedCount);
    }
}
