using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IPTV.Models
{
    internal class ChannelManager
    {

        static HttpClient client = new HttpClient();

        private async Task<string> GetRequestAsync(string path)
        {

            HttpResponseMessage response = await client.GetAsync(path, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();

            }
            else
            {
                throw new HttpRequestException();
            }

        }


        public async Task<List<Chanel>> GetChanelsAsync(string path)
        {
            List<Chanel> chanels = new List<Chanel>();

            string request = String.Empty;

            try
            {
               request = await GetRequestAsync(path);
            }
            catch(HttpRequestException)
            {
                return chanels;
            }
            
            string pattern = @"tvg-logo=""(([^""]+)?)"".+,(.+)\s(https?\S+)";

            foreach (Match m in Regex.Matches(request, pattern))
            {
                chanels.Add(new Chanel()
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
