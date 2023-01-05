using ReactiveUI;
using System.Reactive;
using Hippocampus.Services;
using System.Collections;

namespace Hippocampus.ViewModels
{
    public enum OutputFormat
    {
        ShowByLabel = 0,
        WriteToFile = 1,
    }

    public class MainViewModel : ViewModelBase
    {
        string filePath, key, labelOutput;
        OutputFormat outputFormat = OutputFormat.ShowByLabel;

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

        public string LabelOutput
        {
            get => labelOutput;
            set => this.RaiseAndSetIfChanged(ref labelOutput, value);
        }

        public ReactiveCommand<Unit, Unit> Launch { get; }
        public ReactiveCommand<Unit, Unit> LabelSelected { get; }
        public ReactiveCommand<Unit, Unit> FileSelected { get; }

        void ShowText(string text) => LabelOutput = text;
        void ShowOutput()
        {
            string output = Coder.Code(HardDrive.Read(FilePath), Key);
            switch (outputFormat)
            {
                case OutputFormat.ShowByLabel:
                    ShowText(output);
                    return;
                case OutputFormat.WriteToFile:
                    HardDrive.Write(FilePath + ".vo", output);
                    return;
            }
        }

        public MainViewModel()
        {
            var okEnabled = this.WhenAnyValue(
                x => x.FilePath,
                x => !string.IsNullOrWhiteSpace(x));

            Launch = ReactiveCommand.Create(() => ShowOutput(), okEnabled);

            LabelSelected = ReactiveCommand.Create(() =>
                { outputFormat = OutputFormat.ShowByLabel; });
            FileSelected = ReactiveCommand.Create(() =>
            { outputFormat = OutputFormat.WriteToFile; });
        }
    }
}
