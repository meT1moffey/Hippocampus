using ReactiveUI;
using Avalonia.Media.Imaging;
using System.IO;
using LibVLCSharp.Shared;
using System;

namespace Hippocampus.ViewModels
{
    public class VideoWindowViewModel : ViewModelBase
    {

        readonly LibVLC _libVlc = new LibVLC();
        public MediaPlayer MediaPlayer { get; }

        public VideoWindowViewModel(Stream video)
        {
            MediaPlayer = new MediaPlayer(_libVlc);
            Play();
        }

        public void Play()
        {
            using var media = new Media(_libVlc, new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));
            MediaPlayer.Play(media);
        }

    }
}
