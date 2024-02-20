using IgniteSlideExpress.Core;

namespace IgniteSlideExpress.UnitTests.Application;

public class GetSessionHandlerTests
{
    [Fact]
    public async Task GetEmptySessionWhenNoSessionExists()
    {
        // ARRANGE
        GetSessionHandler handler = new(new SessionRepository());

        // ACT
        var session = await handler.Handle(new GetSessionHandler.GetSessionQuery());

        // ASSERT
        Assert.NotNull(session); 
        // TODO: remove file before test
        // Assert.Empty(session.Talks);
    }
}