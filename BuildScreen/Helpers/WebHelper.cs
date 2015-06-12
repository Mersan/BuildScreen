using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace BuildScreen.Helpers
{
    public class WebHelper
    {
        public static string GetFileContent(string url)
        {
            var client = new WebClient();
            try
            {
                var data = client.OpenRead(url);
                if (data != null)
                {
                    var reader = new StreamReader(data);
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return "No version info available.";
            }
            return string.Empty;
        }

        public static string GetVersionInfo(string buildurl)
        {
            if (!buildurl.EndsWith("/"))
                buildurl += "/";
            buildurl += "version.txt";
            return GetFileContent(buildurl);
        }

        protected static StringContent JsonContent(string content)
        {
            return new StringContent(content, Encoding.UTF8, "application/json");
        }
    }
}