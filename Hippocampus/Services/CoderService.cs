using System;
using System.IO;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using HarfBuzzSharp;

namespace Hippocampus.Services
{
    internal static class CoderService
    {
        static public Stream Load(Stream data, string key)
        {
            Stream stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            byte[] buffer = new byte[1];

            if (string.IsNullOrEmpty(key)) key = "\0";
            
            for (int i = 0; data.Read(buffer, 0, buffer.Length) != 0; i++)
            {
                writer.Write((byte)(buffer[0] ^ key[i % key.Length]));
            }

            writer.Flush();
            stream.Position = 0;

            return stream;
        }

        static public string ReadStream(Stream stream)
        {
            string data = "";

            using (Stream s = stream)
            {
                byte[] buffer = new byte[stream.Length];

                while (s.Read(buffer, 0, buffer.Length) > 0)
                    data += new UTF8Encoding(true).GetString(buffer);
            }

            return data;
        }
    }
}
