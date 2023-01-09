using Hippocampus.Services;
using System;
using System.IO;

namespace Hippocampus.Models.OutputOptions
{
    public abstract class OutputOption
    {
        Func<FilePath> GetInputPath;
        Func<string> GetKey;

        protected Action<string> EditLabel;

        public OutputOption(BaseOutputConfig config)
        {
            GetInputPath = config.GetInputPath;
            GetKey = config.GetKey;
            EditLabel = config.EditLabel;
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
        public virtual bool RequestOutputPath() => false;
    }

    public struct BaseOutputConfig
    {
        public Action<string> EditLabel { get; set; }
        public Func<FilePath> GetInputPath { get; set; }
        public Func<string> GetKey { get; set; }

        public BaseOutputConfig(Action<string> _SetLabel, Func<FilePath> _GetInputPath, Func<string> _GetKey)
         {
            EditLabel = _SetLabel;
            GetInputPath = _GetInputPath;
            GetKey = _GetKey;
        }
    }
}
