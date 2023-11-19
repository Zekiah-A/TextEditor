using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Styling;

namespace TextEditorAvalonia.Services;

class ThemeManagerService
{
    private readonly SaveFileService saveFileService = new SaveFileService();

    // Not sure if it works cross-platform
    private readonly string themePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "TextEditorAvaloniaTheme.txt");
    
    private void SetCurrentTheme(bool isLightTheme)
    {
        if (Application.Current != null)
        {
            Application.Current.RequestedThemeVariant = isLightTheme ? ThemeVariant.Light : ThemeVariant.Dark;
        }
    }

    public bool ApplyTheme()
    {
        var isLightTheme = false;
        if (File.Exists(themePath))
        {
            isLightTheme = File.ReadAllText(themePath).Contains("light");
            SetCurrentTheme(isLightTheme);
        }
        else
        {
            SetCurrentTheme(false);
            File.Create(themePath);
        }
        
        return isLightTheme;
    }

    public async Task UpdateTheme(bool isLightTheme)
    {
        SetCurrentTheme(isLightTheme);
        
        if (File.Exists(themePath))
        {
            await saveFileService.RequestSave(themePath, isLightTheme ? "light" : "dark");
        }
        else
        {
            await saveFileService.RequestSave(themePath, "dark");
        }
    }
}