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
    public enum OutputType
    {
        Text = 0,
        Image = 1,
        File = 2,
    }

    public class MainWindowViewModel : ViewModelBase
    {
        string inputPath;
        string outputPath;
        string key, labelOutput;
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
        public ReactiveCommand<string, Unit> TypeSelected { get; }
        public ReactiveCommand<Unit, string> BrowseInput { get; }
        public ReactiveCommand<Unit, string> BrowseOutput { get; }
        public Interaction<ImageWindowViewModel, MainWindowViewModel?> ShowDialog { get; }

        static public Task<string[]?> ShowFileBrowser(Window parent)
            => new OpenFileDialog().ShowAsync(parent);

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
            if (((FilePath)OutputPath).Empty())
            {
                ShowText("To save file enter it's path");
                return;
            }
            ShowText("File saved as " + OutputPath);
            ((FilePath)OutputPath).Upload(LoadOutput());
        }

        void ShowOutput()
        {
            switch (outputType)
            {
                case OutputType.Text:
                    ShowText(CoderService.ReadStream(LoadOutput()));
                    return;
                case OutputType.Image:
                    ReactiveCommand.CreateFromTask(() => ShowImage(LoadOutput())).Execute();
                    return;
                case OutputType.File:
                    SaveOutputToFile();
                    return;
            }
        }

        public MainWindowViewModel()
        {
            ShowDialog = new Interaction<ImageWindowViewModel, MainWindowViewModel?>();
            var ready = this.WhenAnyValue(m => m.InputPath, i => ((FilePath)i).Exists());

            Launch = ReactiveCommand.Create(() => ShowOutput(), ready);

            TypeSelected = ReactiveCommand.Create((string _fileType) =>
            { Enum.TryParse(_fileType, out outputType); });

            BrowseInput = ReactiveCommand.CreateFromTask(async() =>
                InputPath = (await ShowFileBrowser(win))[0]);

            BrowseOutput = ReactiveCommand.CreateFromTask(async () =>
                OutputPath = (await ShowFileBrowser(win))[0]);
        }
    }
}
