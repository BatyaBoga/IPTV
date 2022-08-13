﻿using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using IPTV.Models.Model;

namespace IPTV.Interfaces
{
    public interface IIptvManager
    {
        Task<List<Playlist>> GetPlaylistCollection();

        Task<Playlist> CreatePlaylist(string playlistTitle, string link);

        Task AddPlayList(ObservableCollection<Playlist> playlistCollection, Playlist playlist);

        Task DeletePlaylist(ObservableCollection<Playlist> playlistCollection, Playlist playlist);

        Task EditPlaylist(ObservableCollection<Playlist> playlistCollection, Playlist playlist, int indexOfPlaylist);

        Task UpdatePlaylist(Playlist playlist);
    }
}
