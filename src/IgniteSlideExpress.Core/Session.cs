using System.Text.Json.Serialization;

namespace IgniteSlideExpress.Core;

public class Session
{
    [JsonInclude] 
    public IList<Talk?> Talks { get; private set; } = new List<Talk?>();
 
    public void Add(Talk talk)
    {
        if (Talks.Any(existingTalk =>
                existingTalk != null && existingTalk.Title.Equals(talk.Title) && existingTalk.Speaker.Equals(talk.Speaker)))
            throw new Exception("No talks with the same title and speaker please");
        
        Talks.Add(talk);
    }

    public void Remove(Guid talkId)
    {
        var talk = Talks.FirstOrDefault(x => x != null && x.Id.Equals(talkId));
        Talks.Remove(talk!);
    }

    public void Update(Guid talkId, string title, string speaker)
    {
        var index = Talks.IndexOf(Talks.FirstOrDefault(x => x != null && x.Id.Equals(talkId)));
        if (index < 0)
            return;

        if (Talks.Any(existingTalk =>
                existingTalk != null &&
                !existingTalk.Id.Equals(talkId) &&
                existingTalk.Title.Equals(title) &&
                existingTalk.Speaker.Equals(speaker)))
            throw new Exception("No talks with the same title and speaker please");

        var talk = Talks[index]!;
        Talks[index] = talk with { Title = title, Speaker = speaker };
    }
    
    public void MoveUp(Guid talkId)
    {
        var index = Talks.IndexOf(Talks.FirstOrDefault(x => x != null && x.Id.Equals(talkId)));
        if (index <= 0 || index >= Talks.Count)
            return;
        
        var item = Talks[index];
        Talks.RemoveAt(index);
        Talks.Insert(index - 1, item);
    }

    public void MoveDown(Guid talkId)
    {
        var index = Talks.IndexOf(Talks.FirstOrDefault(x => x != null && x.Id.Equals(talkId)));
        if (index < 0 || index >= Talks.Count - 1) 
            return;
        
        var item = Talks[index];
        Talks.RemoveAt(index);
        Talks.Insert(index + 1, item);
    } 
}
