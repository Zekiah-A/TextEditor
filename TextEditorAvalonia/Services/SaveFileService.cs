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
            using StreamWriter streamWriter = new StreamWriter(path);
            await streamWriter.WriteAsync(content);
            SaveSucceeded = true;
        }
        catch (Exception exception) when (exception is DirectoryNotFoundException
            || exception is IOException || exception is UnauthorizedAccessException
            || exception is ArgumentException)
        {
            SaveSucceeded = false;
        }
    }
}