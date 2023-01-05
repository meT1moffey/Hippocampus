using ReactiveUI;

namespace Hippocampus.ViewModels
{
    class HippocampusWindowViewModel : ViewModelBase
    {
        public MainViewModel coder { get; }

        ViewModelBase content;

        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public HippocampusWindowViewModel()
        {
            Content = coder = new MainViewModel();
        }
    }
}