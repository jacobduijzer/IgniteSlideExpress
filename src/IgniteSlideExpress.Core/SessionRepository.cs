using System.Text.Json;

namespace IgniteSlideExpress.Core;

public class SessionRepository : ISessionRepository
{
    private const string DataFile = "sessionData.json";
    
    public async Task<Session> Load()
    {
        return await Create();
    }

    public async Task<Talk> Get(Guid talkId)
    {
        var sessions = await Load();
        return sessions.Talks.First(talk => talk != null && talk.Id.Equals(talkId))!;
    }

    public async Task Add(Talk talk)
    {
        var session = await Create();
        session!.Add(talk);
        await Save(session);
    }

    public async Task Delete(Guid talkId)
    {
        var session = await Create();
        session!.Remove(talkId);
        await Save(session);
    }

    public async Task MoveUp(Guid talkId)
    {
        var session = await Create();
        session!.MoveUp(talkId);
        await Save(session);
    }

    public async Task MoveDown(Guid talkId)
    {
        var session = await Create();
        session!.MoveDown(talkId);
        await Save(session);
    }

    private async Task<Session> Create()
    {
        var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        if (!File.Exists(Path.Combine(path, DataFile)))
            await Save(new Session());
        
        using StreamReader reader = new(Path.Combine(path, DataFile));
        var json = await reader.ReadToEndAsync();
        var session = JsonSerializer.Deserialize<Session>(json);
        return session!;
    }

    private static async Task Save(Session session)
    {
        var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        await using var writer = new StreamWriter(Path.Combine(path, DataFile));
        await writer.WriteAsync(JsonSerializer.Serialize(session));
    }
}