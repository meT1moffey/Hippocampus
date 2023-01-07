﻿using System;
using System.IO;
using System.Text;

namespace Hippocampus.Services
{
    internal static class HardDriveService
    {
        static public bool FileExsist(string path) => File.Exists(path);

        static public Stream Download(string path) => File.OpenRead(path);
        static public void Upload(Stream data, string path)
        {
            using(FileStream file = File.OpenWrite(path))
                data.CopyTo(file);
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

        static public void Delete(string path) => File.Delete(path);
    }
}
