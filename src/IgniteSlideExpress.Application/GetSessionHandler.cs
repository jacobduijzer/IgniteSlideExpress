using IgniteSlideExpress.Domain;

namespace IgniteSlideExpress.Application;

public class GetSessionHandler
{
    private readonly ISessionRepository _sessionRepository;

    public record GetSessionCommand();
    
    public GetSessionHandler(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Session> Handle(GetSessionCommand command)
    {
        return await _sessionRepository.Load();
    }
}