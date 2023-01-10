using System.IO;
using System;
using System.Net;
using System.Net.Http;

namespace Hippocampus.Models.FileLocations
{
    public class UrlLink : FileLocation
    {
        public UrlLink(string url="") => Location = url;

        #region File methods
        public override bool Exists() => true;

        public override Stream Download()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Location);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return response.GetResponseStream();
        }
        public override Stream DownloadCoded(string key)
        {
            DirectoryPath path = new(AppDomain.CurrentDomain.BaseDirectory + "/_cache_");
            Stream coded;

            path.Upload(Download());
            coded = path.DownloadCoded(key);
            path.Delete();

            return coded;
        }
        #endregion

        public override FileLocation MakeLocation(string location) => new UrlLink(location);
        public override string GetName() => "From web (Beta)";
    }
}
