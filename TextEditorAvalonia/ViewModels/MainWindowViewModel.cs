using ReactiveUI;
using System.IO;

namespace TextEditorAvalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public TextEditorViewModel TextEditorViewModel { set; get; } = new TextEditorViewModel();

        private bool _isReadyToEdit;
        public bool IsReadyToEdit { set => this.RaiseAndSetIfChanged(ref _isReadyToEdit, value); get => _isReadyToEdit; } 

        private string _applicationTitle = "";
        public string ApplicationTitle { set => this.RaiseAndSetIfChanged(ref _applicationTitle, value); get => _applicationTitle; }

        public MainWindowViewModel()
        {
            ApplicationTitle = Path.GetFileName(TextEditorViewModel.CurrentOpenedFilePath);
        }
    }
}
