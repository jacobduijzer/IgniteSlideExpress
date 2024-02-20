using Microsoft.AspNetCore.Components.Forms;

namespace IgniteSlideExpress.UI;

public class UploadFileProcessor 
{
    public async Task<string> Process(List<IBrowserFile> files)
    {
        if (files == null || !files.Any())
            throw new Exception("Add upload files first");

        var fullPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "presentations");

        Directory.CreateDirectory(fullPath);
        var file = files.FirstOrDefault();
        var path = Path.Combine(fullPath, file!.Name);

        await using FileStream fs = new(path, FileMode.Create);
        await file.OpenReadStream(1 * 1024 * 1024 * 1024).CopyToAsync(fs);

        return file.Name;
    }
}