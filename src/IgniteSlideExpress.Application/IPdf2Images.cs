namespace IgniteSlideExpress.Application;

public interface IPdf2Images
{
    Task<int> ProcessPdf(Guid talkId, string pdfFile);
}