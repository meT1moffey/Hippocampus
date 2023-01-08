using ReactiveUI;
using System.Reactive;
using Hippocampus.Services;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Hippocampus.Models;

namespace Hippocampus.ViewModels
{
    public class OutputOptionViewModel : ViewModelBase
    {
        OutputOption option;

        public OutputOptionViewModel(OutputOption _option) => option = _option;

        public OutputOptionEnum GetOption() => option.option;

        public string OptionName
        {
            get => option.option.ToString();
        }
    }
}