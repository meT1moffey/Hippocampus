using System.IO;
using System.Text;

namespace Hippocampus.Services
{
    internal static class HardDrive
    {
        static public string Read(string path)
        {
            string data = "";

            using (FileStream fs = File.OpenRead(path))
            {
                byte[] buffer = new byte[fs.Length];
                UTF8Encoding decoder = new UTF8Encoding(true);

                while (fs.Read(buffer, 0, buffer.Length) > 0)
                    data += decoder.GetString(buffer);
            }

            return data;
        }
    }
}
