using Hippocampus.Services;
using ReactiveUI;
using Avalonia.Media.Imaging;
using System.Threading.Tasks;
using System.IO;

namespace Hippocampus.ViewModels
{
    public class ImageWindowViewModel : ViewModelBase
    {
        string source = "C:\\Tima\\Scripti\\Hippocampus\\Hippocampus\\TestImage.png";
        Bitmap? image;

        public Bitmap? Image
        {
            get => image;
            private set => this.RaiseAndSetIfChanged(ref image, value);
        }
        public string Source
        {
            get => source;
            set => this.RaiseAndSetIfChanged(ref source, value);
        }

        public ImageWindowViewModel(Stream image)
        {
            Image = Bitmap.DecodeToHeight(image, 720);
        }
    }
}
