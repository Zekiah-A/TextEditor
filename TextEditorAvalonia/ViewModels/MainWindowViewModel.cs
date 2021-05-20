using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;
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
        private ThemeManagerService ThemeManagerService { get; } = new ThemeManagerService();

        private bool _canSaveFile;
        public bool CanSaveFile { set => this.RaiseAndSetIfChanged(ref _canSaveFile, value); get => _canSaveFile; }

        private bool _isLightTheme;
        public bool IsLightTheme { set => this.RaiseAndSetIfChanged(ref _isLightTheme, value); get => _isLightTheme; }

        // File menu
        public ReactiveCommand<Unit, Unit> OpenFolderCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveFileCommand { get; }

        // Settings menu
        public ReactiveCommand<bool, Unit> ChangeThemeCommand { get; }

        public MainWindowViewModel()
        {
            IsLightTheme = ThemeManagerService.ApplyTheme();

            this.WhenAnyValue(value => value.TextEditorViewModel.SelectedItem)
                .Subscribe(_ => CanSaveFile = TextEditorViewModel.SelectedItem != null);

            OpenFolderCommand = ReactiveCommand.CreateFromTask(HandleOpenFolderMenu);
            OpenFileCommand = ReactiveCommand.CreateFromTask(HandleOpenFileMenu);
            SaveFileCommand = ReactiveCommand.CreateFromTask(HandleSaveFileMenu);

            ChangeThemeCommand = ReactiveCommand.CreateFromTask<bool>(HandleChangeTheme);
        }

        public async Task HandleChangeTheme(bool isLightMode)
        {
            await ThemeManagerService.UpdateTheme(isLightMode);
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
            SaveFileService saveFileService = new SaveFileService();

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
