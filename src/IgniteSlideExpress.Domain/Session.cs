using System.Text.Json.Serialization;

namespace IgniteSlideExpress.Domain;

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