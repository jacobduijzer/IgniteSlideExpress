namespace IgniteSlideExpress.Core;

public class RemoveTalkHandler
{
    private readonly ISessionRepository _sessionRepository;

    public record RemoveTalkCommand(Guid TalkId);

    public RemoveTalkHandler(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task Handle(RemoveTalkCommand command) =>
        await _sessionRepository.Remove(command.TalkId);
}