namespace Hippocampus.Services
{
    public class Coder
    {
        public Coder() { }

        public string Code(string data, string key)
        {
            string coded = "";

            for(int i = 0; i < data.Length; i++)
            {
                coded += (char)(data[i] ^ key[i % key.Length]);
            }

            return coded;
        }
    }
}
