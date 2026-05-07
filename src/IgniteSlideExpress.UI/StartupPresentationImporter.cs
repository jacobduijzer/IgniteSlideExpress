using IgniteSlideExpress.Core;

namespace IgniteSlideExpress.UI;

public class StartupPresentationImporter(
    IConfiguration configuration,
    IPdf2Slides pdf2Slides,
    ISessionRepository sessionRepository,
    ILogger<StartupPresentationImporter> logger)
{
    private const string PresentationsFolderKey = "PresentationsFolder";
    private const string UnavailableTitle = "not available";

    public async Task Import()
    {
        var presentationsFolder = configuration[PresentationsFolderKey];
        if (string.IsNullOrWhiteSpace(presentationsFolder))
            return;

        if (!Directory.Exists(presentationsFolder))
        {
            logger.LogWarning("Presentations folder {PresentationsFolder} does not exist.", presentationsFolder);
            return;
        }

        var pdfFiles = Directory
            .EnumerateFiles(presentationsFolder, "*.pdf", SearchOption.TopDirectoryOnly)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (pdfFiles.Count == 0)
            return;

        Directory.CreateDirectory(GetPresentationsRoot());
        var session = await sessionRepository.Load();

        foreach (var pdfFile in pdfFiles)
        {
            var (speaker, title) = GetTalkMetadata(pdfFile);
            if (session.Talks.Any(talk => talk is not null && talk.Title == title && talk.Speaker == speaker))
                continue;

            var talkId = Guid.NewGuid();
            var importedPdfFileName = $"{talkId}.pdf";
            var importedPdfPath = Path.Combine(GetPresentationsRoot(), importedPdfFileName);

            File.Copy(pdfFile, importedPdfPath, overwrite: true);
            var numberOfSlides = await pdf2Slides.ProcessPdf(talkId, importedPdfFileName);
            var talk = new Talk(talkId, title, speaker, numberOfSlides);

            await sessionRepository.Add(talk);
            session.Add(talk);
            logger.LogInformation("Imported presentation {PresentationTitle} from {PresentationFile}.", title, pdfFile);
        }
    }

    private static string GetPresentationsRoot() =>
        Path.Combine(Environment.CurrentDirectory, "wwwroot", "presentations");

    private static (string Speaker, string Title) GetTalkMetadata(string pdfFile)
    {
        var fileName = Path.GetFileNameWithoutExtension(pdfFile);
        var separatorIndex = fileName.IndexOf('-', StringComparison.Ordinal);

        if (separatorIndex <= 0 || separatorIndex == fileName.Length - 1)
            return (fileName, UnavailableTitle);

        return (
            fileName[..separatorIndex].Trim(),
            fileName[(separatorIndex + 1)..].Trim());
    }
}
