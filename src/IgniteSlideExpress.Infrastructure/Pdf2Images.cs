using IgniteSlideExpress.Application;
using SkiaSharp;

namespace IgniteSlideExpress.Infrastructure;

public class Pdf2Images : IPdf2Images
{
    public async Task<int> ProcessPdf(Guid talkId, string pdfFile)
    {
        var fullPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "presentations", talkId.ToString());
        var filePath = Path.Combine(fullPath, pdfFile);
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"The file {pdfFile} can not be found in directory {talkId}.");

        var numberOfPages = 0;
        await using var inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var images = PDFtoImage.Conversion.ToImages(inputStream);
        foreach (var item in images.Select((value, i) => new { i, value }))
        {
            using SKFileWStream fs = new(Path.Combine(fullPath, $"{item.i + 1}.jpg"));
            item.value.Encode(fs, SKEncodedImageFormat.Jpeg, quality: 100);
            numberOfPages++;
        }

        return numberOfPages;
    }
}