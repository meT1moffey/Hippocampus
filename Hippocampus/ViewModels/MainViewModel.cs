using ReactiveUI;
using System.Reactive;
using Hippocampus.Services;
using System;
using System.IO;
using Avalonia.Controls.Shapes;
using System.Text;

namespace Hippocampus.ViewModels
{
    public enum OutputFormat
    {
        ShowByLabel = 0,
        WriteToFile = 1,
    }

    public enum FileType
    {
        Text = 0,
        Image = 1,
    }

    public class MainViewModel : ViewModelBase
    {
        string inputPath, key, outputPath, labelOutput;
        OutputFormat outputFormat = OutputFormat.ShowByLabel;
        FileType fileType = FileType.Text;
        HippocampusWindowViewModel win;

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
        public ReactiveCommand<string, Unit> TypeSelected { get; }

        void ShowText(string text) => LabelOutput = text;
        void LoadImage(Stream image)
        {
            win.ShowImage(image);
        }

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
                    switch (fileType) {
                        case FileType.Text:
                            ShowText(output);
                            return;
                        case FileType.Image:
                            LoadImage(HardDrive.Load(InputPath));
                            return;
                    }
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

        public MainViewModel(HippocampusWindowViewModel _win)
        {
            win = _win;

            var okEnabled = this.WhenAnyValue(
                m => m.InputPath,
                i => !string.IsNullOrEmpty(i)
                );

            Launch = ReactiveCommand.Create(() => ShowOutput(), okEnabled);

            OutputSelected = ReactiveCommand.Create((string _outputFormat) =>
            { Enum.TryParse(_outputFormat, out outputFormat); });

            TypeSelected = ReactiveCommand.Create((string _fileType) =>
            { Enum.TryParse(_fileType, out fileType); });
        }
    }
}
