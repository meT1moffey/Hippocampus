using System;
using System.IO;

namespace Hippocampus.Models.FileLocations
{
    public abstract class FileLocation
    {
        string location;

        public string Location
        {
            get => location;
            set => location = value;
        }

        #region Convertation operators
        public static implicit operator string(FileLocation location)
            => location != null ? location.Location : "";
        public override string ToString() => Location;
        #endregion

        #region File methods
        public abstract bool Exists();
        public bool Empty() => string.IsNullOrEmpty(location);

        public abstract Stream Download();
        public virtual void Upload(Stream data)
            => throw new InvalidCastException("This location type does not support edit files");
        public virtual void Delete()
            => throw new InvalidCastException("This location type does not support edit files");
        public abstract Stream DownloadCoded(string key);
        #endregion

        public abstract FileLocation MakeLocation(string location);
        public abstract string GetName();
        public virtual bool AllowBrowseInput() => false;
    }
}
