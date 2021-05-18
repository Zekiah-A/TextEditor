namespace TextEditorAvalonia.ViewModels
{
    public class TextEditorViewModel : ViewModelBase
    {
        public string CurrentOpenedFilePath { set; get; } = "";
        public string CurrentOpenedFileContent { set; get; } = "";

        public bool IsEditingAFile { set; get; }
    }
}
