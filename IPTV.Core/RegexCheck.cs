using System.Text.RegularExpressions;
using IPTV.Constants;
using IPTV.Interfaces;

namespace IPTV.Services
{
    public class RegexCheck : IRegexCheck
    {
        public bool IsLink(string link)
        {
            return Regex.IsMatch(link, Constant.RegexForLink);
        }

        public bool IsTitle(string title)
        {
            return Regex.IsMatch(title, Constant.RegexForTitle);
        }
    }
}
