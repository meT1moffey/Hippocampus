using ReactiveUI;
using System.Reactive;
using Hippocampus.Services;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace Hippocampus.ViewModels
{
    public enum OutputType
    {
        Text = 0,
        Image = 1,
        File = 2,
    }

    public class MainWindowViewModel : ViewModelBase
    {
        string inputPath, key, outputPath, labelOutput;
        OutputType outputType = OutputType.Text;
        
        public Window win;

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
        public ReactiveCommand<Unit, string> BrowseInput { get; }
        public ReactiveCommand<Unit, string> BrowseOutput { get; }
        public Interaction<ImageWindowViewModel, MainWindowViewModel?> ShowDialog { get; }

        void ShowText(string text) => LabelOutput = text;
        async Task LoadImage(Stream image)
        {
            ImageWindowViewModel imageWin;
            try
            {
                imageWin = new ImageWindowViewModel(image);
            }
            catch (NullReferenceException)
            {
                ShowText("Can't open image. Ensure key is correct");
                return;
            }

            await ShowDialog.Handle(imageWin);
        }

        void ShowOutput()
        {
            Stream output;
            if(!HardDriveService.FileExsist(InputPath))
            {
                ShowText("This file does not excist");
                return;
            }
            using (var file = HardDriveService.Download(InputPath))
                output = CoderService.Load(file, Key);

            switch (outputType)
            {
                case OutputType.Text:
                    ShowText(HardDriveService.ReadStream(output));
                    return;
                case OutputType.Image:
                    ReactiveCommand.CreateFromTask(() => LoadImage(output)).Execute();
                    return;
                case OutputType.File:
                    if(string.IsNullOrEmpty(OutputPath))
                    {
                        ShowText("To save file enter it's path");
                        return;
                    }
                    ShowText("File saved as " + OutputPath);
                    HardDriveService.Upload(output, OutputPath);
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

            TypeSelected = ReactiveCommand.Create((string _fileType) =>
            { Enum.TryParse(_fileType, out outputType); });

            BrowseInput = ReactiveCommand.CreateFromTask(async() =>
                InputPath = (await FileDialogService.ShowOpenFileDialog(win))[0]);

            BrowseOutput = ReactiveCommand.CreateFromTask(async () =>
                OutputPath = (await FileDialogService.ShowOpenFileDialog(win))[0]);
        }
    }
}
