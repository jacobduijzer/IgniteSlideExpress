using Microsoft.AspNetCore.Components.Forms;

namespace IgniteSlideExpress.UI;

public class UploadFileProcessor 
{
    private readonly List<IBrowserFile> _files;

    public UploadFileProcessor(List<IBrowserFile> files)
    {
        _files = files;
    }

    public async Task<string> Process()
    {
        if (_files == null || !_files.Any())
            throw new Exception("Add upload files first");

        var fullPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "presentations");

        Directory.CreateDirectory(fullPath);
        var file = _files.FirstOrDefault();
        var path = Path.Combine(fullPath, file!.Name);

        await using FileStream fs = new(path, FileMode.Create);
        await file.OpenReadStream(1 * 1024 * 1024 * 1024).CopyToAsync(fs);

        return file.Name;
    }
}