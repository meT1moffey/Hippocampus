using Hippocampus.Models.OutputOptions;

namespace Hippocampus.ViewModels
{
    public class OutputFormatViewModel : ViewModelBase
    {
        OutputFormat format;
        public string FormatName
        {
            get => format.GetName();
        }

        public OutputFormatViewModel(OutputFormat _format) => format = _format;

        public OutputFormat GetOption() => format;

    }
}