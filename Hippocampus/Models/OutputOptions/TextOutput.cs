using Hippocampus.Services;
using System;

namespace Hippocampus.Models.OutputOptions
{
    public class TextOutput : OutputOption
    {
        public TextOutput(BaseOutputConfig config) : base(config) { }
        public override void ShowOutput() => EditLabel(CoderService.ReadStream(LoadOutput()));
        public override string GetName() => "Text";
    }
}
