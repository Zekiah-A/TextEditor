using Avalonia.Controls;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;
using TextEditorAvalonia.Models;

namespace TextEditorAvalonia.Services.ItemDialogs;

public class OpenFolderDialogService : IOpenItemDialogService
{
    public Item Item { private set; get; } = new Item("", "");

    public bool IsSuccess { private set; get; }

    public async Task ShowAsync(Window? windowInstance)
    {
        /*var openFolderDialog = new OpenFolderDialog();

        var result = await openFolderDialog.ShowAsync(windowInstance);

        IsSuccess = !string.IsNullOrEmpty(result);
        if (IsSuccess)
        {
            Item = new Item(result);
        }*/
    }
}