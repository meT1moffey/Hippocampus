using System.IO;

namespace Hippocampus.Services
{
    internal static class CoderService
    {
        static public Stream Load(Stream data, string key)
        {
            Stream stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            if (string.IsNullOrEmpty(key)) key = "\0";
            
            for (int i = 0; i < data.Length; i++)
            {
                writer.Write((byte)(data.ReadByte() ^ key[i % key.Length]));
            }

            writer.Flush();
            stream.Position = 0;

            return stream;
        }
    }
}
