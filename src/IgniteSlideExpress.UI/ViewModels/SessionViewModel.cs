using IgniteSlideExpress.Core;
using Microsoft.AspNetCore.Components.Forms;

namespace IgniteSlideExpress.UI.ViewModels;

public class SessionViewModel(
    IPdf2Slides pdf2Slides,
    UploadFileProcessor uploadFileProcessor,
    ISessionRepository sessionRepository)
    : BaseViewModel
{
    private Session _session = new();

    public Session Session
    {
        get => _session;
        private set { SetValue(ref _session, value); }
    }

    private List<IBrowserFile> _browserFiles;

    public async Task LoadSession()
    {
        Session = await sessionRepository.Load();
    }

    public void AddFiles(List<IBrowserFile> files) => _browserFiles = files;

    public async Task AddTalk(string title, string speaker)
    {
        var pdfFile = await uploadFileProcessor.Process(_browserFiles);
        var talkId = Guid.NewGuid();
        var numberOfPages = await pdf2Slides.ProcessPdf(talkId, pdfFile);
        await sessionRepository.Add(new Talk(talkId, title, speaker, numberOfPages));
        await LoadSession();
    }

    public async Task ChangeTalkPosition(Direction direction, Guid talkId)
    {
        if (direction == Direction.Up)
            await sessionRepository.MoveUp(talkId);
        else
            await sessionRepository.MoveDown(talkId);

        await LoadSession();
    }

    public async Task DeleteTalk(Guid talkId) =>
        await sessionRepository.Delete(talkId);
}