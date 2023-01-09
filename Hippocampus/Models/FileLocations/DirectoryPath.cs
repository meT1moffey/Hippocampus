using System.IO;

namespace Hippocampus.Models.FileLocations
{
    public class DirectoryPath : FileLocation
    {
        public DirectoryPath(string path = "") => Location = path;

        #region File methods
        public override bool Exists() => File.Exists(Location);

        public override Stream Download() => File.OpenRead(Location);
        public override void Upload(Stream data)
        {
            using (FileStream file = File.OpenWrite(Location))
                data.CopyTo(file);
        }
        public override void Delete() => File.Delete(Location);
        #endregion

        public override string GetName() => "From hard drive";
        public virtual bool AllowBrowseInput() => true;
    }
}
