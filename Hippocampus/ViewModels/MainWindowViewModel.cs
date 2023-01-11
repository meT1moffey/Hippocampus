using Avalonia.Controls;
using Hippocampus.Models.FileLocations;
using Hippocampus.Models.OutputFormats;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Hippocampus.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Varibles
        string inputPath, outputPath, key, labelOutput;
        bool requestOutputPath, allowBrowseInput;
        OutputFormatViewModel outputFormat;
        InputFormatViewModel inputFormat;

        public Window win;
        #endregion

        #region Properties
        OutputFormat[] OutputFormats
        {
            get
            {
                Action<string> SetLabel = (label) => LabelOutput = label;
                Action<ImageWindowViewModel> ShowImageWindow = (vm) => OpenImageWindow(vm);
                Action<VideoWindowViewModel> ShowVideoWindow = (vm) => OpenVideoWindow(vm);
                Func<FileLocation> GetInputFile = () => Input.MakeLocation(InputPath);
                Func<DirectoryPath> GetOutputPath = () => new DirectoryPath(OutputPath);
                Func<string> GetKey = () => Key;
                BaseOutputConfig config = new(SetLabel, GetInputFile, GetKey);

                return new OutputFormat[] {
                    new TextOutput(config),
                    new ImageOutput(config, ShowImageWindow),
                    new FileOutput(config, GetOutputPath),
                    new VideoOutput(config, ShowVideoWindow)
                };
            }
        }

        FileLocation[] InputFormats
        {
            get
            {
                return new FileLocation[] {
                    new DirectoryPath(),
                    new UrlLink()
                };
            }
        }
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
        public OutputFormat Output
        {
            get => outputFormat.GetFormat();
        }
        public OutputFormatViewModel SelectedOutputFormat
        {
            get => outputFormat;
            set
            {
                this.RaiseAndSetIfChanged(ref outputFormat, value);
                RequestOutputPath = outputFormat.GetFormat().RequestOutputPath();
            }
        }
        public FileLocation Input
        {
            get => inputFormat.GetFormat();
        }
        public InputFormatViewModel SelectedInputFormat
        {
            get => inputFormat;
            set
            {
                this.RaiseAndSetIfChanged(ref inputFormat, value);
                AllowBrowseInput = inputFormat.GetFormat().AllowBrowseInput();
            }
        }
        public bool RequestOutputPath
        {
            get => requestOutputPath;
            set => this.RaiseAndSetIfChanged(ref requestOutputPath, value);
        }
        public bool AllowBrowseInput
        {
            get => allowBrowseInput;
            set => this.RaiseAndSetIfChanged(ref allowBrowseInput, value);
        }
        #endregion

        #region ReactiveUI varibles
        public ReactiveCommand<Unit, Unit> Launch { get; }
        public ReactiveCommand<Unit, string> BrowseInput { get; }
        public ReactiveCommand<Unit, string> BrowseOutput { get; }
        public Interaction<ImageWindowViewModel, MainWindowViewModel?> ImageWindowInteraction { get; } = new();
        public Interaction<VideoWindowViewModel, MainWindowViewModel?> VideoWindowInteraction { get; } = new();
        public ObservableCollection<OutputFormatViewModel> GetOutputFormats { get; } = new();
        public ObservableCollection<InputFormatViewModel> GetInputFormats { get; } = new();
        #endregion

        #region Methods
        void LoadOutputFormats()
        {
            GetOutputFormats.Clear();

            foreach(OutputFormat o in OutputFormats)
            {
                var vm = new OutputFormatViewModel(o);

                GetOutputFormats.Add(vm);
            }

            SelectedOutputFormat = GetOutputFormats[0];
        }

        void LoadInputFormats()
        {
            GetInputFormats.Clear();

            foreach (FileLocation i in InputFormats)
            {
                var vm = new InputFormatViewModel(i);

                GetInputFormats.Add(vm);
            }

            SelectedInputFormat = GetInputFormats[0];
        }

        async Task<string?> ShowFileBrowser()
        {
            string[]? browsedFile = await new OpenFileDialog().ShowAsync(win);
            if (browsedFile != null) return browsedFile[0];
            else return null;
        }

        public void OpenImageWindow(ImageWindowViewModel imageWin)
            => ReactiveCommand.CreateFromTask(async ()
                => await ImageWindowInteraction.Handle(imageWin)).Execute();

        public void OpenVideoWindow(VideoWindowViewModel videoWin)
            => ReactiveCommand.CreateFromTask(async ()
                => await VideoWindowInteraction.Handle(videoWin)).Execute();
        #endregion

        public MainWindowViewModel()
        {
            if (Program.Args.Length >= 2)
                InputPath = Program.Args[1];

            LoadOutputFormats();
            LoadInputFormats();

            var ready = this.WhenAnyValue(m => m.InputPath, i => Input.MakeLocation(i).Exists());

            Launch = ReactiveCommand.Create(() => Output.ShowOutput(), ready);

            BrowseInput = ReactiveCommand.CreateFromTask(async ()
                => InputPath = await ShowFileBrowser() ?? InputPath);

            BrowseOutput = ReactiveCommand.CreateFromTask(async ()
                => OutputPath = await ShowFileBrowser() ?? OutputPath);
        }
    }
}
