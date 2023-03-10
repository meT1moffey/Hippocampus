using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Hippocampus.ViewModels;
using Hippocampus.Views;
using LibVLCSharp;
using LibVLCSharp.Shared;

namespace Hippocampus
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            Core.Initialize();
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };

                (desktop.MainWindow.DataContext as MainWindowViewModel).win = desktop.MainWindow;
            }
        }
    }
}
