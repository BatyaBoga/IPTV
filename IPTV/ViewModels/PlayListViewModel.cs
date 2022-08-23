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
        private Playlist playlist;

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
            get
            {
                return playlist;
            }
            set
            {
                SetProperty(ref playlist, value);
            }
        }
        
        public List<Channel> Channels
        {
            get => searchText == String.Empty ? playlist.ChannelList : FilterChannels();
        }

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                if(value >= 0 && SetProperty(ref selectedIndex, value))
                {
                    OnPropertyChanged(nameof(SelectedChannel));
                }                
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
                {
                    OnPropertyChanged(nameof(Channels));
                }
            }
        }

        public string PlayListName
        {
            get => playlist.PlaylistTitle; 
        }

        public MediaSource SelectedChannel
        {
            get => MediaSource.CreateFromUri(new Uri(Channels[selectedIndex].Stream));
        }

        public ICommand ReturnBack
        {
            get => new RelayCommand(() => navigation.GoBack()); 
        }

        private List<Channel> FilterChannels()
        {
           return playlist.ChannelList.Where(x => x.Title.ToUpper().StartsWith(SearchText.ToUpper())).ToList();
        }
    }
}
