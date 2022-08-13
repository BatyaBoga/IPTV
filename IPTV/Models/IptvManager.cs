using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IPTV.Constants;
using IPTV.Interfaces;
using IPTV.Models.Model;

namespace IPTV.Models
{
    public class IptvManager : IIptvManager
    {
        private IExplorer explorer;

        public IptvManager(IExplorer explorer)
        {
            this.explorer = explorer;

            Task.Run(async () => await ConfigExplorer()).Wait();
        }

        public async Task DeletePlaylist(ObservableCollection<Playlist> playlistCollection, Playlist playlist)
        {
            await explorer.DeleteFile(playlist.FileName);

            playlistCollection.Remove(playlist);
        }

        public async Task UpdatePlaylist(Playlist playlist)
        {
            playlist.ChannelList = await GetChannelsAsync(playlist.Link);

            await explorer.SaveToFile(playlist.FileName, JsonConvert.SerializeObject(playlist));
        }

        public async Task<Playlist> CreatePlaylist(string playlistTitle, string link)
        {
            var playlist = new Playlist()
            {
                PlaylistTitle = playlistTitle,

                Link = link,

                FileName = Guid.NewGuid().ToString() + ".json",

                ChannelList = await GetChannelsAsync(link)
            };

            return playlist;
        }

        public async Task EditPlaylist(ObservableCollection<Playlist> playlistCollection, Playlist playlist, int indexOfPlaylist)
        {
            await explorer.SaveToFile(playlist.FileName, JsonConvert.SerializeObject(playlist));

            playlistCollection[indexOfPlaylist] = playlist;
        }

        public async Task AddPlayList(ObservableCollection<Playlist> playlistCollection, Playlist playlist)
        {
            string textToFile = JsonConvert.SerializeObject(playlist);

            await explorer.SaveToNewFile(playlist.FileName, textToFile);

            playlistCollection.Add(playlist);
        }

        public async Task<List<Playlist>> GetPlaylistCollection()
        {
            var jsontext = await explorer.LoadFromFiles();

            var playlistCollection = new List<Playlist>();

            foreach (var item in jsontext)
            {
                if (item != String.Empty)
                {
                    playlistCollection.Add(JsonConvert.DeserializeObject<Playlist>(item));
                }
            }

            return playlistCollection;
        }

        private async Task<List<Channel>> GetChannelsAsync(string link)
        {
            string request = String.Empty;

            var channelList = new List<Channel>();

            try
            {
                request = await HttpManager.GetRequestAsync(link);
            }
            catch (HttpRequestException) { }

            foreach (Match m in Regex.Matches(request, Constant.RegexForChnaels))
            {
                channelList.Add(new Channel()
                {
                    Logo = m.Groups[2].Value ?? String.Empty,
                    Title = m.Groups[3].Value ?? String.Empty,
                    Stream = m.Groups[4].Value ?? String.Empty
                });
            }

            return channelList;
        }

        private async Task Preload()
        {
            await explorer.CreateFolder(Constant.IptvDataFolderName);

            string preloadtext = await Explorer.GetPreloadData();

            var jobject = JsonConvert.DeserializeObject<JToken>(preloadtext);

            foreach (var item in jobject["Links"])
            {
                var playlist = await CreatePlaylist(item["Playlist"].ToString(), item["Link"].ToString());
                await explorer.SaveToNewFile(playlist.FileName, JsonConvert.SerializeObject(playlist));
            }
        }

        private async Task ConfigExplorer()
        {
            try
            {
                await explorer.ConfigureFolder(Constant.IptvDataFolderName);
            }
            catch (FileNotFoundException)
            {
               await Preload();
            }
        }
    }
}
