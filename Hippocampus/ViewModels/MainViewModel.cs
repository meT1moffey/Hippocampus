using ReactiveUI;
using System.Reactive;
using Hippocampus.Services;

namespace Hippocampus.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        string filePath, key, output;

        public string FilePath
        {
            get => filePath;
            set => this.RaiseAndSetIfChanged(ref filePath, value);
        }

        public string Key
        {
            get => key;
            set => this.RaiseAndSetIfChanged(ref key, value);
        }

        public string Output
        {
            get => output;
            set => this.RaiseAndSetIfChanged(ref output, value);
        }

        public ReactiveCommand<Unit, Unit> Launch { get; }

        public void ShowText(string text) => Output = text;

        public MainViewModel()
        {
            var okEnabled = this.WhenAnyValue(
                x => x.FilePath,
                x => !string.IsNullOrWhiteSpace(x));

            Launch = ReactiveCommand.Create(() => ShowText(
                Coder.Code(HardDrive.Read(FilePath), Key)
                ), okEnabled);
        }
    }
}
