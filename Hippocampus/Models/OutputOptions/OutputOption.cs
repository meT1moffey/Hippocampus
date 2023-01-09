using Hippocampus.Services;
using System;
using System.IO;

namespace Hippocampus.Models.OutputOptions
{
    public abstract class OutputOption
    {
        Func<FilePath> GetInputPath;
        Func<string> GetKey;

        protected Action<string> SetLabel;

        public OutputOption(BaseOutputConfig config)
        {
            GetInputPath = config.GetInputPath;
            GetKey = config.GetKey;
            SetLabel = config.SetLabel;
        }
        protected Stream LoadOutput()
        {
            if (!GetInputPath().Exists())
            {
                throw new FileNotFoundException("Input file does not exsist");
            }
            using (var file = GetInputPath().Download())
                return CoderService.Load(file, GetKey());
        }

        public abstract void ShowOutput();
        public abstract string GetName();
    }

    public struct BaseOutputConfig
    {
        public Action<string> SetLabel { get; set; }
        public Func<FilePath> GetInputPath { get; set; }
        public Func<string> GetKey { get; set; }

        public BaseOutputConfig(Action<string> _SetLabel, Func<FilePath> _GetInputPath, Func<string> _GetKey)
         {
            SetLabel = _SetLabel;
            GetInputPath = _GetInputPath;
            GetKey = _GetKey;
        }
    }
}
