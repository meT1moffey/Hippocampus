using LibVLCSharp.Shared;
using System;
using System.IO;

namespace Hippocampus.ViewModels
{
    public class VideoWindowViewModel : ViewModelBase
    {

        readonly LibVLC _libVlc = new LibVLC();
        public MediaPlayer MediaPlayer { get; }

        public VideoWindowViewModel(Stream video)
        {
            MediaPlayer = new MediaPlayer(_libVlc);
            MediaPlayer.Media = new Media(_libVlc, new StreamMediaInput(video));
            MediaPlayer.Play();
        }
    }
}
