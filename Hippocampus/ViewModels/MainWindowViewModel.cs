using ReactiveUI;

namespace Hippocampus.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        public CoderViewModel coder { get; }

        ViewModelBase content;

        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public MainWindowViewModel()
        {
            Content = coder = new CoderViewModel();
        }
    }
}