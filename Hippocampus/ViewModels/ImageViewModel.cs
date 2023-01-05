using ReactiveUI;
using System.Reactive;
using Hippocampus.Services;
using System;

namespace Hippocampus.ViewModels
{
    public class ImageViewModel : ViewModelBase
    {
        string source = "C:\\Tima\\Scripti\\Hippocampus\\Hippocampus\\TestImage.png";

        public string Source
        {
            get => source;
            set => this.RaiseAndSetIfChanged(ref source, value);
        }

        public ImageViewModel()
        {

        }
    }
}
