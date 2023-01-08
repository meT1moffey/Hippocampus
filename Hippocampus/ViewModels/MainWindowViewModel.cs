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
    public class MainWindowViewModel : ViewModelBase
    {
        string inputPathText, outputPathText, key, labelOutput;
        OutputFormat outputFormat;
        
        public Window win;

        public string InputPathText
        {
            get => inputPathText;
            set => this.RaiseAndSetIfChanged(ref inputPathText, value);
        }

        public FilePath InputPath
        {
            get => (FilePath)inputPathText;
        }

        public string Key
        {
            get => key;
            set => this.RaiseAndSetIfChanged(ref key, value);
        }
        public string OutputPathText
        {
            get => outputPathText;
            set => this.RaiseAndSetIfChanged(ref outputPathText, value);
        }
        public FilePath OutputPath
        {
            get => (FilePath)OutputPathText;
        }
        public string LabelOutput
        {
            get => labelOutput;
            set => this.RaiseAndSetIfChanged(ref labelOutput, value);
        }
        public ReactiveCommand<Unit, Unit> Launch { get; }
        public ReactiveCommand<string, OutputFormat> TypeSelected { get; }
        public ReactiveCommand<Unit, string> BrowseInput { get; }
        public ReactiveCommand<Unit, string> BrowseOutput { get; }
        public Interaction<ImageWindowViewModel, MainWindowViewModel?> ShowDialog { get; }

        public async Task<string> ShowFileBrowser()
            => (await new OpenFileDialog().ShowAsync(win))[0];

        void ShowText(string text) => LabelOutput = text;
        async Task ShowImage(Stream image)
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

        Stream LoadOutput()
        {
            if (!((FilePath)InputPath).Exists())
            {
                throw new FileNotFoundException("Input file does not exsist");
            }
            using (var file = ((FilePath)InputPath).Download())
                return CoderService.Load(file, Key);
        }

        void SaveOutputToFile()
        {
            if (OutputPath.Empty())
            {
                ShowText("To save file enter it's path");
                return;
            }
            ShowText("File saved as " + OutputPathText);
            OutputPath.Upload(LoadOutput());
        }

        void ShowOutput()
        {
            switch (outputFormat.format)
            {
                case FormatEnum.Text:
                    ShowText(CoderService.ReadStream(LoadOutput()));
                    return;
                case FormatEnum.Image:
                    ReactiveCommand.CreateFromTask(() => ShowImage(LoadOutput())).Execute();
                    return;
                case FormatEnum.File:
                    SaveOutputToFile();
                    return;
            }
        }

        public MainWindowViewModel()
        {
            ShowDialog = new Interaction<ImageWindowViewModel, MainWindowViewModel?>();
            var ready = this.WhenAnyValue(m => m.InputPathText, i => ((FilePath)i).Exists());

            Launch = ReactiveCommand.Create(() => ShowOutput(), ready);

            TypeSelected = ReactiveCommand.Create((string format) => outputFormat = format);

            BrowseInput = ReactiveCommand.CreateFromTask(async ()
                => InputPathText = await ShowFileBrowser());

            BrowseOutput = ReactiveCommand.CreateFromTask(async ()
                => OutputPathText = await ShowFileBrowser());
        }
    }
}
