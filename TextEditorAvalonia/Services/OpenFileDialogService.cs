using Avalonia.Controls;
using System.IO;
using System.Threading.Tasks;

namespace TextEditorAvalonia.Services
{
    public interface IOpenFileDialogService
    {
        public Task ShowAsync(Window? windowInstance);

        public string FilePath { get; }
        public string FileContent { get; }
        public string FileName { get; }

        /// <summary>
        /// If the user clicks on the cancel button this becomes false.
        /// </summary>
        public bool IsSuccess { get; }
    }

    public class OpenFileDialogService : IOpenFileDialogService
    {
        public string FilePath { private set; get; } = "";
        public string FileContent { private set; get; } = "";
        public string FileName { private set; get; } = "";

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

            IsSuccess = true;
            FilePath = result[0];
            FileContent = await File.ReadAllTextAsync(result[0]);
            FileName = Path.GetFileName(result[0]);
        }
    }
}
