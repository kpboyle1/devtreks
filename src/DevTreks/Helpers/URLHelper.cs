using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Configuration;

namespace DevTreks.Data.Helpers
{
    public static class URLHelper
    {
        
        public static string GetSource(this UrlHelper helper, string url)
        {
            Uri uri;
            string source = "Unknown";
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                if (uri.IsAbsoluteUri)
                {
                    string authority = uri.Authority;
                    if (authority.EndsWith("blob.core.windows.net"))
                    {
                        source = "Blob Service";
                    }
                    else if (authority.EndsWith("vo.msecnd.net"))
                    {
                        source = "CDN";
                    }
                    else if (authority.Contains("127.0.0.1"))
                    {
                        source = "Emulator";
                    }
                }
                else
                {
                    source = "Hosting Server";
                }
            }

            return source;
        }
    }
}
