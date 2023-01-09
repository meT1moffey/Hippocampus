using Hippocampus.ViewModels;
using System;

namespace Hippocampus.Models.OutputOptions
{
    public class ImageOutput : OutputOption
    {
        public Action<ImageWindowViewModel> ShowImageWindow { get; }

        public ImageOutput(BaseOutputConfig config, Action<ImageWindowViewModel> _ShowImageWindow) : base(config)
            => ShowImageWindow = _ShowImageWindow;
        public override void ShowOutput()
        {
            ImageWindowViewModel imageWin;
            try
            {
                imageWin = new ImageWindowViewModel(LoadOutput());
            }
            catch (NullReferenceException)
            {
                EditLabel("Can't open image. Ensure key is correct");
                return;
            }
            ShowImageWindow(imageWin);
        }
        public override string GetName() => "Image";
    }
}
