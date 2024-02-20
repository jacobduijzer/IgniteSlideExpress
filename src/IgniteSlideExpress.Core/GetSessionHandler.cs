namespace IgniteSlideExpress.Core;

public class GetSessionHandler
{
    private readonly ISessionRepository _sessionRepository;

    public record GetSessionQuery();
    
    public GetSessionHandler(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Session> Handle(GetSessionQuery query)
    {
        return await _sessionRepository.Load();
    }
}