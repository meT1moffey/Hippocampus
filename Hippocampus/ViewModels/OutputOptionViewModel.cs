using Hippocampus.Models.OutputOptions;

namespace Hippocampus.ViewModels
{
    public class OutputOptionViewModel : ViewModelBase
    {
        OutputOption option;
        public string OptionName
        {
            get => option.GetName();
        }

        public OutputOptionViewModel(OutputOption _option) => option = _option;

        public OutputOption GetOption() => option;

    }
}