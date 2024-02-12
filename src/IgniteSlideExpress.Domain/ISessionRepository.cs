namespace IgniteSlideExpress.Domain;

public interface ISessionRepository
{
    Task<Session> Load();

    Task AddFiles<T>(Talk talk, List<T> files);
   
    Task Add(Talk talk);

    Task Remove(Guid talkId);

    Task MoveUp(Guid talkId);

    Task MoveDown(Guid talkId); 
}