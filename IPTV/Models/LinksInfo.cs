using System.Collections.Generic;

namespace IPTV.Models
{
    public class LinksInfo
    {
        public string Link { get; set; }

        public string Title { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public  List<Channel> channellList { get; set; }
    }


    public class LinksInfoList
    {
        public List<LinksInfo> links { get; set; }
    }
}
