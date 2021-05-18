using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using TextEditorAvalonia.Services;

namespace TextEditorAvalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public TextEditorViewModel TextEditorViewModel { set; get; } = new TextEditorViewModel();

        private bool _isReadyToEdit;
        public bool IsReadyToEdit { set => this.RaiseAndSetIfChanged(ref _isReadyToEdit, value); get => _isReadyToEdit; }

        private string _applicationTitle = " - Text Editor";
        public string ApplicationTitle { set => this.RaiseAndSetIfChanged(ref _applicationTitle, value); get => _applicationTitle; }

        public ReactiveCommand<Unit, Unit> OpenFolderCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveFileCommand { get; }

        public MainWindowViewModel()
        {
            OpenFolderCommand = ReactiveCommand.CreateFromTask(HandleOpenFolderMenu);
            OpenFileCommand = ReactiveCommand.CreateFromTask(HandleOpenFileMenu);
            SaveFileCommand = ReactiveCommand.CreateFromTask(HandleSaveFileMenu);

            ApplicationTitle = Path.GetFileName(TextEditorViewModel.CurrentOpenedFilePath);
        }

        public async Task HandleOpenFolderMenu()
        {
            IOpenFolderDialogService openFolderDialogService = new OpenFolderDialogService();
            await openFolderDialogService.ShowAsync(GetWindowInstance());
        }

        public async Task HandleOpenFileMenu()
        {
            IOpenFileDialogService openFileDialogService = new OpenFileDialogService();
            await openFileDialogService.ShowAsync(GetWindowInstance());
        }

        public async Task HandleSaveFileMenu()
        {
            ISaveFileService saveFileService = new SaveFileService();
            await saveFileService.RequestSave(TextEditorViewModel.CurrentOpenedFilePath, TextEditorViewModel.CurrentOpenedFileContent);
        }

        private Window? GetWindowInstance()
        {
            return (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows.First(w => w.IsActive);
        }
    }
}
