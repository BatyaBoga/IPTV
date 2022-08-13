using System;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using Windows.Media.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPTV.Models.Model;
using IPTV.Interfaces;

namespace IPTV.ViewModels
{
    public class PlayListViewModel : ObservableObject
    {
        private List<Channel> channels;

        private string playListName;

        private int selectedIndex;

        private string searchText;

        private readonly INavigationService navigation;

        public PlayListViewModel(INavigationService navigation)
        {
            this.navigation = navigation;

            selectedIndex = 0;

            searchText = String.Empty;
        }
        
        public Playlist PlayList
        {
            set
            {
                channels = value.ChannelList;

                playListName = value.PlaylistTitle;
            }
        }
        
        public List<Channel> Channels
        {
            get => searchText == String.Empty ? channels : FilterChannels();
        }

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                if(SetProperty(ref selectedIndex, value))
                OnPropertyChanged(nameof(SelectedChannel));
            }
        }

        public string SearchText
        {
            get 
            { 
                return searchText; 
            }
            set 
            { 
                if(SetProperty(ref searchText, value))
                OnPropertyChanged(nameof(Channels));
            }
        }

        public string PlayListName
        {
            get => playListName; 
        }

        public MediaSource SelectedChannel
        {
            get 
            {
                var uri = new Uri((selectedIndex >= 0) ? Channels[selectedIndex].Stream : null);
                  
                var a = MediaSource.CreateFromUri(uri);
                return a;
            }
        }

        public ICommand ReturnBack
        {
            get => new RelayCommand(() => navigation.GoBack()); 
        }

        private List<Channel> FilterChannels()
        {
           return channels.Where(x => x.Title.ToUpper().StartsWith(SearchText.ToUpper())).ToList();
        }
    }
}
