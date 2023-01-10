using Hippocampus.Models.FileLocations;
using Hippocampus.Services;
using System;
using System.IO;

namespace Hippocampus.Models.OutputFormats
{
    public abstract class OutputFormat
    {
        Func<FileLocation> GetInputFile;
        Func<string> GetKey;

        protected Action<string> EditLabel;

        public OutputFormat(BaseOutputConfig config)
        {
            GetInputFile = config.GetInputFile;
            GetKey = config.GetKey;
            EditLabel = config.EditLabel;
        }
        protected Stream LoadOutput()
        {
            if (!GetInputFile().Exists())
            {
                throw new FileNotFoundException("Input file does not exsist");
            }
            return GetInputFile().DownloadCoded(GetKey());
        }

        public abstract void ShowOutput();
        public abstract string GetName();
        public virtual bool RequestOutputPath() => false;
    }

    public struct BaseOutputConfig
    {
        public Action<string> EditLabel { get; set; }
        public Func<FileLocation> GetInputFile { get; set; }
        public Func<string> GetKey { get; set; }

        public BaseOutputConfig(Action<string> _SetLabel, Func<FileLocation> _GetInputFile, Func<string> _GetKey)
         {
            EditLabel = _SetLabel;
            GetInputFile = _GetInputFile;
            GetKey = _GetKey;
        }
    }
}
