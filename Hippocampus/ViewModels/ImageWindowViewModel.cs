using ReactiveUI;
using Avalonia.Media.Imaging;
using System.IO;

namespace Hippocampus.ViewModels
{
    public class ImageWindowViewModel : ViewModelBase
    {
        Bitmap? image;

        public Bitmap? Image
        {
            get => image;
            private set => this.RaiseAndSetIfChanged(ref image, value);
        }

        public ImageWindowViewModel(Stream image)
        {
            Image = Bitmap.DecodeToHeight(image, 1000);
        }
    }
}
