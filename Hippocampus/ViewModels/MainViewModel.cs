using ReactiveUI;
using System.Reactive;
using Hippocampus.Services;
using System;

namespace Hippocampus.ViewModels
{
    public enum OutputFormat
    {
        ShowByLabel = 0,
        WriteToFile = 1,
    }

    public class MainViewModel : ViewModelBase
    {
        string inputPath, key, outputPath, labelOutput;
        OutputFormat outputFormat = OutputFormat.ShowByLabel;

        public string InputPath
        {
            get => inputPath;
            set => this.RaiseAndSetIfChanged(ref inputPath, value);
        }

        public string Key
        {
            get => key;
            set => this.RaiseAndSetIfChanged(ref key, value);
        }

        public string OutputPath
        {
            get => outputPath;
            set => this.RaiseAndSetIfChanged(ref outputPath, value);
        }

        public string LabelOutput
        {
            get => labelOutput;
            set => this.RaiseAndSetIfChanged(ref labelOutput, value);
        }

        public ReactiveCommand<Unit, Unit> Launch { get; }
        public ReactiveCommand<string, Unit> OutputSelected { get; }

        void ShowText(string text) => LabelOutput = text;
        void ShowOutput()
        {
            string output;
            if (string.IsNullOrEmpty(Key))
                output = HardDrive.Read(InputPath);
            else
                output = Coder.Code(HardDrive.Read(InputPath), Key);

            switch (outputFormat)
            {
                case OutputFormat.ShowByLabel:
                    ShowText(output);
                    return;
                case OutputFormat.WriteToFile:
                    if(string.IsNullOrEmpty(OutputPath))
                    {
                        ShowText("To save file enter it's path");
                        return;
                    }
                    ShowText("File saved as " + OutputPath);
                    HardDrive.Write(OutputPath, output);
                    return;
            }
        }

        public MainViewModel()
        {
            var okEnabled = this.WhenAnyValue(
                m => m.InputPath,
                i => !string.IsNullOrEmpty(i)
                );

            Launch = ReactiveCommand.Create(() => ShowOutput(), okEnabled);

            OutputSelected = ReactiveCommand.Create((string _outputFormat) =>
            { Enum.TryParse(_outputFormat, out outputFormat); });
        }
    }
}
