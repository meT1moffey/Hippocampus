using System;

namespace Hippocampus.Models.OutputFormats
{
    public class VideoOutput : OutputFormat
    {
        public VideoOutput(BaseOutputConfig config) : base(config) { }
        public override void ShowOutput()
        {
            throw new NotImplementedException();
        }
        public override string GetName() => "Video";
    }
}
