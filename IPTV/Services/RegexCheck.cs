using System.Text.RegularExpressions;
using IPTV.Constants;

namespace IPTV.Services
{
    public static class RegexCheck
    {
        public static bool IsLink(string link)
        {
            return Regex.IsMatch(link, Constant.RegexForLink);
        }

        public static bool IsTitle(string title)
        {
            return Regex.IsMatch(title, Constant.RegexForTitle);
        }
    }
}
