using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using IPTV.Models;
using Windows.Media.Core;
using IPTV.Services;
using CommunityToolkit.Mvvm.ComponentModel;

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
        
        public LinksInfo PlayList
        {
            set
            {
                channels = value.ChannellList;

                playListName = value.Title;
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
                var uri = new Uri((selectedIndex >= 0) ? Channels[selectedIndex].TvStreamlink : null);

                return MediaSource.CreateFromUri(uri); 
            }
        }

        public ICommand ReturnBack
        {
            get => new RelayCommand((_) => navigation.GoBack()); 
        }

        private List<Channel> FilterChannels()
        {
           return channels.Where(x => x.TvName.ToUpper().StartsWith(SearchText.ToUpper())).ToList();
        }
    }
}
