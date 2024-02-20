namespace IgniteSlideExpress.Core;

public interface IPdf2Slides
{
    Task<int> ProcessPdf(Guid talkId, string pdfFile);
}