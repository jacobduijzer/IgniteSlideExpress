using IgniteSlideExpress.Domain;
using Microsoft.AspNetCore.Components.Forms;

namespace IgniteSlideExpress.Application;

public class CreateTalkHandler
{
    public record CreateTalkCommand(string Title, string Speaker, List<IBrowserFile> Files);

    private readonly IPdf2Images _pdf2Images;
    private readonly ISessionRepository _sessionRepository;

    public CreateTalkHandler(IPdf2Images pdf2Images, ISessionRepository sessionRepository)
    {
        _pdf2Images = pdf2Images;
        _sessionRepository = sessionRepository;
    }

    public async Task Handle(CreateTalkCommand command)
    {
        var talkId = Guid.NewGuid();
        Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "wwwroot", "presentations", talkId.ToString()));
        var file = command.Files.FirstOrDefault();
        var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", "presentations", talkId.ToString(), file.Name);

        await using FileStream fs = new(path, FileMode.Create);
        await file.OpenReadStream(1 * 1024 * 1024 * 1024).CopyToAsync(fs);

        var numberOfPages = await _pdf2Images.ProcessPdf(talkId, file.Name);
        await _sessionRepository.Add(new Talk(talkId, command.Title, command.Speaker, numberOfPages));
    }
}