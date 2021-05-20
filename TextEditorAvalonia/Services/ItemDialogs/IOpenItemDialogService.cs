using Avalonia.Controls;
using System.Threading.Tasks;
using TextEditorAvalonia.Models;

namespace TextEditorAvalonia.Services.ItemDialogs
{
    /// <summary>
    /// Responsible for opening a folder/file dialog.
    /// </summary>
    public interface IOpenItemDialogService
    {
        public Task ShowAsync(Window? windowInstance);

        public Item Item { get; }

        /// <summary>
        /// If the user clicks on the cancel button this becomes false.
        /// </summary>
        public bool IsSuccess { get; }
    }
}
