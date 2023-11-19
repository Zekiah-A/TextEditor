using ReactiveUI;
using System.Collections.ObjectModel;
using TextEditorAvalonia.Models;

namespace TextEditorAvalonia.ViewModels;

public class TextEditorViewModel : ViewModelBase
{
    public ObservableCollection<Item> OpenedItems { set; get; } = new ObservableCollection<Item>();

    // Items in file explorer
    public ObservableCollection<Item> Items { set; get; } = new ObservableCollection<Item>();

    private Item? selectedItem;
    public Item? SelectedItem
    {
        set
        {
            this.RaiseAndSetIfChanged(ref selectedItem, value);
            if (!OpenedItems.Contains(SelectedItem!))
            {
                OpenedItems.Add(SelectedItem!);
            }
        }
        get => selectedItem;
    }
}