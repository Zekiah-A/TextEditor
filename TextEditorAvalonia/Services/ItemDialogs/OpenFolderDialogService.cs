using Avalonia.Controls;
using System.Threading.Tasks;
using TextEditorAvalonia.Models;

namespace TextEditorAvalonia.Services.ItemDialogs
{
    public class OpenFolderDialogService : IOpenItemDialogService
    {
        public Item Item { private set; get; } = new Item("", "");

        public bool IsSuccess { private set; get; }

        public async Task ShowAsync(Window? windowInstance)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();

            string result = await openFolderDialog.ShowAsync(windowInstance);

            IsSuccess = !string.IsNullOrEmpty(result);
            if (IsSuccess)
                Item = new Item(result);
        }
    }
}
