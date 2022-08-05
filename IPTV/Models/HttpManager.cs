using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace IPTV.Models
{
    public class HttpManager
    {
        private static HttpClient client = new HttpClient();

        private static async Task<string> GetRequestAsync(string path)
        {

            var response = await client.GetAsync(path, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();

            }
            else
            {
                throw new HttpRequestException();
            }

        }
    }
}
