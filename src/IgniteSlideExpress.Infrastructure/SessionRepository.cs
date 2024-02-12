using System.Text.Json;
using IgniteSlideExpress.Domain;

namespace IgniteSlideExpress.Infrastructure;

public class SessionRepository : ISessionRepository
{
    private const string DataFile = "sessionData.json";
    
    public async Task<Session?> Load()
    {
        return await Create();
    }

    public Task AddFiles<T>(Talk talk, List<T> files) 
    {
        throw new NotImplementedException();
    }

    public async Task Add(Talk talk)
    {
        var session = await Create();
        session!.Add(talk);
        await Save(session);
    }

    public async Task Remove(Guid talkId)
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

    private async Task<Session?> Create()
    {
        if (!File.Exists(DataFile))
            await Save(new Session());
        
        using StreamReader reader = new(DataFile);
        var json = await reader.ReadToEndAsync();
        var session = JsonSerializer.Deserialize<Session>(json);
        return session;
    }

    private static async Task Save(Session session)
    {
        await using var writer = new StreamWriter(DataFile);
        await writer.WriteAsync(JsonSerializer.Serialize(session));
    }
}