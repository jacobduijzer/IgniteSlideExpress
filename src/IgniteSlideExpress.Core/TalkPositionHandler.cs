namespace IgniteSlideExpress.Core;

public class TalkPositionHandler
{
    private readonly ISessionRepository _sessionRepository;

    public record TalkPositionCommand(Guid TalkId, Direction Direction);

    public TalkPositionHandler(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task Handle(TalkPositionCommand command)
    {
        if (command.Direction == Direction.Up)
            await _sessionRepository.MoveUp(command.TalkId);
        else
            await _sessionRepository.MoveDown(command.TalkId);
    }

    public enum Direction
    {
        Up,
        Down
    }
}