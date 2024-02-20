using IgniteSlideExpress.Core;
using Microsoft.AspNetCore.Components.Forms;

namespace IgniteSlideExpress.UI.ViewModels;

public class SessionViewModel : BaseViewModel
{
    private readonly IPdf2Slides _pdf2Slides;
    private readonly UploadFileProcessor _uploadFileProcessor;
    private readonly ISessionRepository _sessionRepository;

    private Session _session;

    public Session Session
    {
        get => _session;
        private set { SetValue(ref _session, value); }
    }

    private List<IBrowserFile> _browserFiles;

    public SessionViewModel(IPdf2Slides pdf2Slides, UploadFileProcessor uploadFileProcessor,
        ISessionRepository sessionRepository)
    {
        _pdf2Slides = pdf2Slides;
        _uploadFileProcessor = uploadFileProcessor;
        _sessionRepository = sessionRepository;
    }

    public async Task LoadSession()
    {
        Session = await _sessionRepository.Load();
    }

    public void AddFiles(List<IBrowserFile> files) => _browserFiles = files;

    public async Task AddTalk(string title, string speaker)
    {
        var pdfFile = await _uploadFileProcessor.Process(_browserFiles);
        var talkId = Guid.NewGuid();
        var numberOfPages = await _pdf2Slides.ProcessPdf(talkId, pdfFile);
        await _sessionRepository.Add(new Talk(talkId, title, speaker, numberOfPages));
        await LoadSession();
    }

    public async Task ChangeTalkPosition(Direction direction, Guid talkId)
    {
        if (direction == Direction.Up)
            await _sessionRepository.MoveUp(talkId);
        else
            await _sessionRepository.MoveDown(talkId);

        await LoadSession();
    }

    public async Task DeleteTalk(Guid talkId) =>
        await _sessionRepository.Delete(talkId);
}