using Hippocampus.Models;

namespace Hippocampus.ViewModels
{
    public class OutputOptionViewModel : ViewModelBase
    {
        OutputOption option;
        public string OptionName
        {
            get => option.option.ToString();
        }

        public OutputOptionViewModel(OutputOption _option) => option = _option;

        public OutputOptionEnum GetOption() => option.option;

    }
}