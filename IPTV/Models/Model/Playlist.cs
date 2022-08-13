using System.Collections.Generic;

namespace IPTV.Models.Model
{
    public class Playlist
    {
        public string PlaylistTitle { get; set; }

        public List<Channel> ChannelList { get; set; }

        public string Link { get; set; }

        public string FileName { get; set; }
    }
}
