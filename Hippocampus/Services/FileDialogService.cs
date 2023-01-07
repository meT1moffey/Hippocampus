using Avalonia.Controls;
using System.Threading.Tasks;

namespace Hippocampus.Services
{
    static class FileDialogService
    {
        static public Task<string[]?> ShowOpenFileDialog(Window parent)
            => new OpenFileDialog().ShowAsync(parent);
    }
}
