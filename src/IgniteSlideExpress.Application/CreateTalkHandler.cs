using IgniteSlideExpress.Domain;

namespace IgniteSlideExpress.Application;

public class CreateTalkHandler
{
    public record CreateTalkCommand(string Title, string Speaker, string PdfFile);

    private readonly IPdf2Slides _pdf2Slides;
    private readonly ISessionRepository _sessionRepository;

    public CreateTalkHandler(IPdf2Slides pdf2Slides, ISessionRepository sessionRepository)
    {
        _pdf2Slides = pdf2Slides;
        _sessionRepository = sessionRepository;
    }

    public async Task Handle(CreateTalkCommand command)
    {
        var talkId = Guid.NewGuid();
        var numberOfPages = await _pdf2Slides.ProcessPdf(talkId, command.PdfFile);
        await _sessionRepository.Add(new Talk(talkId, command.Title, command.Speaker, numberOfPages));
    }
}