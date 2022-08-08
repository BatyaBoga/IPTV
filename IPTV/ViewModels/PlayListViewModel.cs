using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using IPTV.Models;
using Windows.Media.Core;
using IPTV.Services;

namespace IPTV.ViewModels
{
    public class PlayListViewModel : ViewModel
    {
        private List<Channel> channels;

        private string playListName;

        private int selectedIndex;

        private string searchText;

        public PlayListViewModel(LinksInfo playlist)
        {
            channels = playlist.ChannellList;

            playListName = playlist.Title;

            selectedIndex = 0;
        } 
        
        public List<Channel> Channels
        {
            get 
            {
                if(searchText == null)
                {
                    return channels;
                }

                return channels.Where(x => x.TvName.ToUpper().StartsWith(SearchText.ToUpper())).ToList();
            }
        }

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;

                OnPropertyChanged();

                OnPropertyChanged("SelectedChannel");
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
                searchText = value;

                OnPropertyChanged();

                OnPropertyChanged("Channels");
            }
        }

        public string PlayListName
        {
            get
            {
                return playListName;
            }
            set
            {
                playListName = value;

                OnPropertyChanged();
            }
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
            get => new RelayCommand((_)=> NavigationService.Instance.GoBack()); 
        }
    }
}
