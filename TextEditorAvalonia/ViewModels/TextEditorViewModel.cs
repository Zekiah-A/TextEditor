using ReactiveUI;
using System.Collections.ObjectModel;
using TextEditorAvalonia.Models;

namespace TextEditorAvalonia.ViewModels
{
    public class TextEditorViewModel : ViewModelBase
    {
        public ObservableCollection<Item> OpenedItems { set; get; } = new ObservableCollection<Item>();

        // Items in file explorer
        public ObservableCollection<Item> Items { set; get; } = new ObservableCollection<Item>();

        private Item? _selectedItem;
        public Item? SelectedItem
        {
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedItem, value);
                if (!OpenedItems.Contains(SelectedItem!))
                    OpenedItems.Add(SelectedItem!);
            }
            get => _selectedItem;
        }
    }
}
