using Avalonia.Controls;
using System.IO;
using System.Threading.Tasks;
using TextEditorAvalonia.Models;

namespace TextEditorAvalonia.Services.ItemDialogs;

public class OpenFileDialogService : IOpenItemDialogService
{
    public Item Item { private set; get; } = new Item("", "");

    public bool IsSuccess { private set; get; }

    public async Task ShowAsync(Window? windowInstance)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.AllowMultiple = false;

        string[] result = await openFileDialog.ShowAsync(windowInstance);
        if (result.Length < 1)
        {
            IsSuccess = false;
            return;
        }

        Item = new Item(result[0], await File.ReadAllTextAsync(result[0]));
        IsSuccess = true;
    }
}