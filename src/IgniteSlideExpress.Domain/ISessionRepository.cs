namespace IgniteSlideExpress.Domain;

public interface ISessionRepository
{
    Task<Session> Load();

    Task<Talk> Get(Guid talkId);
    
    Task Add(Talk talk);

    Task Remove(Guid talkId);

    Task MoveUp(Guid talkId);

    Task MoveDown(Guid talkId); 
}