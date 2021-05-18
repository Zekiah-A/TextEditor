using Avalonia.Controls;
using System.Threading.Tasks;

namespace TextEditorAvalonia.Services
{
    public interface IOpenFolderDialogService
    {
        public Task ShowAsync(Window? windowInstance);

        public string FolderPath { get; }

        /// <summary>
        /// If the user clicks on the cancel button this becomes false.
        /// </summary>
        public bool IsSuccess { get; }
    }

    public class OpenFolderDialogService : IOpenFolderDialogService
    {
        public string FolderPath { private set; get; } = "";

        public bool IsSuccess { private set; get; }

        public async Task ShowAsync(Window? windowInstance)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();

            string result = await openFolderDialog.ShowAsync(windowInstance);
            FolderPath = result;
            IsSuccess = !string.IsNullOrEmpty(result);
        }
    }
}
