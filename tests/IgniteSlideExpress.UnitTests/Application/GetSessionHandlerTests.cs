using IgniteSlideExpress.Application;
using IgniteSlideExpress.Infrastructure;

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
        Assert.Empty(session.Talks);
    }
}