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
        public override Stream DownloadCoded(string key)
        {
            Stream stream = new MemoryStream(), data = Download();
            var writer = new BinaryWriter(stream);

            if (string.IsNullOrEmpty(key)) key = "\0";

            for (int i = 0; i < data.Length; i++)
            {
                writer.Write((byte)(data.ReadByte() ^ key[i % key.Length]));
            }

            writer.Flush();
            data.Close();
            stream.Position = 0;
            return stream;
        }
        #endregion

        public override FileLocation MakeLocation(string location) => new DirectoryPath(location);
        public override string GetName() => "From hard drive";
        public virtual bool AllowBrowseInput() => true;
    }
}
