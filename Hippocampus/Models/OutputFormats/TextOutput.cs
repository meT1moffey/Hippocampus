using Hippocampus.Services;

namespace Hippocampus.Models.OutputFormats
{
    public class TextOutput : OutputFormat
    {
        public TextOutput(BaseOutputConfig config) : base(config) { }
        public override void ShowOutput() => EditLabel(CoderService.ReadStream(LoadOutput()));
        public override string GetName() => "Text";
    }
}
