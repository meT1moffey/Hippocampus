using System.IO;

namespace Hippocampus.Services
{
    internal static class CoderService
    {
        static public string Code(string data, string key)
        {
            string coded = "";
            if (string.IsNullOrEmpty(key)) key = "\0";

            for(int i = 0; i < data.Length; i++)
            {
                coded += (char)(data[i] ^ key[i % key.Length]);
            }

            return coded;
        }

        static public Stream Load(Stream data, string key)
        {
            Stream stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            byte[] buffer = new byte[data.Length];

            if (string.IsNullOrEmpty(key)) key = "\0";
            data.Read(buffer, 0, buffer.Length);
            for (int i = 0; i < data.Length; i++)
            {
                buffer[i] ^= (byte)key[i % key.Length];
            }

            writer.Write(buffer);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }
    }
}
