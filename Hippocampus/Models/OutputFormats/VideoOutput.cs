using System;
using Hippocampus.ViewModels;

namespace Hippocampus.Models.OutputFormats
{
    public class VideoOutput : OutputFormat
    {
        public Action<VideoWindowViewModel> ShowVideoWindow { get; }

        public VideoOutput(BaseOutputConfig config, Action<VideoWindowViewModel> _ShowVideoWindow) : base(config)
            => ShowVideoWindow = _ShowVideoWindow;

        public override void ShowOutput()
        {
            VideoWindowViewModel videoWin;
            try
            {
                videoWin = new VideoWindowViewModel(LoadOutput());
            }
            catch (NullReferenceException)
            {
                EditLabel("Can't open video. Ensure key is correct");
                return;
            }
            ShowVideoWindow(videoWin);
        }
        public override string GetName() => "Video (Alpha)";
    }
}
