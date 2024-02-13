using IgniteSlideExpress.Application;
using SkiaSharp;

namespace IgniteSlideExpress.Infrastructure;

public class Pdf2Slides : IPdf2Slides
{
    public async Task<int> ProcessPdf(Guid talkId, string pdfFile)
    {
        var rootPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "presentations");
        var pdfFilePath = Path.Combine(rootPath, pdfFile);
        var slidesPath = Path.Combine(rootPath, talkId.ToString());

        if (!File.Exists(pdfFilePath))
            throw new FileNotFoundException($"The file {pdfFile} can not be found in directory {talkId}.");

        Directory.CreateDirectory(slidesPath);
        
        var numberOfSlides = 0;
        await using var inputStream = new FileStream(pdfFilePath, FileMode.Open, FileAccess.Read);
        var images = PDFtoImage.Conversion.ToImages(inputStream);
        foreach (var item in images.Select((value, i) => new { i, value }))
        {
            using SKFileWStream fs = new(Path.Combine(slidesPath, $"{item.i + 1}.jpg"));
            item.value.Encode(fs, SKEncodedImageFormat.Jpeg, quality: 100);
            numberOfSlides++;
        }

        return numberOfSlides;
    }
}