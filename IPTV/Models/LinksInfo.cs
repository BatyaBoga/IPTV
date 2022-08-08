using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IPTV.Models
{
    public class LinksInfo : ICloneable
    {
        public string Link { get; set; }

        public string Title { get; set; }

        [JsonIgnore]
        public  List<Channel> ChannellList { get; set; }

        public object Clone()
        {
            return new LinksInfo()
            {
                Title = this.Title,
                Link = this.Link,
                ChannellList = this.ChannellList
            };
        }
    }

    public class LinksInfoList
    {
        public List<LinksInfo> Links { get; set; }
    }
}
