using IgniteSlideExpress.Core;

namespace IgniteSlideExpress.UnitTests.Domain;

public class SessionTests
{
    [Fact]
    public void CanAddTalkToSession()
    {
        // ARRANGE
        Session session = new();

        // ACT2
        session.Add(new Talk(Guid.NewGuid(),"Talk 1", "Speaker 1", 0));

        // ASSERT
        Assert.Equal(1, session.Talks.Count);
    }

    [Fact]
    public void CanNotAddTalkAndSpeakerWithDuplicateNames()
    {
        // ARRANGE
        Session session = new();
        session.Add(new Talk(Guid.NewGuid(), "Title", "Speaker", 0));
        
        // ACT & ASSERT
        Assert.Throws<Exception>(() => session.Add(new Talk(Guid.NewGuid(), "Title", "Speaker", 0)));
    }
    
    [Fact]
    public void CanRemoveTalksFromSession()
    {
        // ARRANGE
        Session session = new();
        Talk talk1  = new (Guid.NewGuid(), "Talk 1", "Speaker 1", 0);
        Talk talk2  = new (Guid.NewGuid(), "Talk 2", "Speaker 2", 0);
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
        Talk talk1  = new (Guid.NewGuid(), "Talk 1", "Speaker 1", 0);
        Talk talk2  = new (Guid.NewGuid(),"Talk 2", "Speaker 2", 0);
        session.Add(talk1);
        session.Add(talk2);
        
        // ACT
        session.MoveUp(talk2.Id);
        
        // ASSERT
        Assert.True(session.Talks.FirstOrDefault()!.Title.Equals(talk2.Title));
    } 
}