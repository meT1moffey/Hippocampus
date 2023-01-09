using Avalonia.ReactiveUI;
using Hippocampus.ViewModels;
using ReactiveUI;
using System.Threading.Tasks;

namespace Hippocampus.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WhenActivated(d => d(ViewModel!.ImageWindowInteraction.RegisterHandler(DoShowDialogAsync)));
        }

        private async Task DoShowDialogAsync(InteractionContext<ImageWindowViewModel, MainWindowViewModel?> interaction)
        {
            var dialog = new ImageWindow();
            dialog.DataContext = interaction.Input;

            var result = await dialog.ShowDialog<MainWindowViewModel?>(this);
            interaction.SetOutput(result);
        }
    }
}
