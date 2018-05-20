using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Common
{
    public class ImageBaseDTO
    {
        public static string UrlPrefix
        { get; set; }

        public static string PaddingUrlPrefix(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {

                if (url.IndexOf("http://") >= 0 || url.IndexOf("https://") >= 0 || string.IsNullOrEmpty(UrlPrefix))
                {
                    return url;
                }
                else
                {
                    return string.Format("{0}/{1}", UrlPrefix.Trim().Trim('/'), url.Trim().Trim('/'));
                }
            }
            else
            {
                return url;
            }
        }

        public static string RemoveUrlPrefix(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {

                if (url.IndexOf("http://") >= 0 || url.IndexOf("https://") >= 0 || string.IsNullOrEmpty(UrlPrefix))
                {
                    return url.Replace(UrlPrefix, "");
                }
            }
            return url;
        }

    }
}
