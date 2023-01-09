using Hippocampus.Models.FileLocations;
using Hippocampus.Models.OutputFormats;

namespace Hippocampus.ViewModels
{
    public class InputFormatViewModel : ViewModelBase
    {
        FileLocation format;
        public string FormatName
        {
            get => format.GetName();
        }

        public InputFormatViewModel(FileLocation _format) => format = _format;

        public FileLocation GetFormat() => format;

    }
}