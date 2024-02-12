using IgniteSlideExpress.Domain;

namespace IgniteSlideExpress.UnitTests.Domain;

public class SessionTests
{
    [Fact]
    public void CanAddTalkToSession()
    {
        // ARRANGE
        Session session = new();

        // ACT2
        session.Add(new Talk(Guid.NewGuid(),"Talk 1", "Speaker 1", new List<string>()));

        // ASSERT
        Assert.Equal(1, session.Talks.Count);
    }

    [Fact]
    public void CanNotAddTalkAndSpeakerWithDuplicateNames()
    {
        // ARRANGE
        Session session = new();
        session.Add(new Talk(Guid.NewGuid(), "Title", "Speaker", new List<string>()));
        
        // ACT & ASSERT
        Assert.Throws<Exception>(() => session.Add(new Talk(Guid.NewGuid(), "Title", "Speaker", new List<string>())));
    }
    
    [Fact]
    public void CanRemoveTalksFromSession()
    {
        // ARRANGE
        Session session = new();
        Talk talk1  = new (Guid.NewGuid(), "Talk 1", "Speaker 1", new List<string>());
        Talk talk2  = new (Guid.NewGuid(), "Talk 2", "Speaker 2", new List<string>());
        session.Add(talk1);
        session.Add(talk2);
        
        // ACT
        session.Remove(talk1.Id);
        
        // ASSERT
        Assert.DoesNotContain(session.Talks, talk => talk == talk1);
    }
    
    [Fact]
    public void CanChangePosition()
    {
        // ARRANGE
        Session session = new();
        Talk talk1  = new (Guid.NewGuid(), "Talk 1", "Speaker 1", new List<string>());
        Talk talk2  = new (Guid.NewGuid(),"Talk 2", "Speaker 2", new List<string>());
        session.Add(talk1);
        session.Add(talk2);
        
        // ACT
        session.MoveUp(talk2.Id);
        
        // ASSERT
        Assert.True(session.Talks.FirstOrDefault()!.Title.Equals(talk2.Title));
    } 
}