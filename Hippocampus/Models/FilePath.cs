using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System.Globalization;
using System;
using System.IO;

namespace Hippocampus.Models
{
    public class FilePath
    {
        public string path;

        public FilePath() => path = "";
        public FilePath(string _path) => path = _path;
        public static explicit operator FilePath(string path) => new FilePath(path);
        public static implicit operator string(FilePath path) => path != null ? path.path : "";
        public override string ToString() => path;

        public bool Exists() => File.Exists(path);
        public bool Empty() => string.IsNullOrEmpty(path);

        public Stream Download() => File.OpenRead(path);
        public void Upload(Stream data)
        {
            using (FileStream file = File.OpenWrite(path))
                data.CopyTo(file);
        }
        public void Delete() => File.Delete(path);
    }
}
