﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IPTV.Models;
using Windows.Media.Core;
using IPTV.Service;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace IPTV.ViewModels
{
    public class PlayListViewModel : ViewModel
    {
        private List<Channel> channels;

        private int selectedIndex;

        private string searchText;



        public PlayListViewModel(List<Channel> channels)
        {
            this.channels = channels;
            selectedIndex = 0;
        }
        
        public MediaSource SelectedChannel
        {
            get 
            {
                Uri uri = new Uri((selectedIndex >= 0) ? Channels[selectedIndex].TvStreamlink : null);
                return MediaSource.CreateFromUri(uri);
                
            }
        }


        public string SearchText
        {
            get { return searchText; }
            set { 
                searchText = value;
                OnPropertyChanged();
                OnPropertyChanged("Channels");
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }

            set
            {
                this.selectedIndex = value;
                OnPropertyChanged();
                OnPropertyChanged("SelectedChannel");
            }
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

        public ICommand ReturnBack
        {
            get
            {
                return new RelayCommand((_)=> {

                    NS.Instance.GoBack();
                    //NavigationService.CurrentInstance.GoBack();
                });
            }
        }
    }
}