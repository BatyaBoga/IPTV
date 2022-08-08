using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IPTV.Constants;

namespace IPTV.Models
{
    public static class ChannelManager
    {
        public static async Task<List<Channel>> GetChanelsAsync(string path)
        {
            var chanels = new List<Channel>();

            string request;

            try
            {
               request = await HttpManager.GetRequestAsync(path);
            }
            catch(HttpRequestException)
            {
                return chanels;
            }

            foreach (Match m in Regex.Matches(request, Constant.RegexForChnaels))
            {
                chanels.Add(new Channel()
                {
                    TvLogo = m.Groups[2].Value ?? String.Empty,
                    TvName = m.Groups[3].Value ?? String.Empty,
                    TvStreamlink = m.Groups[4].Value ?? String.Empty
                });
            }

            return chanels;
        }
    }
}
