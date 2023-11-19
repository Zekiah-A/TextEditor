using Avalonia;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TextEditorAvalonia.Services;

class ThemeManagerService
{
    private SaveFileService _saveFileService = new SaveFileService();

    // Not sure if it works cross-platform
    private readonly string _PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "TextEditorSelectedTheme.txt";

    private void AddTheme(bool isLightTheme)
    {
        /*Application.Current.Styles.Clear();
        FluentThemeMode mode = isLightTheme ? FluentThemeMode.Light : FluentThemeMode.Dark;
        FluentTheme fluentTheme = new FluentTheme(new Uri(@"avares://Avalonia.Themes.Fluent/FluentTheme.xaml")) { Mode = mode };
        Application.Current.Styles.Add(fluentTheme);*/
    }

    public bool ApplyTheme()
    {
        bool isLightTheme = false;
        if (File.Exists(_PATH))
        {
            isLightTheme = File.ReadAllText(_PATH).Contains("light");
            AddTheme(isLightTheme);
        }
        else
        {
            AddTheme(false);
            File.Create(_PATH);
        }
        return isLightTheme;
    }

    public async Task UpdateTheme(bool isLightTheme)
    {
        if (File.Exists(_PATH))
        {
            await _saveFileService.RequestSave(_PATH, isLightTheme ? "light" : "dark");
        }
        else
        {
            await _saveFileService.RequestSave(_PATH, "dark");
        }
    }
}