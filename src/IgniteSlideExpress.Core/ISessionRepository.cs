namespace IgniteSlideExpress.Core;

public interface ISessionRepository
{
    Task<Session> Load();

    Task<Talk> Get(Guid talkId);
    
    Task Add(Talk talk);

    Task Update(Guid talkId, string title, string speaker);

    Task Delete(Guid talkId);

    Task MoveUp(Guid talkId);

    Task MoveDown(Guid talkId); 
}
