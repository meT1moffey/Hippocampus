using ReactiveUI;

namespace Hippocampus.ViewModels
{
    public class HippocampusWindowViewModel : ViewModelBase
    {
        public MainViewModel main { get; }

        ViewModelBase content;

        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public HippocampusWindowViewModel()
        {
            Content = main = new MainViewModel(this);
        }

        public void ShowImage()
        {
            Content = new ImageViewModel();
        }
    }
}