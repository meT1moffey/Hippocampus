﻿using ReactiveUI;
using System.Reactive;
using Hippocampus.Services;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;

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

    public class MainWindowViewModel : ViewModelBase
    {
        string inputPath, key, outputPath, labelOutput;
        OutputFormat outputFormat = OutputFormat.ShowByLabel;
        FileType fileType = FileType.Text;

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
        public Interaction<ImageWindowViewModel, MainWindowViewModel?> ShowDialog { get; }

        void ShowText(string text) => LabelOutput = text;
        async Task LoadImage(Stream image)
        {
            var imageWin = new ImageWindowViewModel(image);

            await ShowDialog.Handle(imageWin);
        }

        void ShowOutput()
        {
            Stream output;
            if(!HardDrive.FileExsist(InputPath))
            {
                ShowText("This file does not excist");
                return;
            }
            using (var file = HardDrive.Download(InputPath))
                output = Coder.Load(file, Key);

            switch (outputFormat)
            {
                case OutputFormat.ShowByLabel:
                    switch (fileType) {
                        case FileType.Text:
                            ShowText(HardDrive.ReadStream(output));
                            return;
                        case FileType.Image:
                            ReactiveCommand.CreateFromTask(() => LoadImage(output)).Execute();
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
                    HardDrive.Upload(output, OutputPath);
                    return;
            }
        }

        public MainWindowViewModel()
        {
            var okEnabled = this.WhenAnyValue(
                m => m.InputPath,
                i => !string.IsNullOrEmpty(i)
                );

            ShowDialog = new Interaction<ImageWindowViewModel, MainWindowViewModel?>();
            Launch = ReactiveCommand.Create(() => ShowOutput(), okEnabled);

            OutputSelected = ReactiveCommand.Create((string _outputFormat) =>
            { Enum.TryParse(_outputFormat, out outputFormat); });

            TypeSelected = ReactiveCommand.Create((string _fileType) =>
            { Enum.TryParse(_fileType, out fileType); });
        }
    }
}