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
        Assert.Single(session.Talks);
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
    public void CanUpdateTalkDetails()
    {
        Session session = new();
        Talk talk = new(Guid.NewGuid(), "Old title", "Old speaker", 20);
        session.Add(talk);

        session.Update(talk.Id, "New title", "New speaker");

        var updatedTalk = Assert.Single(session.Talks);
        Assert.Equal(talk.Id, updatedTalk!.Id);
        Assert.Equal("New title", updatedTalk.Title);
        Assert.Equal("New speaker", updatedTalk.Speaker);
        Assert.Equal(20, updatedTalk.NumberOfSlides);
    }

    [Fact]
    public void CanNotUpdateTalkAndSpeakerWithDuplicateNames()
    {
        Session session = new();
        Talk talk1 = new(Guid.NewGuid(), "Title 1", "Speaker 1", 20);
        Talk talk2 = new(Guid.NewGuid(), "Title 2", "Speaker 2", 20);
        session.Add(talk1);
        session.Add(talk2);

        Assert.Throws<Exception>(() => session.Update(talk2.Id, "Title 1", "Speaker 1"));
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
        Assert.Equal(talk2.Title, session.Talks.FirstOrDefault()!.Title);
    } 
}
