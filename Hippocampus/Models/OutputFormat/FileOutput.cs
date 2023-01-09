using System;

namespace Hippocampus.Models.OutputOptions
{
    public class FileOutput : OutputFormat
    {
        Func<FilePath> GetOutputPath;

        public FileOutput(BaseOutputConfig config, Func<FilePath> _GetOutputPath) : base(config)
            => GetOutputPath = _GetOutputPath;

        public override void ShowOutput()
        {
            if (GetOutputPath().Empty())
            {
                EditLabel("To save file enter it's path");
                return;
            }
            EditLabel("File saved as " + GetOutputPath());
            GetOutputPath().Upload(LoadOutput());
        }

        public override string GetName() => "File";
        public override bool RequestOutputPath() => true;
    }
}
