using ReactiveUI;
using System.Reactive;
using Hippocampus.Services;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Hippocampus.Models;
using System.Collections.ObjectModel;

namespace Hippocampus.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        string inputPathText, outputPathText, key, labelOutput;
        OutputOptionViewModel selectedOutputVM;
        
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
        public OutputOptionViewModel SelectedOutputVM
        {
            get => selectedOutputVM;
            set => this.RaiseAndSetIfChanged(ref selectedOutputVM, value);
        }
        public OutputOptionEnum SelectedOutput
        {
            get => selectedOutputVM.GetOption();
        }
        public ReactiveCommand<Unit, Unit> Launch { get; }
        public ReactiveCommand<string, OutputOption> TypeSelected { get; }
        public ReactiveCommand<Unit, string> BrowseInput { get; }
        public ReactiveCommand<Unit, string> BrowseOutput { get; }
        public Interaction<ImageWindowViewModel, MainWindowViewModel?> ShowDialog { get; }
        public ObservableCollection<OutputOptionViewModel> GetOptions { get; } = new();

        void LoadOptions()
        {
            GetOptions.Clear();

            foreach(OutputOption o in OutputOption.GetAllOptions())
            {
                var vm = new OutputOptionViewModel(o);

                GetOptions.Add(vm);
            }

            SelectedOutputVM = GetOptions[0];
        }

        async Task<string> ShowFileBrowser()
            => (await new OpenFileDialog().ShowAsync(win))[0];

        Stream LoadOutput()
        {
            if (!InputPath.Exists())
            {
                throw new FileNotFoundException("Input file does not exsist");
            }
            using (var file = InputPath.Download())
                return CoderService.Load(file, Key);
        }

        void ShowText() => LabelOutput = CoderService.ReadStream(LoadOutput());

        async Task OpenImage(Stream image)
        {
            ImageWindowViewModel imageWin;
            try
            {
                imageWin = new ImageWindowViewModel(image);
            }
            catch (NullReferenceException)
            {
                LabelOutput = "Can't open image. Ensure key is correct";
                return;
            }

            await ShowDialog.Handle(imageWin);
        }

        void ShowImage()
            => ReactiveCommand.CreateFromTask(() => OpenImage(LoadOutput())).Execute();

        void SaveOutputToFile()
        {
            if (OutputPath.Empty())
            {
                LabelOutput = "To save file enter it's path";
                return;
            }
            LabelOutput = "File saved as " + OutputPathText;
            OutputPath.Upload(LoadOutput());
        }

        void ShowOutput()
        {
            switch (SelectedOutput)
            {
                case OutputOptionEnum.Text:
                    ShowText();
                    return;
                case OutputOptionEnum.Image:
                    ShowImage();
                    return;
                case OutputOptionEnum.File:
                    SaveOutputToFile();
                    return;
            }
        }

        public MainWindowViewModel()
        {
            ShowDialog = new Interaction<ImageWindowViewModel, MainWindowViewModel?>();
            var ready = this.WhenAnyValue(m => m.InputPathText, i => ((FilePath)i).Exists());

            Launch = ReactiveCommand.Create(() => ShowOutput(), ready);

            BrowseInput = ReactiveCommand.CreateFromTask(async ()
                => InputPathText = await ShowFileBrowser());

            BrowseOutput = ReactiveCommand.CreateFromTask(async ()
                => OutputPathText = await ShowFileBrowser());

            LoadOptions();
        }
    }
}
