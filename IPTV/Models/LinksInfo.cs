using System.Collections.Generic;
using Newtonsoft.Json;

namespace IPTV.Models
{
    public class LinksInfo
    {
        public string Link { get; set; }

        public string Title { get; set; }

        [JsonIgnore]
        public  List<Channel> ChannellList { get; set; }
    }

    public class LinksInfoList
    {
        public List<LinksInfo> Links { get; set; }
    }
}
