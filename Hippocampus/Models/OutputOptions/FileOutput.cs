using System;

namespace Hippocampus.Models.OutputOptions
{
    public class FileOutput : OutputOption
    {
        Func<FilePath> GetOutputPath;

        public FileOutput(BaseOutputConfig config, Func<FilePath> _GetOutputPath) : base(config)
            => GetOutputPath = _GetOutputPath;

        public override void ShowOutput()
        {
            if (GetOutputPath().Empty())
            {
                SetLabel("To save file enter it's path");
                return;
            }
            SetLabel("File saved as " + GetOutputPath());
            GetOutputPath().Upload(LoadOutput());
        }

        public override string GetName() => "File";
    }
}
