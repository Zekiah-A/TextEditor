using System;
using System.IO;
using System.Threading.Tasks;

namespace TextEditorAvalonia.Services;

public class SaveFileService
{
    public bool SaveSucceeded { private set; get; }

    public async Task RequestSave(string path, string content)
    {
        try
        {
            await using var streamWriter = new StreamWriter(path);
            await streamWriter.WriteAsync(content);
            SaveSucceeded = true;
        }
        catch (Exception exception) when (exception is DirectoryNotFoundException
            or IOException or UnauthorizedAccessException or ArgumentException)
        {
            SaveSucceeded = false;
        }
    }
}