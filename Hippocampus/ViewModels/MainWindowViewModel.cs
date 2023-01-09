﻿using Avalonia.Controls;
using Hippocampus.Models;
using Hippocampus.Models.OutputOptions;
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
        bool requestOutputPath;
        OutputOptionViewModel option;

        public Window win;
        #endregion

        #region Properties
        OutputOption[] OutputOptions
        {
            get
            {
                Action<string> SetLabel = (label) => LabelOutput = label;
                Action<ImageWindowViewModel> ShowImageWindow = (vm) => OpenImageWindow(vm);
                Func<FilePath> GetInputPath = () => (FilePath)InputPath,
                    GetOutputPath = () => (FilePath)OutputPath;
                Func<string> GetKey = () => Key;
                BaseOutputConfig config = new(SetLabel, GetInputPath, GetKey);

                return new OutputOption[] {
                    new TextOutput(config),
                    new ImageOutput(config, ShowImageWindow),
                    new FileOutput(config, GetOutputPath)
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
        public OutputOption Output
        {
            get => option.GetOption();
        }
        public OutputOptionViewModel SelectedOption
        {
            get => option;
            set
            {
                this.RaiseAndSetIfChanged(ref option, value);
                RequestOutputPath = option.GetOption().RequestOutputPath();
            }
        }
        public bool RequestOutputPath
        {
            get => requestOutputPath;
            set => this.RaiseAndSetIfChanged(ref requestOutputPath, value);
        }
        #endregion

        #region ReactiveUI varibles
        public ReactiveCommand<Unit, Unit> Launch { get; }
        public ReactiveCommand<Unit, string> BrowseInput { get; }
        public ReactiveCommand<Unit, string> BrowseOutput { get; }
        public Interaction<ImageWindowViewModel, MainWindowViewModel?> ImageWindowInteraction { get; }
        public ObservableCollection<OutputOptionViewModel> GetOptions { get; } = new();
        #endregion

        #region Methods
        void LoadOptions()
        {
            GetOptions.Clear();

            foreach(OutputOption o in OutputOptions)
            {
                var vm = new OutputOptionViewModel(o);

                GetOptions.Add(vm);
            }

            SelectedOption = GetOptions[0];
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
        #endregion

        public MainWindowViewModel()
        {
            ImageWindowInteraction = new Interaction<ImageWindowViewModel, MainWindowViewModel?>();
            var ready = this.WhenAnyValue(m => m.InputPath, i => ((FilePath)i).Exists());

            Launch = ReactiveCommand.Create(() => Output.ShowOutput(), ready);

            BrowseInput = ReactiveCommand.CreateFromTask(async ()
                => InputPath = await ShowFileBrowser() ?? InputPath);

            BrowseOutput = ReactiveCommand.CreateFromTask(async ()
                => OutputPath = await ShowFileBrowser() ?? OutputPath);

            LoadOptions();
        }
    }
}
