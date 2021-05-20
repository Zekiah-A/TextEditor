using System.Collections.ObjectModel;
using System.IO;

namespace TextEditorAvalonia.Models
{
    // An item could be a file or folder
    public class Item
    {
        // For folders
        public ObservableCollection<Item> SubItems { get; } = new ObservableCollection<Item>();

        public string Name { get; }
        public string Path { get; }
        public string? Content { set; get; }

        // File
        public Item(string path, string content)
        {
            Path = path;
            Content = content;
            Name = System.IO.Path.GetFileName(path);
        }

        // Folder
        public Item(string path)
        {
            Path = path;

            string[] subFolderPaths = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            foreach (string folderPath in subFolderPaths)
                SubItems.Add(new Item(folderPath));

            Name = System.IO.Path.GetFileName(path);
        }

    }
}
