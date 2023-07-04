using Manager.WebApp.Settings;

namespace Manager.WebApp.Helpers
{
    public class CdnHelper
    {       
        public static string GetRootPathUrl()
        {
            try
            {
                return string.Format("{0}/{1}", CDNSettings.FileServerUrl, CDNSettings.FileServerRootPath);
            }
            catch
            {
                return CDNSettings.FileServerUrl;
            }
        }

        public static string GetFullFilePath(string url)
        {
            var baseUrl = GetRootPathUrl();
            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    if (url.Contains("http://") || url.Contains("https://"))
                    {
                        return url;
                    }
                }

                if (!string.IsNullOrEmpty(url))
                    url = url.Replace(baseUrl, string.Empty);

                return string.Format("{0}?url={1}", baseUrl, url);
            }
            catch
            {
                return url;
            }
        }
    }
}