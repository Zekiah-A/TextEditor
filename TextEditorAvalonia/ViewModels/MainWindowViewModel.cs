using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;
using TextEditorAvalonia.Models;
using TextEditorAvalonia.Services;
using TextEditorAvalonia.Services.ItemDialogs;

namespace TextEditorAvalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public TextEditorViewModel TextEditorViewModel { set; get; } = new TextEditorViewModel();

        private bool _canSaveFile;
        public bool CanSaveFile { set => this.RaiseAndSetIfChanged(ref _canSaveFile, value); get => _canSaveFile; }

        // File menu
        public ReactiveCommand<Unit, Unit> OpenFolderCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveFileCommand { get; }

        public MainWindowViewModel()
        {
            OpenFolderCommand = ReactiveCommand.CreateFromTask(HandleOpenFolderMenu);
            OpenFileCommand = ReactiveCommand.CreateFromTask(HandleOpenFileMenu);
            SaveFileCommand = ReactiveCommand.CreateFromTask(HandleSaveFileMenu);

            this.WhenAnyValue(value => value.TextEditorViewModel.SelectedItem)
                .Subscribe(_ => CanSaveFile = TextEditorViewModel.SelectedItem != null);
        }

        public async Task HandleOpenFolderMenu()
        {
            IOpenItemDialogService openFolderDialogService = new OpenFolderDialogService();
            await openFolderDialogService.ShowAsync(GetWindowInstance());
        }

        public async Task HandleOpenFileMenu()
        {
            IOpenItemDialogService openFileDialogService = new OpenFileDialogService();
            await openFileDialogService.ShowAsync(GetWindowInstance());

            if (openFileDialogService.IsSuccess)
            {
                foreach (Item item in TextEditorViewModel.Items)
                    if (item.Name == openFileDialogService.Item.Name)
                        return;

                TextEditorViewModel.Items.Add(openFileDialogService.Item);
            }
        }

        public async Task HandleSaveFileMenu()
        {
            ISaveFileService saveFileService = new SaveFileService();

            Item selectedItem = TextEditorViewModel.SelectedItem!;
            await saveFileService.RequestSave(selectedItem.Path, selectedItem.Content!);
        }

        private Window? GetWindowInstance()
        {
            // There will be one window for now. No need for LINQ
            return (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows[0];
        }
    }
}
